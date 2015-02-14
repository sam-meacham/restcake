using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using RestCake.Clients;
using RestCake.Metadata;
using RestCake.Routing;
using RestCake.Util;

namespace RestCake
{
	public abstract class RestCakeHandler : RoutedHttpHandler
	{
		private static readonly string VERSION = "RestCake v1.4.0";

		/// <summary>
		/// All of the dictionaries in this class that have "Type" as the first type parameter store information for the class that inherits from this class.
		/// These dictionaries create a sort of "static inherited" data storage.
		/// The value of the "Type" in the outer dictionary is the type of derived class, so that the derived class has it's own Dictionary of storage (the inner dictionary).
		/// 
		/// The inner dictionaries are mappings: A UriTemplate that maps to a specific rest service method in the service class.
		/// </summary>
		private static readonly Dictionary<Type, bool> s_isMetadataInitialized = new Dictionary<Type, bool>();

		private static readonly Dictionary<Type, Dictionary<string, string>> s_jsProxies = new Dictionary<Type, Dictionary<string, string>>();

		/// <summary>
		/// Serializer settings for individual methods.  If there's no entry for a MethodMetadata key, we fall back to the setting for the service (see below dictionary)
		/// </summary>
		private static readonly Dictionary<MethodMetadata, JsonSerializerSettings> s_methodSerializerSettings = new Dictionary<MethodMetadata, JsonSerializerSettings>();

		/// <summary>
		/// Serializer settings for the whole service class.  If this is null, we'll use whatever we get from GetSerializer().
		/// </summary>
		private static readonly Dictionary<Type, JsonSerializerSettings> s_serviceSerializerSettings = new Dictionary<Type, JsonSerializerSettings>();

		/// <summary>
		/// In ProcessRequest, if the incoming url matches a regex in this dictionary, the normal processing will not occur.  Instead,
		/// the action (the value in the dictionary) will be invoked.
		/// </summary>
		private static readonly Dictionary<Type, Dictionary<Regex, Action<Match, Type, string, HttpContext>>> s_regexOverrides = new Dictionary<Type, Dictionary<Regex, Action<Match, Type, string, HttpContext>>>();


		/// <summary>
		/// Used for locking for thread safety when I first initialize a sub class's metadata.
		/// I don't know the specifics of the ASP.NET runtime, but there was a race condition previously in initializeMetadata(),
		/// that was causing either NullReferenceExceptions or "The given key is not present in the dictionary" exceptions. Hard to track
		/// down, but the lock fixes it.
		/// </summary>
		private static readonly object s_objSync = new object();


		/// <summary>
		/// sendErrorToClientAndCloseResponse() will set this value to true.  This value is specific to each request, as it uses
		/// HttpContext.Current.Items for storage.  This value can be useful in your hosting applications Application_Error handler (in Global.asax).
		/// If it's true, you may want to go ahead and call Server.ClearError(), since the error has been sent to the client and the response stream has already
		/// been closed anyway (if you DON'T clear the error, the asp.net runtime will just try to write a yellow screen of death to the closed reponse stream, though
		/// it doesn't seem to cause any noticeable errors...).  If it's not true, you might want to avoid calling Server.ClearError(), since no error was sent to the client
		/// (this usually means there was an error in RestCake -- possibly (probably?) a bug.  Report it!), and by not clearing the error you'll at least get a yellow
		/// screen of death that hopefully points you in the right direction.
		/// </summary>
		public static bool IsJsonErrorSent
		{
			get
			{
				if (HttpContext.Current.Items["RestCake_IsJsonErrorSent"] == null)
					return false;
				return (bool) HttpContext.Current.Items["RestCake_IsJsonErrorSent"];
			}

			private set { HttpContext.Current.Items["RestCake_IsJsonErrorSent"] = value; }
		}


		private JsonSerializerSettings m_jsonSettings;

		private JsonSerializer __serializer;
		protected JsonSerializer Serializer
		{
			get { return __serializer ?? (__serializer = JsonSerializer.Create(m_jsonSettings)); }
		}

		public HttpResponse Response { get; private set; }
		public HttpRequest Request { get; private set; }
		public HttpContext Context { get; private set; }
		public string RequestContentType { get; private set; }
		public string RequestVerb { get; private set; }
		public bool IsFormUrlEncoded { get; private set; }
		public bool IsXml { get; private set; }
		public bool IsJson { get; private set; }
		public Type ThisType { get; private set; }
		public string AbsBaseUrl { get; private set; }
		public string EverythingAfterRouteUrl { get; private set; }


		public string OriginHost { get; set; }

		public string Origin { get; set; }

		public string Host { get; set; }

		public bool IsCors { get; set; }
		public bool IsCorsPreflight { get; set; }

		private static readonly string JSON_CALLBACK_PARAM_NAME = "jsoncallback";

		private bool? __isJsonp;
		public bool IsJsonp
		{
			get
			{
				// This just caches the result of the expression, so it's only evaluated once.
				// Subsequent calls will just return __isJsonp, since it'll have a value.
				return (bool) (__isJsonp ?? (__isJsonp = !String.IsNullOrWhiteSpace(Request.QueryString[JSON_CALLBACK_PARAM_NAME])));
			}
		}

		// constructor
		protected RestCakeHandler()
		{
			ThisType = GetType();
		}


		// ********************************************************************************
		// *** Private methods ************************************************************
		// ********************************************************************************
		private string getStartJsonp()
		{
			if (!IsJsonp)
				return String.Empty;

			return string.Format("{0}(", Request.QueryString[JSON_CALLBACK_PARAM_NAME]);
		}

		private string getEndJsonp()
		{
			if (!IsJsonp)
				return String.Empty;

			return ")";
		}

		private void writeJsonResponse(string json)
		{
			Response.Write(getStartJsonp());
			Response.ContentType = Constants.ContentTypeJson;
			Response.Write(json);
			Response.Write(getEndJsonp());
		}

		private void writeJsonResponse(object obj)
		{
			Response.Write(getStartJsonp());
			Response.ContentType = Constants.ContentTypeJson;
			StringWriter stringWriter = new StringWriter();
			Serializer.Serialize(stringWriter, obj);
			Response.Write(stringWriter.ToString());
			Response.Write(getEndJsonp());
		}

