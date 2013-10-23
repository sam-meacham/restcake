using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace RestCake.AuthRules
{
	public class SkipAuthorizationRulesConfigSectionHandler : IConfigurationSectionHandler
	{
		public object Create(object parent, object configContext, XmlNode section)
		{
			// This is the list of rules that we'll return
			List<SkipAuthorizationRule> rules = new List<SkipAuthorizationRule>();

			// If there's no rules, return our empty list
			if (section == null)
				return rules;
			
			// Let's use Linq to Xml instead of the old school Xml stuff.
			XElement xsection = XElement.Parse(section.OuterXml);

			rules.AddRange(
				xsection
					.Elements("Rule")
					.Select(rule => {
						XAttribute absUrlRegex = rule.Attribute("AbsUrlRegex");
						XAttribute relUrlRegex = rule.Attribute("RelUrlRegex");
						XAttribute httpAuthHeaderValue = rule.Attribute("httpAuthHeaderValue");
						XAttribute clientIp = rule.Attribute("ClientIp");

						return new SkipAuthorizationRule(
							absUrlRegex == null ? null : absUrlRegex.Value,
							relUrlRegex == null ? null : relUrlRegex.Value,
							httpAuthHeaderValue == null ? null : httpAuthHeaderValue.Value,
							clientIp == null ? null : clientIp.Value
						);
					})
			);

			return rules;
		}

	}
}
