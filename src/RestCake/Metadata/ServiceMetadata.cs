using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RestCake.Util;


namespace RestCake.Metadata
{
	public class ServiceMetadata
	{
		/// <summary>
		/// The type of the service class
		/// </summary>
		public Type Type { get; private set; }

		/// <summary>
		/// This is the value set in the [ServiceContract] or [RestService] attribute's Name value, and it's what's used
		/// for the generated clients.
		/// </summary>
		public string ServiceName { get; private set; }

		/// <summary>
		/// This is the value set in the [ServiceContract] or [RestService] attribute's Namespace value, and it's what's used
		/// for the generated clients.
		/// </summary>
		public string ServiceNamespace { get; private set; }

		/// <summary>
		/// Don't start this with a ~ or a /.  Don't end it with a / either. Use something like "user" or "rest/user" or "api/user", etc.
		/// </summary>
		public string Route { get; set; }

		/// <summary>
		/// When using <see cref="Cake.JqueryClientScript{T}" />, if this has a value, then the &amp;clientVarName={value} will be appended to the src= of the script.
		/// </summary>
		public string JsClientVarName { get; set; }


		/// <summary>
		/// Whether or not the /_help address will return a help page showing the services API
		/// </summary>
		public bool EnableHelp { get; private set; }


		/// <summary>
		/// The service methods tagged with the RestCake attribute [Get], [Put], [Post], and [Delete].
		/// </summary>
		public List<MethodMetadata> Methods { get; private set; }


		public AuthorizeAttribute AuthRules { get; private set; }

		/// <summary>
		/// This is a list of non-system types (types that are not part of the System.* libs) that are exposed via service methods, either as
		/// parameters or as return types.
		/// </summary>
		public List<Type> ExposedNonSystemTypes { get; private set; }

		public ServiceMetadata(Type type)
		{
			Type = type;

			// Get the [RestService] attribute
			RestServiceAttribute serviceAttribute = ReflectionHelper.GetAttribute<RestServiceAttribute>(Type);

			// If the service name and namespace are provided, use them.  If not, use the actuall class' name and namespace.
			// TODO: If the service class has no attribute, this will throw a "cannot perform runtime binding on a null reference" exception.
			// Fix this to throw a more specific exception.
			ServiceName = String.IsNullOrWhiteSpace(serviceAttribute.Name) ? Type.Name : serviceAttribute.Name;
			ServiceNamespace = String.IsNullOrWhiteSpace(serviceAttribute.Namespace) ? Type.Namespace : serviceAttribute.Namespace;

			EnableHelp = false;
			EnableHelp = serviceAttribute.EnableHelp;
			Route = serviceAttribute.Route;
			JsClientVarName = serviceAttribute.JsClientVarName;

			Methods = new List<MethodMetadata>();
			populateMethods();

			AuthRules = ReflectionHelper.GetAttribute<AuthorizeAttribute>(Type);

			ExposedNonSystemTypes = new List<Type>();
			populateExposedNonSystemTypes();
		}


		private void populateMethods()
		{
			foreach (MethodInfo method in ReflectionHelper.GetMethodsWithAttribute(Type, typeof(GetAttribute))
					.Concat(ReflectionHelper.GetMethodsWithAttribute(Type, typeof(PutAttribute)))
					.Concat(ReflectionHelper.GetMethodsWithAttribute(Type, typeof(PostAttribute)))
					.Concat(ReflectionHelper.GetMethodsWithAttribute(Type, typeof(DeleteAttribute))))
				Methods.Add(new MethodMetadata(this, method));
		}


		private void populateExposedNonSystemTypes()
		{
			Func<Type, bool> includeType = type => type.Module.Name != "mscorlib.dll"
			                                       && !type.IsArray
			                                       //&& !type.IsGenericType
			                                       && !ExposedNonSystemTypes.Contains(type);

			foreach(MethodMetadata meta in Methods)
			{
				// The method's return type
				if (includeType(meta.Method.ReturnType))
					ExposedNonSystemTypes.Add(meta.Method.ReturnType);

				// The methods parameters
				foreach(ParameterInfo param in meta.Method.GetParameters())
				{
					if (includeType(param.ParameterType))
						ExposedNonSystemTypes.Add(param.ParameterType);

					// Have to check for generic type params as well, and recursively, at that, to find things like List<List<Dictionary<string, CustomType>>>.
					if (param.ParameterType.IsGenericType)
					{
						foreach(Type t in ReflectionHelper.GetAllTypeParams(param.ParameterType, null))
						{
							if (includeType(t))
								ExposedNonSystemTypes.Add(t);
						}
					}
				}
			}
		}

	}
}