		private void writeBareResponse(object obj)
		{
			Response.Write(getStartJsonp());
			Response.Write(obj);
			Response.Write(getEndJsonp());
		}


		private static void initializeMetadata(Type type)
		{
			// Dirty check (not thread-safe)
			if (s_isMetadataInitialized.ContainsKey(type) && s_isMetadataInitialized[type])
				return;

			lock (s_objSync)
			{
				// Redundant check; thread-safe this time
				if (s_isMetadataInitialized.ContainsKey(type) && s_isMetadataInitialized[type])
					return;

				// Set up a "regex backdoor" for the js and clr clients, and the help page
				s_regexOverrides[type] = new Dictionary<Regex, Action<Match, Type, string, HttpContext>>();

				// "amd2" client, in hindsight, I realized I shouldn't return the class, but a working client
				s_regexOverrides[type][new Regex(@"^/_js\?type=amd2.*$")] = returnAmd2JsClientDefinition;

				// original amd module (requirejs), returns the client class.
				s_regexOverrides[type][new Regex(@"^/_js\?type=amd.*$")] = returnAmdClientDefinition;

				s_regexOverrides[type][new Regex(@"^/_js\?(?<type>\w+).*$")] = returnJsClientDefinition;

				s_regexOverrides[type][new Regex(@"^/_cs\?type=(?<type>\w+)&namespace=(?<namespace>\w+)$")] = returnClrClientDefinition;
				s_regexOverrides[type][new Regex(@"^/_help")] = returnHelpPage;

				// Set up the js proxies cache for this type
				s_jsProxies[type] = new Dictionary<string, string>();

				// Get serialization settings from [JsonNetSettings] attributes, on both service classes and methods (the attribute is valid on both)
				JsonNetSettings serviceJsonSettings = ReflectionHelper.GetAttribute<JsonNetSettings>(type);
				if (serviceJsonSettings != null)
				{
					JsonSerializerSettings settings = new JsonSerializerSettings()
					{
						ConstructorHandling = serviceJsonSettings.ConstructorHandling,
						DefaultValueHandling = serviceJsonSettings.DefaultValueHandling,
						MissingMemberHandling = serviceJsonSettings.MissingMemberHandling,
						NullValueHandling = serviceJsonSettings.NullValueHandling,
						ObjectCreationHandling = serviceJsonSettings.ObjectCreationHandling,
						PreserveReferencesHandling = serviceJsonSettings.PreserveReferencesHandling,
						ReferenceLoopHandling = serviceJsonSettings.ReferenceLoopHandling,
						Converters = serviceJsonSettings.Converters.ToList()
					};

					s_serviceSerializerSettings[type] = settings;
				}

				foreach(MethodMetadata methodMeta in Cake.Services[type].Methods)
				{
					JsonNetSettings methodJsonSettings = ReflectionHelper.GetAttribute<JsonNetSettings>(methodMeta.Method);
					if (methodJsonSettings != null)
					{
						JsonSerializerSettings settings = new JsonSerializerSettings()
						{
							ConstructorHandling = methodJsonSettings.ConstructorHandling,
							DefaultValueHandling = methodJsonSettings.DefaultValueHandling,
							MissingMemberHandling = methodJsonSettings.MissingMemberHandling,
							NullValueHandling = methodJsonSettings.NullValueHandling,
							ObjectCreationHandling = methodJsonSettings.ObjectCreationHandling,
							PreserveReferencesHandling = methodJsonSettings.PreserveReferencesHandling,
							ReferenceLoopHandling = methodJsonSettings.ReferenceLoopHandling,
							Converters = methodJsonSettings.Converters.ToList()
						};

						s_methodSerializerSettings[methodMeta] = settings;
					}
				}

				s_isMetadataInitialized.Add(type, true);
			}
		}


		/// <summary>
		/// You will notice that every single thrown exception in this class uses this method, including exceptions caught in catch blocks (they are
		/// passed to this method, where they are wrapped in a RestException, with the original exception becoming the inner exception, and then rethrown).
		/// This method not only wraps all of the exceptions in RestExceptions, but it writes the RestException object to the response stream (json serialized),
		/// and then CLOSES the response stream.  By closing the response stream AND rethrowing, we accomplish 2 things.  1, We send the client an appropriately formatted error
		/// (remember that this is a service call.  The client is not expecting a yellow-screen-of-death (YSOD), they are expecting a json string),
		/// and 2, we let the exception bubble up to the hosting application (where it will go to Application_Error and hopefully be logged).
		/// If Application_Error doesn't happen to call Server.ClearError(), the asp.net runtime would attempt to write a YSOD to the response stream, and the response would contain
		/// the serialized RestException (json), with the YSOD tacked on to the end, and it would cause an error in the client.  That's why we close the stream, so that ONLY the
		/// json is written.  It seems hackish, but this is a very important part of RestCake!: Send back only json errors to clients AND let the hosting applications error handling
		/// execute at the same time.
		/// </summary>
		/// <param name="innerException"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		private RestException sendErrorToClientAndCloseResponse(Exception innerException, string message = null)
		{
			RestException restEx = innerException as RestException;
			RestException exceptionToThrow = restEx ?? new RestException(message ?? innerException.Message, innerException);

			// clear anything we may have written to the output stream already
			Response.Clear();
			Response.StatusCode = (int)exceptionToThrow.ResponseStatusCode;
			Response.ContentType = Constants.ContentTypeJson;

			StringBuilder sbuilder = new StringBuilder();
			TextWriter twriter = new StringWriter(sbuilder);
			twriter.Write(getStartJsonp());
			Serializer.Serialize(twriter, exceptionToThrow);
			twriter.Write(getEndJsonp());
			twriter.Close();

			// By flushing and closing the response stream, the content length cannot be properly set by the asp.net runtime, so we have to set it ourselves.
			// That's why we write the json for the exception to a StringBuilder, so we can get the length.
			// We set the content-length, and then write the StringBuidler's contents to the output stream, and then flush it and close it.
			Response.AddHeader("Content-Length", sbuilder.Length.ToString());

			// Contribution from Rich Alger. If you have an error status code, IIS is going to ignore your response stream and send back a generic error page
			// (depending on your customErrors settings), and you won't see the json representation of the RestException.
			// This line will try to prevent that IIS behavior.
			Response.TrySkipIisCustomErrors = true;

			// By flushing and closing the output stream, it makes it so that a YSOD cannot be sent under any conditions.  Normally, you'd
			// want to Server.Clear() in an error handler to prevent the YSOD, but if the developer doesn't do that, then this will still prevent it.
			Response.Write(sbuilder.ToString());
			Response.Flush();
			Response.Close();

			IsJsonErrorSent = true;
			return exceptionToThrow;
		}

