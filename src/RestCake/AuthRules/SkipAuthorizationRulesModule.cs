using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RestCake.AuthRules
{
	public class SkipAuthorizationRulesModule : IHttpModule
	{
		private List<SkipAuthorizationRule> m_rules;


		public void Dispose()
		{ }

		public void Init(HttpApplication context)
		{
			// Get any rules specified in the web.config
			m_rules = (List<SkipAuthorizationRule>) ConfigurationManager.GetSection("SkipAuthorizationRules") ?? new List<SkipAuthorizationRule>();

			// We need to intercept before authorization
			context.AuthenticateRequest += OnAuthenticateRequest;
		}


		public void OnAuthenticateRequest(Object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication) sender;

			if (m_rules.Any(rule => rule != null && rule.IsMatch(app.Request)))
				app.Context.SkipAuthorization = true;
		}

	}
}
