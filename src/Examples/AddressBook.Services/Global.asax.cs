using System;
using System.IO;
using System.ServiceModel.Activation;
using System.Text;
using System.Web.Routing;
using RestCake.AddressBook.DataAccess;
using RestCake.Routing;


namespace RestCake.AddressBook.Services
{
	public class Global : System.Web.HttpApplication
	{
		private static void registerRoutes()
		{
			// WCF and CakeRest routes for the WcfAndRestCakeDualService service
			RouteTable.Routes.Add(new GenericHandlerRoute<WcfAndRestCakeDualService>("dual_cake"));
			RouteTable.Routes.Add(new ServiceRoute("dual_wcf", new WebServiceHostFactory(), typeof(WcfAndRestCakeDualService)));

			// WCF and CakeRest routes for the WebMessageBodyStyleComparisonService service
			RouteTable.Routes.Add(new GenericHandlerRoute<WebMessageBodyStyleComparisonService>("bodyStyle_cake"));
			RouteTable.Routes.Add(new ServiceRoute("bodyStyle_wcf", new WebServiceHostFactory(), typeof(WebMessageBodyStyleComparisonService)));

			// AddressBook service
			RouteTable.Routes.Add(new GenericHandlerRoute<AddressBookService>("contacts"));

			// Math service (used for the error handling examples)
			RouteTable.Routes.Add(new GenericHandlerRoute<MathService>("math"));

			// Unit test service
			RouteTable.Routes.Add(new GenericHandlerRoute<UnitTestService>("unittest"));
		}


		protected void Application_Start(object sender, EventArgs e)
		{
			AutoMapperConfig.CreateMappings();
			registerRoutes();
		}


		protected void Application_Error(object sender, EventArgs e)
		{
			Exception ex = Server.GetLastError();

			// Poor man's error logging...
			Guid guid = Guid.NewGuid();
			string path = Path.Combine(Server.MapPath("~/"), "error-" + guid + ".txt");
			FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
			StreamWriter writer = new StreamWriter(stream);
			writer.WriteLine(DateTime.Now.ToString());
			writer.WriteLine(getExceptionInfo(ex));
			writer.WriteLine(new string('-', 80));
			// Closes the underlying stream as well
			writer.Close();

			// Prevent a YSOD.  RestCake already handles sending down a proper json-serialized error.
			// (RestCake also flushes and closes the response stream, blocking a YSOD anyway, but it's still good to clear it here, or we'll
			// get an error in the windows logs, that there wan an unhandled exception in asp.net)
			if (RestHttpHandler.IsJsonErrorSent)
				Server.ClearError();
		}

		private static string getExceptionInfo(Exception ex)
		{
			Exception curEx = ex;
			StringBuilder sb = new StringBuilder();

			bool isInner = false;
			while (curEx != null)
			{
				if (isInner)
				{
					sb.AppendLine();
					sb.AppendLine("(Inner Exception)");
				}
				sb.AppendLine("Type: " + curEx.GetType());
				sb.AppendLine("Message: " + curEx.Message);
				sb.AppendLine("Stack Trace: " + curEx.StackTrace);
				sb.AppendLine("Source: " + curEx.Source);
				sb.AppendLine("TargetSite: " + curEx.TargetSite);
				curEx = curEx.InnerException;
				isInner = true;
			}
			return sb.ToString();
		}
	}
}