		/// <summary>
		/// Overload of above method
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		private RestException sendErrorToClientAndCloseResponse(string message)
		{
			RestException restEx = new RestException(message);
			return sendErrorToClientAndCloseResponse(restEx);
		}


		/// <summary>
		/// This method is one of the core pieces of RestCake.
		/// Inputs are the UriTemplateMatch and the service method's metadata.  But what does that mean?
		/// Think about it from the start.  The client made a request with a url.  The ASP.NET routing matched that url to an IHttpHandler,
		/// which happens to be a RestCakeHandler.  So in the first place, we know what class will be handler the request.
		/// Once in the RestCakeHandler, ProcessRequest() determines which specific service method matches the UriTemplate by getting a match (UriTemplateMatch).
		/// From the match, we can get the values of the bound variables (the {} placeholders in the uri template, so something like "{id}", we get the string value
		/// for id in the url).
		/// The second arg is the MethodMetadata.  So now we know WHAT service method we need to call, and we have the argument values we need to pass that method,
		/// but we only have those values AS STRINGS, since this came in via http.
		/// What THIS method does is, based on the types of params of the service method we're calling, it tries to convert the string values to their
		/// correct target types.
		/// This method returns an object[], which should have the correct values (and correctly typed) for the target service method being called.
		/// </summary>
		private object[] getMethodArgs(UriTemplateMatch match, MethodMetadata methodMetadata)
		{
			List<RequestParameter> prms = new List<RequestParameter>();
			string postedJson = null;
			JObject postedObj = null;
			bool processedFirstJsonArg = false;

			// Loop over every parameter that our target method requires, and try to extract each one from the request.
			// They may be in the query string, uri segments, part of the request body, etc.
			foreach (ParameterInfo methodParam in methodMetadata.Parameters)
			{
				RequestParameter reqParam = new RequestParameter()
					{
						Name = methodParam.Name,
						ParameterInfo = methodParam
					};

				// See if the method param is part of the headers (this is a code contribution from "thecutter" (Gerrit) on CodePlex: http://rest.codeplex.com/Thread/View.aspx?ThreadId=234885)
				if (Request.Headers.AllKeys.Contains(methodParam.Name, StringComparer.CurrentCultureIgnoreCase))
				{
					// Param found in headers, so ASP.NET will have already put the name/value pairs in the Request.Headers bag for us.
					reqParam.StringValue = Request.Headers[methodParam.Name];
					reqParam.Source = RequestParameterSource.Headers;
				}
				// See if the method param is part of the UriTemplate (url segment or query string)
				else if (match.BoundVariables.AllKeys.Contains(methodParam.Name.ToUpper()))
				{
					// Value was part of the uri or query string: content type is irrelevant for this parameter
					reqParam.StringValue = match.BoundVariables[methodParam.Name.ToUpper()];
					reqParam.Source = match.Template.PathSegmentVariableNames.Contains(methodParam.Name.ToUpper())
						? RequestParameterSource.UriSegment : RequestParameterSource.QueryString;
				}
				else if (IsFormUrlEncoded)
				{
					// "x-www-form-urlencoded" means the data was posted as a query string, so ASP.NET will have already put the name/value pairs in the Request.Form bag for us.
					reqParam.StringValue = Request.Form[methodParam.Name];
					reqParam.Source = RequestParameterSource.RequestBodyFormUrlEncoded;
				}
				else if (IsXml)
				{
					throw sendErrorToClientAndCloseResponse(new NotSupportedException("text/xml is not currently supported by the RestCakeHandler"));
				}
				else if (IsJson)
				{
					//
					// This is where it gets tricky.  How we parse the json depends on the number of parameters, their types, and whether the request is wrapped or bare.
					//

					// TODO: Can posted args go before uri and query string args?

					// TODO: need to test how this reacts when nothing is posted.  Will postedJson be null or ""?  What happens when we try to deserialize it?
					if (postedJson == null)
						postedJson = new StreamReader(Request.InputStream).ReadToEnd();

					// Is the request bare?
					if (methodMetadata.BodyStyle == BodyStyle.Bare || methodMetadata.BodyStyle == BodyStyle.WrappedResponse)
					{
						// If we've already processed a json argument (the string data sent with the PUT, POST or DELETE request), then we need to tell
						// the user that their method needs a wrapped request.
						if (processedFirstJsonArg)
							throw sendErrorToClientAndCloseResponse("If the service method has more than a single argument, the body style must be WebMessageBodyStyle.Wrapped or WebMessageBodyStyle.WrappedRequest");
						processedFirstJsonArg = true;
						reqParam.StringValue = postedJson;
						reqParam.Source = RequestParameterSource.RequestBodyJson;
					}
					else
					{
						if (postedObj == null)
						{
							try
							{
								postedObj = JObject.Parse(postedJson);
							}
							catch (JsonSerializationException ex)
							{
								// Potentially a missing default constructor...
								string message = "There was an error deserializing the " + methodParam.Name
									+ " object.  Make sure that the " + methodParam.ParameterType.FullName +
									" class has a default parameterless constructor.";
								throw sendErrorToClientAndCloseResponse(ex, message);
							}
						}

						JProperty property = postedObj.Property(methodParam.Name);
						string valueJson;
						if (property == null || property.Value == null) // property wasn't found in the json object, or the property's value is null
							valueJson = "null";
						else
							// We only want the value part of the json.  So if the json was {"name": "Sam"}, we just want "Sam".
							valueJson = property.Value.ToString(Formatting.None);
						reqParam.StringValue = valueJson;
						reqParam.Source = RequestParameterSource.RequestBodyJson;
					}
				}
				else
				{
					string message = "RestCakeHandler does not currently support the content type \"" + RequestContentType + "\"";
					throw sendErrorToClientAndCloseResponse(new NotSupportedException(message));
				}

				prms.Add(reqParam);
			}
			foreach(RequestParameter prm in prms)
			{
				try
				{
					prm.CalcValue(Serializer);
				}
				catch (Exception ex)
				{
					// Potentially a missing default constructor...
					string message = "There was an error converting the string value \"" + prm.StringValue + "\" to its target type "
						+ prm.ParameterInfo.ParameterType.Name + " for parameter " + prm.Name + " in " + methodMetadata.Name
						+ " (param source: " + prm.Source + ")";
					throw sendErrorToClientAndCloseResponse(ex, message);
				}
			}
			object[] args = prms.Select(p => p.Value).ToArray();
			return args.ToArray();
		}


