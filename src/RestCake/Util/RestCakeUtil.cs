using System;
using System.Web;


namespace RestCake.Util
{
	internal static class RestCakeUtil
	{
		public static T Cast<T>(object obj)
		{
			return (T)obj;
		}

		/// <summary>
		/// This resolves the ~ in urls.  The resulting url is absolute to the root of the server, but
		/// does not include the server protocol or hostname.
		/// Ex: ~/my/path becomes /myApp/my/path
		/// </summary>
		public static string ResolveUrl(string tildedUrl)
		{
			if (tildedUrl == null)
				return null;

			if (tildedUrl.StartsWith("~"))
				return VirtualPathUtility.ToAbsolute(tildedUrl);

			return tildedUrl;
		}


		/// <summary>
		/// This resolves the ~ in urls to a completely absolute url, including the protocol and hostname of the server.
		/// Ex: ~/my/path becomes http://www.mydomain.com/myaApp/my/path
		/// </summary>
		public static string ResolveAbsoluteUrl(string tildedUrl, bool forceHttps = false)
		{
			// If the url is already absolute, we have nothing to do
			if (tildedUrl.IndexOf("://", StringComparison.Ordinal) > -1)
				return tildedUrl;

			string absUrl = ResolveUrl(tildedUrl);

			Uri originalUri = HttpContext.Current.Request.Url;
			absUrl = (forceHttps ? "https" : originalUri.Scheme) + "://" + originalUri.Authority + absUrl;
			return absUrl;
		}
	}
}
