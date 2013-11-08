using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using RestCake.Metadata;
using RestCake.Routing;
using RestCake.Util;

// Causes RestCakeModule.PreAppInit() to be called before the http app is started up
// This means we have a chance to dynamically add assembly refs, http modules, etc.
[assembly: PreApplicationStartMethod(typeof(RestCake.RestCakeModule), "PreAppInit")]

namespace RestCake
{
	public class RestCakeModule : IHttpModule
	{
		private static string s_logfile = "_restcake-log.txt";

		// as indicated by the above [assembly] attribute
		public static void PreAppInit()
		{
			string appdir = AppDomain.CurrentDomain.BaseDirectory;
			string bindir = Path.Combine(appdir, "bin");
			s_logfile = Path.Combine(bindir, s_logfile);

			log("RestCake app startup log (PreAppInit): " + DateTime.Now);
			log("This is where RestCake dynamically adds the RestCakeModule to the web app, and finds any [RestService] classes to set up services and routing.");
			// dynamically register the RestCakeModule - no web.config setting!
			try { DynamicModuleUtility.RegisterModule(typeof (RestCakeModule)); }
			catch { log("Error adding the RestCakeModule"); }

			populateServiceTypes(bindir);
			setupRoutes();
		}

		public void Dispose()
		{ }

		public void Init(HttpApplication context)
		{
			context.AuthenticateRequest += context_AuthenticateRequest;
		}

		private static void log(string msg)
		{
			File.AppendAllText(s_logfile, msg + "\r\n");
		}

		private static void populateServiceTypes(string bindir)
		{
			// Find all classes that have a [RestService] attribute.
			string[] dlls = Directory.GetFiles(bindir, "*.dll");
			log("Dlls found in " + bindir);
			log("\t" + String.Join("\r\n\t", dlls));

			foreach (string dll in dlls)
			{
				// get the plain filename (no path info) of the assembly
				string asmFilename = Path.GetFileName(dll);
				if (asmFilename == null)
					throw new Exception("Couldn't get filename of dll " + dll);

				// skip system and microsoft assemblies
				if (asmFilename.StartsWith("system.", StringComparison.OrdinalIgnoreCase)
					|| asmFilename.StartsWith("microsoft.", StringComparison.OrdinalIgnoreCase))
				continue;

				try
				{
					// load the assembly and get all types in it with [RestService]
					Assembly asm = Assembly.LoadFile(dll);
					IList<Type> serviceTypes = ReflectionHelper
						.GetTypesWithAttribute(asm, typeof(RestServiceAttribute))
						.ToList();

					if (serviceTypes.Count > 0)
					{
						/*
						Sam Meacham - 11/7/2013 - Serious journal entry
						 * I just learned about the assembly attribute PreApplicationStartMethod, for
						 * dynamically loading http modules before the HttpApplication is created, but
						 * without requiring any entry in the web.config (this is possible via Microsoft.Web.Infrastructure,
						 * which was added to accommodate architecture in MVC).  It's also an excellent
						 * extension point to look at all the assemblies, do some reflection, set up anything
						 * you want, etc.
						 * 
						 * In the case of RestCake specifically, this is a great chance to scan all the assemblies
						 * for service types.  So I load the assemblies via Assembly.LoadFrom(string path) and Assembly.GetTypes()
						 * to inspect all the types (and find the ones with the attributes I'm looking for, like [RestService]).
						 * So I have Type objects here representing my service classes.  Later, you access services via
						 * RestCake.Cake.Services[Type], well I was getting a "key not found" exception.
						 * So here was my baffling case:
						 * Type a = the type loaded via Assembly.LoadFile(path).GetTypes()
						 * Type b = the same type passed at runtime to process an incoming request
						 * Every single value of the two types are equal (Name, FullName, assembly, asm version, etc)
						 * But (a == b) returns false.  I couldn't find ANY difference.  I was trying to hunt down
						 * if they were somehow from different app domains (thinking that if this is preinit, the HttpApp
						 * hasn't been created yet, we may not be in the domain yet?) and maybe some weird marshalling was going on.
						 * Well whatever it is, this line is the fix:
						BuildManager.AddReferencedAssembly(asm);
						Leaky abstractions, indeed.
						 */
						BuildManager.AddReferencedAssembly(asm);
						log("[RestService] classes found in " + asm.FullName);
						log("\t" + String.Join("\r\n\t", serviceTypes.Select(t => t.Name)));
						foreach (Type serviceType in serviceTypes)
							Cake.Services.Add(serviceType, new ServiceMetadata(serviceType));
					}
				}
				catch
				{
					// couldn't load the dll, probably not be a .net assembly
				}
			}
		}

//		private static void populateServiceTypes()
//		{
//			// Get every assembly in the current AppDomain that isn't an obvious MS assembly.
//			List<Assembly> assemblies = AppDomain.CurrentDomain
//				.GetAssemblies()
//				.Where(asm => !asm.FullName.StartsWith("mscorlib") && !asm.FullName.StartsWith("System.") && !asm.FullName.StartsWith("Microsoft"))
//				.ToList();
//			// In each of those assemblies, find all classes that have a [RestService] attribute.
//			foreach (Assembly asm in assemblies)
//			{
//				IEnumerable<Type> serviceTypes = ReflectionHelper.GetTypesWithAttribute(asm, typeof(RestServiceAttribute));
//				foreach (Type serviceType in serviceTypes)
//					Cake.Services.Add(serviceType, new ServiceMetadata(serviceType));
//			}
//		}

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