		/// <summary>
		/// In your rest service, you can override this method and it will be called just before your actual service method is called.
		/// This can be useful if you have some kind of custom security (such as checking an api key in the query string, etc) that you want to enforce.
		/// </summary>
		public virtual bool BeforeServiceMethod(MethodMetadata method, object[] args)
		{
			return true;
		}


		/// <summary>
		/// This is called from <see cref="ProcessRequest" /> once it's determined which method in the derived service class we need to call.
		/// This method uses the metadata we collected about the derived class to (via reflection) call the correct method in the service class,
		/// with the proper values for each of the arguments.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="methodMetadata"></param>
		private void callMethod(object[] args, MethodMetadata methodMetadata)
		{
			if (!BeforeServiceMethod(methodMetadata, args))
				return;

			// The service method itself might throw an exception, so we wrap the Invoke() call to the service method in a try block.
			// This way, with any thrown exception, we can send back a RestException, so that in any client API, error handling is much easier.
			// Also, in the service methods, you can simply throw an exception for any kind of validation error, etc.  Easy on both ends.
			object result;
			try
			{
				// If the service method is static, there's no instance object that we're calling the service method on.
				// If the method is non-static, we use the current IHttpHandler as the instance object, since the current handler is our service class.
				IHttpHandler handler = null;
				if (!methodMetadata.Method.IsStatic)
					handler = Context.CurrentHandler;
				result = methodMetadata.Method.Invoke(handler, args);
			}
			// Here are all the exceptions that MethodInfo.Invoke() can throw: http://msdn.microsoft.com/en-us/library/a89hcwhh.aspx
			// If certain of those are thrown, it just means there is a bug in RestCake.  Currently I'm trapping for the ones that I would expect
			// (like the user deliberately throws a RestException in a service method because validation failed)
			catch (TargetInvocationException ex)
			{
				// If an exception is thrown in the service method, the call to Invoke() will wrap that exception in a TargetInvocationException.
				// We simply want to unwrap that and throw the original exception.  It's up to the developer to handle it in the Application_Error method (Global.asax)
				Exception actualEx = ex.InnerException;
				
				// This usage is a little bit different.  We want to preserve the stack trace of the thrown exception, because it makes more sense to the hosting
				// application, so we have to be careful to rethrow the ORIGINAL exception, or the stack trace is destroyed (lost).  So we write the actual exception
				// to the client (cause they don't care about reflection), but rethrow the original reflection exception.
				sendErrorToClientAndCloseResponse(actualEx);
				// We don't throw validation exceptions, because we don't want to log them on the server side.  We just return.
				if (!(actualEx is RestValidationException))
					throw;
				return;

				// This might feel more natural:
				// throw sendErrorToClientAndCloseResponse(actualEx);
				// But it's very very bad!!  More so because it LOOKS right.  If we used the above line, we'd see the same result on the client, but the hosting app
				// would see a stack trace that ends in this call to callMethod(), and we'd have no idea what line in the actual service method caused the problem, or
				// even what kind of exception it was.  We'd essentially have no data.
			}
			catch (Exception ex)
			{
				// If an exception is thrown in the target method, that results in a TargetInvocationException (handled above).  Anything else (this catch block)
				// is probably a bug in RestCake (it doesn't handle a certain situation yet), so we'll report it as such.

				throw sendErrorToClientAndCloseResponse(ex, "There was an error in callMethod, probably due to reflection.  This is likely a bug in RestCake, please report it!");
			}

			// At this point, we have successfully called our target service method, and have the result of that call in the variable "result" (typed as object).
			// Now we just need to craft a response and send it to the client.

			// TODO: The serialization should be an extension point. All serialization should be done in a virtual method
			// that a user can override if they want. Figure out what should be passed in to that method so that overriding it can actually be useful (like passing in a bool "wrapped" etc,
			// so they don't have to figure it out for themselves)
			if (methodMetadata.Method.ReturnType != typeof(void))
			{
				if (methodMetadata.BodyStyle == BodyStyle.Wrapped || methodMetadata.BodyStyle == BodyStyle.WrappedResponse)
				{
					// Wrapped response
					StringWriter sw = new StringWriter();
					JsonTextWriter writer = new JsonTextWriter(sw);
					writer.WriteStartObject();
					writer.WritePropertyName(methodMetadata.Method.Name + "Result");

					StringWriter swriter = new StringWriter();
					Serializer.Serialize(swriter, result);
					writer.WriteRaw(swriter.ToString());

					writer.WriteEndObject();
					writeJsonResponse(sw.ToString());
				}
				else
				{
					// Bare response
					string contentType = Response.ContentType;
					if (String.IsNullOrEmpty(contentType) || contentType.ToLower().Contains("json"))
						writeJsonResponse(result);
					else
						writeBareResponse(result);
				}
			}
		}


		// This static method just calls the instance method
		private static void returnHelpPage(Match match, Type type, string everythingAfterRouteUrl, HttpContext context)
		{
			((RestCakeHandler)context.CurrentHandler).returnHelpPage();
		}


