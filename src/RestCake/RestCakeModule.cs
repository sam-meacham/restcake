using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Linq;
using System.Web.Routing;
using RestCake.Metadata;
using RestCake.Routing;
using RestCake.Util;


namespace RestCake
{
	public class RestCakeModule : IHttpModule
	{
		public void Dispose()
		{ }


		public void Init(HttpApplication context)
		{
			context.AuthenticateRequest += context_AuthenticateRequest;

			if (!Cake.IsInitialized)
			{
				lock(Cake.s_objSync)
				{
					if (!Cake.IsInitialized)
					{
						populateServiceTypes();
						setupRoutes();
						Cake.IsInitialized = true;
					}
				}
			}
		}

		private static void populateServiceTypes()
		{
			// Get every assembly in the current AppDomain that isn't an obvious MS assembly.
			List<Assembly> assemblies = AppDomain.CurrentDomain
				.GetAssemblies()
				.Where(asm => !asm.FullName.StartsWith("mscorlib") && !asm.FullName.StartsWith("System.") && !asm.FullName.StartsWith("Microsoft"))
				.ToList();
			// In each of those assemblies, find all classes that have a [RestService] attribute.
			foreach (Assembly asm in assemblies)
			{
				IEnumerable<Type> serviceTypes = ReflectionHelper.GetTypesWithAttribute(asm, typeof(RestServiceAttribute));
				foreach (Type serviceType in serviceTypes)
					Cake.Services.Add(serviceType, new ServiceMetadata(serviceType));
			}
		}

		private static void setupRoutes()
		{
			foreach (KeyValuePair<Type, ServiceMetadata> service in Cake.Services)
			{
				if (String.IsNullOrWhiteSpace(service.Value.Route))
					continue;
				object route = ReflectionHelper.CreateGeneric(typeof (GenericHandlerRoute<>), service.Key, new object[] {service.Value.Route});
				// These are all inserted at the top to avoid conflicts with MVC routes, which seem to eat up some of the regex overrides and cause 404s
				RouteTable.Routes.Insert(0, (RouteBase) route);
			}
		}

		private static void context_AuthenticateRequest(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;

			// Detect expired forms auth (only apply to RestHttpHandler requests)
			// NOTE: It would be nice to simply use app.Context.Handler, to see if it's a RestHttpHandler,
			// but it hasn't been determined yet.

			// this gets the "~/..." style path to the handler being requested
			string requestRelPath = app.Request.AppRelativeCurrentExecutionFilePath;
			if (requestRelPath == null)
				return;
			requestRelPath = requestRelPath.ToLower();

			foreach (KeyValuePair<Type, ServiceMetadata> service in Cake.Services)
			{
				string serviceRoute = "~/" + service.Value.Route.ToLower();
				if (requestRelPath.StartsWith(serviceRoute))
				{
					RestHttpHandler.FormsAuthOrRedirectMessage(app.Context);
					break;
				}
			}
		}

	}
}
