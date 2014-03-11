using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using minirack;
using RestCake.Metadata;
using RestCake.Routing;
using RestCake.Util;

namespace RestCake
{
	/// <summary>
	/// minirack attributes will dynamically register modules and call startup code
	/// </summary>
	[Pipeline]
	[PostAppStart]
	internal class RestCakeModule : IHttpModule
	{
		public void Dispose()
		{ }

		public void Init(HttpApplication context)
		{
			context.AuthenticateRequest += context_AuthenticateRequest;
		}

		public static void PostAppStart()
		{
			populateServiceTypes();
			setupRoutes();
		}

		private static void populateServiceTypes()
		{
			// Get all classes that have a [RestService] attribute.
			Type[] serviceTypes = PipelineInstaller.GetUserTypes(t => t.HasAttribute<RestServiceAttribute>());
			foreach (Type serviceType in serviceTypes)
			{
				if (!typeof (RestCakeHandler).IsAssignableFrom(serviceType))
					throw new Exception("A [RestService] class must subclass RestCake.RestCakeHandler");
				// TODO: I don't know why it's getting registered multiple times...
				if (Cake.Services.ContainsKey(serviceType))
					continue;
				if (!Cake.Services.TryAdd(serviceType, new ServiceMetadata(serviceType)))
				{
					if (Cake.Services.ContainsKey(serviceType))
						throw new Exception("The service type has already been added.");
					throw new Exception("Could not add service " + serviceType.FullName);
				}
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

			// Detect expired forms auth (only apply to RestCakeHandler requests)
			// NOTE: It would be nice to simply use app.Context.Handler, to see if it's a RestCakeHandler,
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
					RestCakeHandler.FormsAuthOrRedirectMessage(app.Context);
					break;
				}
			}
		}

	}
}