		/// <summary>
		/// This sends back the html help page when someone goes to http://site/serviceRoute/_help.
		/// "_help" is one of the default regex overrides set up in <see cref="initializeMetadata" />.
		/// </summary>
		private void returnHelpPage()
		{
			Response.ContentType = "text/html";
			string htmlTemplate = ReflectionHelper.GetTemplateContents("Js.service-help-page.html");
			StringBuilder sbMethods = new StringBuilder();

			ServiceMetadata currentService = Cake.Services[ThisType];

			// Show service methods in this service
			foreach(MethodMetadata method in currentService.Methods)
			{
				string clrArgsList = String.Join(", ", method.Parameters.Select(
					p => ReflectionHelper.GetTypeAsHtml(p.ParameterType) + " " + p.Name)
					.ToArray());

				string jsArgsList = JsClientWriter.getArgsListAsString(method) + "successCallback, errorCallback, userContext";

				Regex rxColorTemplateParams = new Regex(@"{\w+}");
				string uriTemplateHtml = rxColorTemplateParams.Replace(method.UriTemplate, "<span class='param'>$0</span>");

				sbMethods.AppendLine("<tr>")
					.Append("<td>")
					.Append(method.Name)
					.AppendLine("</td>")

					.Append("<td>")
					.Append(method.Verb.ToString("g"))
					.AppendLine("</td>")

					.Append("<td>")
					.Append(uriTemplateHtml)
					.AppendLine("</td>")

					.Append("<td>")
					.Append("(" + clrArgsList + ")")
					.AppendLine("</td>")

					.Append("<td>")
					.Append(ReflectionHelper.GetTypeAsHtml(method.Method.ReturnType))
					.AppendLine("</td>")

					.Append("<td>")
					.Append("(" + jsArgsList + ")")
					.AppendLine("</td>")

					.AppendLine("</tr>");
			}

			// Show other services in the AppDomain
			string appPath = Request.ApplicationPath;
			if (appPath == null)
				throw new Exception("Request.ApplicationPath is null.  I have no idea how that happens.  Google is your friend");
			if (!appPath.EndsWith("/"))
				appPath += "/";

			StringBuilder sbOtherServices = new StringBuilder();
			sbOtherServices.AppendLine("<strong>Other services in the same AppDomain</strong><br />");
			sbOtherServices.AppendLine("<table cellpadding='5' cellspacing='0' border='1'>");
			sbOtherServices.AppendLine("<thead><th>Url</th><th>Service Class</th><th>Service Name</th></thead><tbody>");

			// List other services
			foreach(KeyValuePair<Type, ServiceMetadata> pair in Cake.Services)
			{
				ServiceMetadata service = pair.Value;

				string routeLink = "No route! (Put a Route= value in your [RestService()] attribute on the class)";
				if (service.Route == currentService.Route)
					// don't make it a link if it's the current service
					routeLink = appPath + service.Route;
				else if (!String.IsNullOrWhiteSpace(service.Route))
					routeLink = "<a href=\"" + appPath + service.Route + "/_help\">" + appPath + service.Route + "</a>";

				sbOtherServices
					.Append("<tr>")

					.Append("<td>")
					.Append(routeLink)
					.AppendLine("</td>")

					.Append("<td>")
					.Append(service.Type.FullName)
					.Append("</td>")

					.Append("<td>")
					.Append(service.ServiceNamespace + "." + service.ServiceName)
					.Append("</td>")
					.AppendLine("</li>")

					.AppendLine("</tr>");
			}

			sbOtherServices.AppendLine("</tbody></table>");

			// put it all together
			string baseUrl = BaseUrl;
			if (!baseUrl.EndsWith("/"))
				baseUrl += "/";
			htmlTemplate = htmlTemplate
				.Replace("<#= ServiceName #>", currentService.ServiceName)
				.Replace("<#= ServiceNamespace #>", currentService.ServiceNamespace)
				.Replace("<#= ServiceBaseUrl #>", baseUrl)
				.Replace("<#= RestCakeVersion #>", VERSION)
				.Replace("<#= MethodRows #>", sbMethods.ToString())
				.Replace("<#= OtherServices #>", sbOtherServices.ToString())
				.Replace("<#= AdditionalContent #>", AdditionalHelpPageContent());

			Response.Write(htmlTemplate);
		}


		/// <summary>
		/// This can be overridden in derived classes to put additional content in the /_help page for a service,
		/// so the service can have custom content.
		/// </summary>
		/// <returns></returns>
		public virtual string AdditionalHelpPageContent()
		{
			return "";
		}


		/// <summary>
		/// This is used for an automatically setup regex override, so it matches the signature for that.
		/// This was set up in <see cref="initializeMetadata" />.
		/// Note that the CLR client (cs) is still very experimental.
		/// </summary>
		private static void returnClrClientDefinition(Match match, Type type, string everythingAfterRouteUrl, HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			//string query = context.Request.Url.Query.ToLower();
			string clientType = context.Request.QueryString[0];
			string serviceNamespace = context.Request.QueryString[1];

			StringWriter stringWriter = new StringWriter();
			ClientWriterBase clientWriter;

			if (clientType == "plain")
				clientWriter = new ClrClientWriter(stringWriter, serviceNamespace);
			else if (clientType == "restsharp")
				clientWriter = new RestSharpClientWriter(stringWriter);
			else
				throw new ArgumentException("Unsupported csharp client type provided (try \"plain\")");

			clientWriter.WriteClientHeaders();
			foreach (var pair in Cake.Services)
				clientWriter.WriteServiceClient(pair.Value);

			context.Response.Write(stringWriter.ToString());
		}


		/// <summary>
		/// This is used for an automatically setup regex override, so it matches the signature for that.
		/// This was set up in <see cref="initializeMetadata" />.
		/// </summary>
		private static void returnJsClientDefinition(Match match, Type type, string everythingAfterRouteUrl, HttpContext context)
		{
			context.Response.ContentType = "text/javascript";
			string protocol = context.Request.IsSecureConnection ? "https" : "http";
			string cacheKey = protocol + ":" + context.Request.Url.Query.ToLower();

			if (s_jsProxies[type].ContainsKey(cacheKey))
			{
				// TODO: anything with the charset?  Does it matter?  Do I need to see what came in the request?
				context.Response.Write(s_jsProxies[type][cacheKey]);
				return;
			}

			StringWriter stringWriter = new StringWriter();
			JsClientWriter clientWriter = new JsClientWriter(stringWriter);

			// See if we're supposed to write the base javascript client class, that the client classes derive from
			string rawUrl = context.Request.RawUrl.ToLower();
			if (rawUrl.Contains("base=true") || rawUrl.Contains("baseonly=true"))
				clientWriter.WriteJsClientBase();

			// See if we're supposed to write the clients or not (if they specified baseonly, then we don't write the specific client defs, just the base client class)
			if (!rawUrl.Contains("baseonly=true"))
			{
				foreach (var pair in rawUrl.Contains("allclasses=true") ? Cake.Services : Cake.Services.Where(s => s.Value.Type.FullName == type.FullName))
				{
					string baseUrl = protocol + "://" + context.Request.ServerVariables["SERVER_NAME"] + context.Request.RawUrl;
					baseUrl = baseUrl.Substring(0, baseUrl.IndexOf(everythingAfterRouteUrl, StringComparison.Ordinal));
					if (!baseUrl.EndsWith("/"))
						baseUrl += "/";

					string clientVarName = context.Request["clientVarName"];
					clientWriter.WriteServiceClient(pair.Value, baseUrl, clientVarName);
				}
			}

			// cache it
			s_jsProxies[type][cacheKey] = stringWriter.ToString();
			context.Response.Write(s_jsProxies[type][cacheKey]);
		}

