using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using RestCake.Metadata;
using RestCake.Util;


namespace RestCake.Clients
{
	public class ClrClientWriter : ClientWriterBase
	{
		public string ServiceNamespace { get; private set; }

		public ClrClientWriter(TextWriter textWriter, string serviceNamespace)
			: base(textWriter)
		{
			ServiceNamespace = serviceNamespace;
		}


		protected override string GetReturnType(MethodMetadata method)
		{
			return ReflectionHelper.GetFriendlyTypeName(method.Method.ReturnType);
		}



		/// <summary>
		/// Returns a string like "arg1, arg2, arg3", etc (joins all params with a comma)
		/// </summary>
		/// <returns></returns>
		internal override string GetArgsListAsString(MethodMetadata method)
		{
			// For args that end with "Json", we want to strip that off the arg name.
			// WCF doesn't seem to let us pass raw json in as a param.  They want to use their own deserialization, which fails for many things.
			// We are using Json.NET, and so we pass in doubly quoted json strings, so the arg going into the service method is not an object, but a string.
			// But, we want our params named sanely.
			string argsList = String.Join(", ", method.Parameters.Select(
				p => ReflectionHelper.GetFriendlyTypeName(p.ParameterType) + " " + p.Name).ToArray());

			if (method.Parameters.Length > 0)
				argsList += ", ";

			return argsList;
		}

		private static string getParamAsUrlValue(ParameterInfo param)
		{
			Type t = param.ParameterType;
			if (t.IsPrimitive || t.FullName == "System.String")
				return param.Name;
			return "JsonConvert.SerializeObject(" + param.Name + ")";
		}

		protected override string GetMethodUrl(MethodMetadata method)
		{
			// Remember that in the template (ServiceMethod.txt), the <#= Url #> placeholder is NOT wrapped in quotes,
			// so we'll have to take care of that in this method.
			string methodUrl = method.UriTemplate;
			ParameterInfo[] urlParams = GetUrlParams(method);

			// These will be set to false if we discover the opposite to be true in the next 2 for loops (going through query string and uri segment params)
			bool endsInString = true;
			bool startsInString = true;

			// Query string params
			foreach (ParameterInfo param in urlParams)
			{
				// search example: name={name} (two curly braces is a literal curly brace in a format string)
				string search = String.Format("{0}={{{0}}}", param.Name);
				string replace = String.Format("{0}=\" + {1} + \"", param.Name, getParamAsUrlValue(param));

				if (methodUrl.StartsWith(search))
					startsInString = false;

				methodUrl = methodUrl.Replace(search, replace);
			}

			// UriSegment params
			for (int i = 0; i < urlParams.Length; i++)
			{
				ParameterInfo param = urlParams[i];
				string search = "{" + param.Name + "}";

				//if (methodUrl.StartsWith(search))
					//startsInString = false;
				//if (methodUrl.EndsWith(search))
					//endsInString = false;

				methodUrl = methodUrl.Replace("{" + param.Name + "}", "\" + " + getParamAsUrlValue(param) + " + \"");
			}

			if (startsInString)
				methodUrl = "\"" + methodUrl;
			if (endsInString)
				methodUrl += "\"";

			// Get rid of useless empty string concats
			methodUrl = methodUrl
				.Replace(" + \"\"", "")
				.Replace("\"\" + ", "");

			return methodUrl;
		}


		public override void WriteClientHeaders()
		{
			string template = ReflectionHelper.GetTemplateContents("ClrPlain.ClrClientBase.cs")
				.Replace("<#= Namespace #>", "" + ServiceNamespace);
			TextWriter.WriteLine(template);
		}


		public override void WriteServiceClient(ServiceMetadata service)
		{
			// Get the service proxy template, and setup the namespace, and the name of the js class
			string clientTemplate = ReflectionHelper.GetTemplateContents("ClrPlain.ClrClient.txt");
			clientTemplate = clientTemplate
				.Replace("<#= Namespace #>", ServiceNamespace)
				.Replace("<#= ClientClassName #>", service.ServiceName + "Client");

			// Go through each method in the service class, creating the client method for each one.
			StringBuilder sbMethodMetadatas = new StringBuilder();
			foreach (MethodMetadata method in service.Methods)
				sbMethodMetadatas.AppendLine(GetMethodBody(method));

			// Add the service methods that we just created to the client tempalte
			clientTemplate = clientTemplate.Replace("<#= ClientServiceMethods #>", sbMethodMetadatas.ToString());

			// Write out the proxy template
			TextWriter.WriteLine("// ClrClient.txt template for " + service.ServiceNamespace + "." + service.ServiceName);
			TextWriter.WriteLine(clientTemplate);
		}


		protected override string GetMethodBody(MethodMetadata method)
		{
			string MethodMetadataTemplate;
			if (GetReturnType(method) == "void")
				MethodMetadataTemplate= ReflectionHelper.GetTemplateContents("ClrPlain.ServiceMethodVoid.txt");
			else
				MethodMetadataTemplate = ReflectionHelper.GetTemplateContents("ClrPlain.ServiceMethod.txt");

			string methodBody = MethodMetadataTemplate
				.Replace("<#= ReturnType #>", GetReturnType(method))
				.Replace("<#= MethodName #>", method.Name)
				.Replace("<#= MethodArgs #>", GetArgsListAsString(method))
				.Replace("<#= Verb #>", method.Verb.ToString("g").ToUpper())
				.Replace("<#= Url #>", GetMethodUrl(method))
				.Replace("<#= Body #>", getRequestBody(method));

			return methodBody;
		}


		public string getRequestBody(MethodMetadata method)
		{
			string[] dataParams = GetDataParamNames(method);
			if (method.Verb == HttpVerb.Get || dataParams.Length == 0)
				return "null";

			string dataParamsDecl = "new [] {" + String.Join(",", dataParams.Select(p => "\"" + p + "\"")) + "}";
			string valuesDecl = "new object[] {" + String.Join(",", dataParams) + "}";
			return "ConstructRequestBody(" + method.IsWrappedRequest.ToString().ToLower() + ", " + dataParamsDecl + ", " + valuesDecl + ")";
		}

	}
}
