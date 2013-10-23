using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RestCake.Metadata;
using RestCake.Util;


namespace RestCake.Clients
{
	public class RestSharpClientWriter : ClientWriterBase
	{
		public RestSharpClientWriter(TextWriter textWriter)
			: base(textWriter)
		{
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
			return argsList;
		}

		protected override string GetMethodUrl(MethodMetadata method)
		{
			string methodUrl = method.UriTemplate;
			string[] urlParams = GetUrlParamNames(method);


			//
			// Query string style
			//
			foreach (string param in urlParams)
			{
				// search example: name={name} (two curly braces is a literal curly brace in a format string)
				string search = String.Format("{0}={{{0}}}&", param);
				string replace = String.Format("{0}=\" + {0} + \"&\" + \"", param);
				methodUrl = methodUrl.Replace(search, replace);
			}
			// The last entry won't have a trailing '&'
			if (urlParams.Length > 0)
			{
				string param = urlParams[urlParams.Length - 1];
				string search = String.Format("{0}={{{0}}}", param);
				string replace = String.Format("{0}=\" + {0} + \"", param);
				methodUrl = methodUrl.Replace(search, replace);
			}

			// Get rid of useless empty string concats
			methodUrl = methodUrl.Replace(" + \"\"", "");

			// Clean url style
			foreach (string param in urlParams)
				methodUrl = methodUrl.Replace("{" + param + "}", "\" + " + param + " + \"");

			// Get rid of weird [ + ''] and ['' + ] instances at the end or beginning (respecitvely) of the string
			Regex rxBeg = new Regex(@"^"""" *");
			Regex rxEnd = new Regex(@" *""""$");
			methodUrl = rxBeg.Replace(methodUrl, "");
			methodUrl = rxEnd.Replace(methodUrl, "");
			return methodUrl;
		}


		public override void WriteClientHeaders()
		{
			TextWriter.WriteLine(ReflectionHelper.GetTemplateContents("RestSharp.RestSharpClientBase.txt"));
		}


		public override void WriteServiceClient(ServiceMetadata service)
		{
			// Get the service proxy template, and setup the namespace, and the name of the js class
			string clientTemplate = ReflectionHelper.GetTemplateContents("RestSharp.RestClient.txt");
			clientTemplate = clientTemplate
				.Replace("<#= Namespace #>", service.ServiceNamespace)
				.Replace("<#= ClientClassName #>", service.ServiceName + "Client");

			// Go through each method in the service class, creating the client method for each one.
			StringBuilder sbMethodMetadatas = new StringBuilder();
			foreach (MethodMetadata method in service.Methods)
				sbMethodMetadatas.AppendLine(GetMethodBody(method));

			// Add the service methods that we just created to the client tempalte
			clientTemplate = clientTemplate.Replace("<#= ClientServiceMethods #>", sbMethodMetadatas.ToString());

			// Write out the proxy template
			TextWriter.WriteLine("// RestClient.txt template for " + service.ServiceNamespace + "." + service.ServiceName);
			TextWriter.WriteLine(clientTemplate);
		}


		protected override string GetMethodBody(MethodMetadata method)
		{
			string MethodMetadataTemplate;
			if (GetReturnType(method) == "void")
				MethodMetadataTemplate= ReflectionHelper.GetTemplateContents("RestSharp.ServiceMethodVoid.txt");
			else
				MethodMetadataTemplate = ReflectionHelper.GetTemplateContents("RestSharp.ServiceMethod.txt");

			StringBuilder sbParams = new StringBuilder();
			sbParams.AppendLine("new Parameter[] {")
				.Append("\t\t\t\t").AppendLine(String.Join(", ", GetDataParamNames(method).Select(p => "new Parameter() { Name = \"" + p + "\", Type = ParameterType.GetOrPost, Value = " + p + " }").ToArray()))
				.Append("\t\t\t}");

			string methodBody = MethodMetadataTemplate
				.Replace("<#= ReturnType #>", GetReturnType(method))
				.Replace("<#= MethodName #>", method.Name)
				.Replace("<#= MethodArgs #>", GetArgsListAsString(method))
				.Replace("<#= MethodUrl #>", GetMethodUrl(method))
				.Replace("<#= HttpVerb #>", method.Verb.ToString("g").ToUpper())
				.Replace("<#= Parameters #>", sbParams.ToString())
				.Replace("<#= IsWrappedRequest #>", method.IsWrappedResponse.ToString().ToLower())
				.Replace("<#= IsWrappedResponse #>", method.IsWrappedResponse.ToString().ToLower());

			return methodBody;
		}

	}
}