		/// <summary>
		/// This is used for an automatically setup regex override, so it matches the signature for that.
		/// This was set up in <see cref="initializeMetadata" />.
		/// </summary>
		private static void returnAmdClientDefinition(Match match, Type type, string everythingAfterRouteUrl, HttpContext context)
		{
			context.Response.ContentType = "text/javascript";
			string protocol = context.Request.IsSecureConnection ? "https" : "http";
			string cacheKey = protocol + ":" + context.Request.Url.Query.ToLower() + "-amd";

			if (s_jsProxies[type].ContainsKey(cacheKey))
			{
				// TODO: anything with the charset?  Does it matter?  Do I need to see what came in the request?
				context.Response.Write(s_jsProxies[type][cacheKey]);
				return;
			}

			var stringWriter = new StringWriter();
			var clientWriter = new AmdClientWriter(stringWriter);
			string rawUrl = context.Request.RawUrl.ToLower();
			foreach (var pair in rawUrl.Contains("allclasses=true") ? Cake.Services : Cake.Services.Where(s => s.Value.Type.FullName == type.FullName))
			{
				string baseUrl = protocol + "://" + context.Request.ServerVariables["SERVER_NAME"] + context.Request.RawUrl;
				baseUrl = baseUrl.Substring(0, baseUrl.IndexOf(everythingAfterRouteUrl, StringComparison.Ordinal));
				if (!baseUrl.EndsWith("/"))
					baseUrl += "/";

				clientWriter.WriteServiceClient(pair.Value, baseUrl);
			}

			// cache it
			s_jsProxies[type][cacheKey] = stringWriter.ToString();
			context.Response.Write(s_jsProxies[type][cacheKey]);
		}


		/// <summary>
		/// This is used for an automatically setup regex override, so it matches the signature for that.
		/// This was set up in <see cref="initializeMetadata" />.
		/// </summary>
		private static void returnAmdJsClientDefinition(Match match, Type type, string everythingAfterRouteUrl, HttpContext context)
		{
			context.Response.ContentType = "text/javascript";
			string protocol = context.Request.IsSecureConnection ? "https" : "http";
			string cacheKey = protocol + ":" + context.Request.Url.Query.ToLower() + "-amdja";

			if (s_jsProxies[type].ContainsKey(cacheKey))
			{
				// TODO: anything with the charset?  Does it matter?  Do I need to see what came in the request?
				context.Response.Write(s_jsProxies[type][cacheKey]);
				return;
			}

			var stringWriter = new StringWriter();
			var clientWriter = new AmdClientWriter(stringWriter);
			string rawUrl = context.Request.RawUrl.ToLower();
			foreach (var pair in rawUrl.Contains("allclasses=true") ? Cake.Services : Cake.Services.Where(s => s.Value.Type.FullName == type.FullName))
			{
				string baseUrl = protocol + "://" + context.Request.ServerVariables["SERVER_NAME"] + context.Request.RawUrl;
				baseUrl = baseUrl.Substring(0, baseUrl.IndexOf(everythingAfterRouteUrl, StringComparison.Ordinal));
				if (!baseUrl.EndsWith("/"))
					baseUrl += "/";

				clientWriter.WriteServiceClient(pair.Value, baseUrl);
			}

			// cache it
			s_jsProxies[type][cacheKey] = stringWriter.ToString();
			context.Response.Write(s_jsProxies[type][cacheKey]);
		}


		/// <summary>
		/// This is used for an automatically setup regex override, so it matches the signature for that.
		/// This was set up in <see cref="initializeMetadata" />.
		/// </summary>
		private static void returnAmd2JsClientDefinition(Match match, Type type, string everythingAfterRouteUrl, HttpContext context)
		{
			context.Response.ContentType = "text/javascript";
			string protocol = context.Request.IsSecureConnection ? "https" : "http";
			string cacheKey = protocol + ":"
				+ context.Request.Url.Host + ":"
				+ context.Request.Url.Query.ToLower() + "-amdja";

			if (s_jsProxies[type].ContainsKey(cacheKey))
			{
				// TODO: anything with the charset?  Does it matter?  Do I need to see what came in the request?
				context.Response.Write(s_jsProxies[type][cacheKey]);
				return;
			}

			var stringWriter = new StringWriter();
			var clientWriter = new AmdClientWriter2(stringWriter);
			string rawUrl = context.Request.RawUrl.ToLower();
			foreach (var pair in rawUrl.Contains("allclasses=true") ? Cake.Services : Cake.Services.Where(s => s.Value.Type.FullName == type.FullName))
			{
				string baseUrl = protocol + "://" + context.Request.ServerVariables["SERVER_NAME"] + context.Request.RawUrl;
				baseUrl = baseUrl.Substring(0, baseUrl.IndexOf(everythingAfterRouteUrl, StringComparison.Ordinal));
				if (!baseUrl.EndsWith("/"))
					baseUrl += "/";

				clientWriter.WriteServiceClient(pair.Value, baseUrl);
			}

			// cache it
			s_jsProxies[type][cacheKey] = stringWriter.ToString();
			context.Response.Write(s_jsProxies[type][cacheKey]);
		}

		// ********************************************************************************
		// *** Public methods *************************************************************
		// ********************************************************************************

		/// <summary>
		/// Defaults to true.  If your REST service uses any state (session, etc), you should override this in derived classes,
		/// and have it return false.
		/// </summary>
		public override bool IsReusable
		{
			get { return true; }
		}

		public ServiceMetadata Metadata
		{
			get { return Cake.Services[ThisType]; }
		}


