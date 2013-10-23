using System;

namespace RestCake.AuthRules
{
	[Serializable]
	internal class SkipAuthorizationRulesConfigSection
	{
		public string RequestUrlRegex { get; set; }

		public string HttpAuthHeaderRegex { get; set; }
		public string HttpAuthHeaderValue { get; set; }

		public string ClientIpRegex { get; set; }
		public string ClientIp { get; set; }
	}
}
