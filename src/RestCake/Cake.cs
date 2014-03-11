using System;
using System.Collections.Concurrent;
using System.Text;
using System.Web;
using RestCake.Metadata;

namespace RestCake
{
	public static class Cake
	{
		/// <summary>
		/// Every RestCakeHandler service in the current AppDomain. This is populated from <see cref="RestCakeModule.Init" />.
		/// </summary>
		public static readonly ConcurrentDictionary<Type, ServiceMetadata> Services = new ConcurrentDictionary<Type, ServiceMetadata>();

		/// <summary>
		/// This can optionally be set from somewhere like Application_Start(). If this has a value, it will be appended to any js includes
		/// with a "&amp;v={value}", to prevent browsers from caching the js clients. This is used from <see cref="JqueryClientScript{T}" />.
		/// </summary>
		public static string ScriptVersionString { get; set; }

		public static string JqueryClientScript<T>(bool includeBase = false, string clientVarName = null) where T : RestCakeHandler
		{
			string appPath = HttpContext.Current.Request.ApplicationPath;
			if (appPath == null)
				throw new Exception("ApplicationPath was null. Not sure how that can happen...");
			if (!appPath.EndsWith("/"))
				appPath += "/";

			ServiceMetadata service = Services[typeof (T)];

			StringBuilder sb = new StringBuilder();
			sb.Append("<script type=\"text/javascript\" src=\"");
			sb.Append(appPath);
			sb.Append(service.Route + "/_js?type=jquery&base=" + includeBase.ToString().ToLower());
			// see if they provided a js "clientVarName"
			if (!String.IsNullOrWhiteSpace(clientVarName))
				sb.Append("&clientVarName=" + clientVarName);
			// see if the service has a default clientVarName
			else if (!String.IsNullOrWhiteSpace(service.JsClientVarName))
				sb.Append("&clientVarName=" + service.JsClientVarName);

			// append a v= version string to the src if they provided one in ScriptVersionString
			if (!String.IsNullOrWhiteSpace(ScriptVersionString))
				sb.Append("&v=" + ScriptVersionString);

			sb.Append("\"></script>");
			return sb.ToString();
		}
	}
}