		/// <summary>
		/// This can be called from an implementing class's static constructor (or anywhere else really, but it
		/// should only be called once per type/regex combo) to register a regex override for that service class.
		/// When a request comes in, if the url of the request matches the regex, the action provided will be called,
		/// instead of the regular processing of ProcessRequest().  (This is actually how the dynamic client definition is
		/// served up)
		/// </summary>
		/// <param name="type"></param>
		/// <param name="regex"></param>
		/// <param name="action"></param>
		public static void AddRegexOverride(Type type, Regex regex, Action<Match, Type, string, HttpContext> action)
		{
			initializeMetadata(type);
			s_regexOverrides[type][regex] = action;
		}


		/// <summary>
		/// The priority order for getting the Json.NET serialization settings is:
		/// 1. A [JsonNetSettings] attribute on the service method
		/// 2. A [JsonNetSettings] attribute on the service class
		/// 3. An overridden implementation of GetSerializerSettings() in a service class
		/// 4. The default RestCake settings
		/// </summary>
		/// <returns></returns>
		private void setSerializerSettings()
		{
			// Priority 1. look for a [JsonNetSettings] attribute on the service method.
			// We don't actually do this part here.  There are some scenarios where the serializer is needed before we
			// determine the service method to be called (such as returning an error that the request url doesn't match any
			// of the service methods' UriTemplates, or a bad HTTP verb was used -- that error needs to be serialized).
			// Once we determine the service method to be called, in ProcessRequest(), we'll look for the attribute on the service method.

			// Priority 2. A [JsonNetSettings] attribute on the service class
			if (s_serviceSerializerSettings.ContainsKey(ThisType))
				m_jsonSettings = s_serviceSerializerSettings[ThisType];
			// Priority 3 & 4. An overridden implementation of GetSerializer() in a service class, or the default implementation (whichever is called via polymorphism)
			else
				m_jsonSettings = GetSerializerSettings();
		}


		/// <summary>
		/// Can be overridden in an implementing class to specify specific JsonSerializer settings to be used.
		/// Note the priority used to determine serialization settings:
		/// 1. A [JsonNetSettings] attribute on the service method
		/// 2. A [JsonNetSettings] attribute on the service class
		/// 3. An overridden implementation of GetSerializerSettings() in a service class
		/// 4. The default RestCake settings (default implementation of GetSerializerSettings())
		/// 
		/// So even if you implement this method, a [JsonNetSettings] attribute on the service class or method will override the results of this method.
		/// </summary>
		public virtual JsonSerializerSettings GetSerializerSettings()
		{
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				ConstructorHandling = ConstructorHandling.Default,
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				NullValueHandling = NullValueHandling.Include,
				MissingMemberHandling = MissingMemberHandling.Ignore,
				DefaultValueHandling = DefaultValueHandling.Include
			};
			settings.Converters.Add(new IsoDateTimeConverter());
			return settings;
		}


		/// <summary>
		/// This tries to match the Uri of the current request to one of the templates in the service class (the current IHttpHandler).
		/// If it finds a match, it will call that method, with the appropriate arguments.  If it doesn't find a match on the Uri, an error
		/// is returned to the client.
		/// </summary>
		/// <param name="context"></param>
		public override void ProcessRequest(HttpContext context)
		{
			// Get some basic info about the request into this class' member vars
			Request                 = context.Request;
			Response                = context.Response;
			Context                 = context;
			RequestContentType      = Request.ContentType.ToLower();
			RequestVerb             = Request.HttpMethod.ToUpper();
			IsFormUrlEncoded        = RequestContentType.Contains("application/x-www-form-urlencoded");
			IsXml                   = RequestContentType.Contains("text/xml");
			IsJson                  = RequestContentType.Contains("application/json");
			AbsBaseUrl              = RestCakeUtil.ResolveAbsoluteUrl(BaseUrl);

			string rawUrl           = Request.RawUrl;
			int ix                  = rawUrl.IndexOf(BaseUrl, StringComparison.Ordinal);
			EverythingAfterRouteUrl = rawUrl.Substring(ix + BaseUrl.Length);

			Host                    = context.Request.Url.Host;
			Origin                  = context.Request.Headers["Origin"];
			OriginHost              = Origin == null ? "" : (new Uri(Origin)).Host;

			// is this a CORS pre-flight request?
			IsCors                  = OriginHost != "" && Host != OriginHost;
			IsCorsPreflight         = IsCors && RequestVerb == "OPTIONS";

			// CORS support, added 2/13/2015 Sam Meacham
			if (IsCors)
			{
				string origin = ConfigurationManager.AppSettings["RestCake.Cors.Origin"];
				if (String.IsNullOrWhiteSpace(origin))
					throw new Exception("It looks like you're trying to do a CORS pre-flight request, but no RestCake.Cors.Origin setting was found in your app config file");
				Response.AddHeader("Access-Control-Allow-Origin", origin);
				Response.AddHeader("Access-Control-Allow-Credentials", "true");
			}

			if (IsCorsPreflight)
			{
				Response.AddHeader("Access-Control-Allow-Headers", "Origin, Content-Type, Accept, Authorize");
				Response.AddHeader("Access-Control-Allow-Methods", "GET, PUT, POST, DELETE, HEAD, OPTIONS");
				return;
			}


			// This can be overridden in individual service methods, but the assumption is json
			Response.ContentType = Constants.ContentTypeJson;

			// Ensure we have metadata for our service's methods.
			// We run this for each ProcessRequest(), because IIS might have discarded the old AppDomain, etc, meaning all
			// static data is reinitialized, which would cause all of the static storage dictionaries in this class to be emptied.
			initializeMetadata(ThisType);

			// Get the initial JsonSerializerSettings. This might change once we determine a specific service method to call, because that method might
			// have a [JsonNetSettings] attribute on it.
			setSerializerSettings();

			// This is the "regex backdoor" that allows us to have custom actions when a certain url is accessed.
			// If one of the registered regexes matches, the corresponding Action will be invoked, and then this method will return early,
			// and no service methods will attempt to be called.
			foreach(KeyValuePair<Regex, Action<Match, Type, string, HttpContext>> regexOverride in s_regexOverrides[ThisType])
			{
				if (regexOverride.Key.IsMatch(EverythingAfterRouteUrl))
				{
					Match m = regexOverride.Key.Match(EverythingAfterRouteUrl);
					regexOverride.Value(m, ThisType, EverythingAfterRouteUrl, context);
					return;
				}
			}

			// Make sure we support the verb of the incoming request.
			string[] supportedVerbs = Enum.GetNames(typeof (HttpVerb)).Select(v => v.ToUpper()).ToArray();
			if (!supportedVerbs.Contains(RequestVerb))
				throw sendErrorToClientAndCloseResponse("Verb \"" + RequestVerb + "\" not supported");

			Uri baseUri = new Uri(AbsBaseUrl);

			// Filter the list of the service's methods to ones that match the current request's HTTP verb.
			IList<MethodMetadata> methods = Metadata
				.Methods
				.Where(m => m.Verb.ToString("g").ToUpper() == RequestVerb)
				// Further filter the list of methods to ones where the UriTemplate is a match
				// (We should actually only match 1.  If we match multiple, that's a logic error, and we'll report that error back to the client).
				.Where(m =>
				{
					UriTemplate t = new UriTemplate(m.UriTemplate);
					UriTemplateMatch mtch = t.Match(baseUri, Request.Url);
					return mtch != null;
				})
				.ToList();

			if (methods.Count == 0)
				// The url of the request did not match any of the service methods' UriTemplates.
				throw sendErrorToClientAndCloseResponse("Bad service path '" + Request.PathInfo + "'");
			
			if (methods.Count > 1)
			{
				// Ambiguous UriTemplates. The incoming request matches more than 1 of the UriTemplates on the service methods.
				string errorMessage = "Multiple UriTemplates match the request's url.  The UriTemplates cannot be ambiguous.  The methods that matched are: ";
				foreach (MethodMetadata m in methods)
					errorMessage += m.Name + ", ";
				throw sendErrorToClientAndCloseResponse(errorMessage);
			}

			// Get the MethodMetadata for the service method we're actually going to call
			MethodMetadata method = methods.First();

			// Let's see if they pass any auth rules
			if ((method.AuthRules != null && !method.AuthRules.DoesPass(context))
				|| (method.Service.AuthRules != null && !method.Service.AuthRules.DoesPass(context)))
			{
				// send a 403 Forbidden
				// TODO: For jsonp, we'll still need to send a 200, with a status: or _status: property containing the 403
				Response.StatusCode = (int)HttpStatusCode.Forbidden;
				return;
			}

			// Set the response content type to whatever the method produces (it defaults to JSON if there's no [Produces] attribute)
			Response.ContentType = method.ContentTypeProduces;

			// Now that we know the method, we have to see if it has specific serializer settings.
			// If it does, the attribute on the method always has the highest priority.)
			if (s_methodSerializerSettings.ContainsKey(method))
				m_jsonSettings = s_methodSerializerSettings[method];

			// Get the args that we'll pass to our target method from the incoming request
			UriTemplate template = new UriTemplate(method.UriTemplate);
			UriTemplateMatch match = template.Match(baseUri, Request.Url);
			// TODO: Would be nice to have a virtual method that any service class can override that is an extension point.
			// TODO: The method would be called before getMethodArgs(), and would be a chance for the service class to do any custom processing.
			// TODO: Might be cool to find a way to skip processing of any args that the user already processed. A bool[] to indicate which were process maybe. Dunno.
			object[] args = getMethodArgs(match, method);

			// Call the target method
			callMethod(args, method);
		}



