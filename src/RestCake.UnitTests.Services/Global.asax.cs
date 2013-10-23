using System;
using System.Web.Routing;
using RestCake.Routing;

namespace RestCake.UnitTests.Services
{
	public class Global : System.Web.HttpApplication
	{
		private static void registerRoutes()
		{
			RouteTable.Routes.Add(new GenericHandlerRoute<NullValueHandlingTest1>("nullValueHandlingTest1"));
			RouteTable.Routes.Add(new GenericHandlerRoute<InputParamsService>("inputs"));
		}

		protected void Application_Start(object sender, EventArgs e)
		{
			registerRoutes();
		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{

		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}