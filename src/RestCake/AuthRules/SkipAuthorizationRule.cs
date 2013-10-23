using System.Text.RegularExpressions;
using System.Web;

namespace RestCake.AuthRules
{
	/// <summary>
	/// Rules are for 1 of 3 things: the request url (the AbsoluteUri), the HTTP AUTH header value, or the client's IP.
	/// If a rule has multiple parts (e.g. url and clientIP), then ALL parts must be satisfied for the rule to match.
	/// Instances of this class will be created by the <see cref="SkipAuthorizationRulesModule" /> in Init(), when it
	/// processes any rules specified in the web.config.
	/// </summary>
	public class SkipAuthorizationRule
	{
		public Regex AbsUrlRegex { get; private set; }
		public Regex RelUrlRegex { get; private set; }
		public string HttpAuthHeaderValue { get; private set; }
		public string ClientIp { get; private set; }

		public SkipAuthorizationRule(Regex absUrlRegex, Regex relUrlRegex, string httpAuthHeaderValue, string clientIp)
		{
			AbsUrlRegex = absUrlRegex;
			RelUrlRegex = relUrlRegex;
			HttpAuthHeaderValue = httpAuthHeaderValue;
			ClientIp = clientIp;
		}

		public SkipAuthorizationRule(string absUrlRegex, string relUrlRegex, string httpAuthHeaderValue, string clientIp)
		{
			AbsUrlRegex = absUrlRegex == null ? null : new Regex(absUrlRegex, RegexOptions.IgnoreCase);
			RelUrlRegex = relUrlRegex == null ? null : new Regex(relUrlRegex, RegexOptions.IgnoreCase);
			HttpAuthHeaderValue = httpAuthHeaderValue;
			ClientIp = clientIp;
		}


		/// <summary>
		/// Checks to see if an HttpRequest is a match for any one of the values or regexes provided for this rule.
		/// Urls are checked again Request.Url.AbsoluteUri
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public bool IsMatch(HttpRequest request)
		{
			if (AbsUrlRegex != null && !AbsUrlRegex.IsMatch(request.Url.AbsoluteUri))
				return false;

			string relUrl = request.RawUrl.Substring(request.ApplicationPath.Length);

			if (RelUrlRegex != null && (!RelUrlRegex.IsMatch(relUrl) && !RelUrlRegex.IsMatch(request.RawUrl)))
				return false;

			if (HttpAuthHeaderValue != null && HttpAuthHeaderValue != request.Headers["Authorization"])
				return false;

			if (ClientIp != null && ClientIp != request.ServerVariables["REMOTE_ADDR"])
				return false;

			return true;
		}

	}
}