		// ********************************************************************************
		// *** Public static methods ******************************************************
		// *** These methods don't neccessarily have anything to do with the specific processing in the RestCakeHandler,
		// *** but this class servers as the best entry point to access these static utility methods, API-wise.
		// ********************************************************************************

		public static bool FormsAuthOrRedirectMessage(HttpContext context, bool checkUrlAccessForPrincipal = true)
		{
			// If something else in the pipeline has already indicated that we should ignore auth altogether, then respect it.
			// See the SkipAuthorizationRulesModule for examples.
			if (context.SkipAuthorization)
				return true;

			// CORS stuff (preflight requests don't send cookies!!)
			string verb   = context.Request.HttpMethod.ToUpper();
			string host   = context.Request.Url.Host;
			string origin = context.Request.Headers["Origin"];
			string originHost = origin == null ? "" : (new Uri(origin)).Host;

			// is this a CORS pre-flight request?
			if (originHost != "" && host != originHost && verb == "OPTIONS")
				return true;

			if (checkUrlAccessForPrincipal)
			{
				// Make sure the url they're trying to access is actually protected under forms auth.
				// The context.User might be null if the user is not logged in, hence the coalesce with a generic principal.
				IPrincipal genericPrincipal = new GenericPrincipal(new GenericIdentity(""), null);
				if (UrlAuthorizationModule.CheckUrlAccessForPrincipal(context.Request.Path, context.User ?? genericPrincipal, context.Request.HttpMethod))
					// The url is accessible without forms auth, so return true
					return true;
			}

			// The url requires forms auth, so let's see if they have a valid cookie
			FormsAuthenticationTicket ticket = GetFormsAuthTicket(context);
			if (ticket == null || ticket.Expired)
			{
				// They are not logged in via forms auth
				var redirectMessage = new
				                      	{
				                      		_redirect = true,
				                      		_url = FormsAuthentication.LoginUrl
				                      	};
				// Create an HTTP 407 response.
				// Using things like jQuery.ajax(), this will cause the error callback to fire, instead of the success callback,
				// and the error number of very specific so it's easy to trap for.
				context.Response.StatusCode = (int)HttpStatusCode.ProxyAuthenticationRequired;
				context.Response.ContentType = Constants.ContentTypeJson;
				string json = JsonConvert.SerializeObject(redirectMessage);
				context.Response.Write(json);
				context.Response.End();
				return false;
			}
			return true;
		}

		/// <summary>
		/// This will return the decrypted Forms Authentication ticket, or null if it doesn't exist, is invalid, or has expired.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static FormsAuthenticationTicket GetFormsAuthTicket(HttpContext context)
		{
			HttpCookie formsAuthCookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
			FormsAuthenticationTicket ticket = null;
			if (formsAuthCookie != null)
			{
				try
				{
					ticket = FormsAuthentication.Decrypt(formsAuthCookie.Value);
				}
				catch (Exception)
				{
					ticket = null;
				}
			}
			return ticket;
		}
	}
}
