using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;


namespace RestCake
{
	/// <summary>
	/// A whitelist of rules to protect a service or service method. If a service or method has this attribute, one of the
	/// provided rules must pass or the caller will get an HTTP 403 Forbidden.  Note that only 1 rule has to pass.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class AuthorizeAttribute : Attribute
	{
		public string[] Roles { get; set; }
		public string[] Users { get; set; }
		public string[] AuthHeaderValues { get; set; }
		public string[] IpAddresses { get; set; }
		public string Method { get; set; }
		public AuthRuleStyle Style { get; set; }

		public AuthorizeAttribute()
		{
			Style = AuthRuleStyle.Any;
		}

		public bool DoesPass(HttpContext context)
		{
			List<bool> results = new List<bool>();

			if (Roles != null)
			{
				bool pass = Roles.Any(role => context.User.IsInRole(role));
				results.Add(pass);
			}

			if (Users != null)
			{
				string username = null;
				if (context.User != null)
					username = context.User.Identity.Name.ToLowerInvariant();
				bool pass = username != null && Users.Any(user => user.ToLowerInvariant() == username);
				results.Add(pass);
			}

			if (AuthHeaderValues != null)
			{
				
				string authHeader = context.Request.Headers["Authorization"];
				bool pass = !String.IsNullOrEmpty(authHeader) && AuthHeaderValues.Contains(authHeader);
				results.Add(pass);
			}

			if (IpAddresses != null)
			{
				bool pass = IpAddresses.Contains(context.Request.ServerVariables["REMOTE_ADDR"]);
				results.Add(pass);
			}

			if (Method != null)
			{
				Type serviceClass = context.CurrentHandler.GetType();
				MethodInfo authMethod = serviceClass.GetMethods().FirstOrDefault(m => m.Name == Method);
				if (authMethod == null)
					throw new Exception("You specified in an [Authorize] attribute to call a method: " + Method + ".  No such method exists.");
				bool pass;
				if (authMethod.IsStatic)
					pass = (bool)authMethod.Invoke(null, null);
				else
					pass = (bool) authMethod.Invoke(context.CurrentHandler, null);
				results.Add(pass);
			}

			if (Style == AuthRuleStyle.Any)
				return results.Any(r => r);
			return results.All(r => r);
		}

	}
}
