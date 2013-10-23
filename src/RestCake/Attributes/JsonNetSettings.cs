using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace RestCake
{
	/// <summary>
	/// This attribute can be placed on a rest service class or method to specify the Json.NET serialization settings that are
	/// to be used for that entire class or that specific method.  You can have default settings for a whole class, and then specific settings
	/// for a particular method if needed.
	/// If you need to specify more advanced settings, you may want to override RestHttpHandler.GetSerializer() in your service class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class JsonNetSettings : Attribute
	{
		public ConstructorHandling ConstructorHandling { get; set; }
		public DefaultValueHandling DefaultValueHandling { get; set; }
		public MissingMemberHandling MissingMemberHandling { get; set; }
		public NullValueHandling NullValueHandling { get; set; }
		public ObjectCreationHandling ObjectCreationHandling { get; set; }
		public PreserveReferencesHandling PreserveReferencesHandling { get; set; }
		public ReferenceLoopHandling ReferenceLoopHandling { get; set; }
		public FormatterAssemblyStyle TypeNameAssemblyFormat { get; set; }
		public TypeNameHandling TypeNameHandling { get; set; }
		public IEnumerable<JsonConverter> Converters { get; set; }

		public JsonNetSettings()
		{
			ConstructorHandling = ConstructorHandling.Default;
			DefaultValueHandling = DefaultValueHandling.Include;
			MissingMemberHandling = MissingMemberHandling.Ignore;
			NullValueHandling = NullValueHandling.Include;
			ObjectCreationHandling = ObjectCreationHandling.Auto;
			PreserveReferencesHandling = PreserveReferencesHandling.None;
			ReferenceLoopHandling = ReferenceLoopHandling.Error;
			TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple;
			TypeNameHandling = TypeNameHandling.None;

			// This is the default for RestCake.
			// Note that if in the attribute they specify a value, this won't be used, because the attribute property assignments happen after.
			Converters = new List<JsonConverter>()
			{
				new IsoDateTimeConverter()
			};
		}

		internal JsonSerializerSettings GetSettings()
		{
			return new JsonSerializerSettings()
			{
				ConstructorHandling = ConstructorHandling,
				DefaultValueHandling = DefaultValueHandling,
				MissingMemberHandling = MissingMemberHandling,
				NullValueHandling = NullValueHandling,
				ObjectCreationHandling = ObjectCreationHandling,
				PreserveReferencesHandling = PreserveReferencesHandling,
				ReferenceLoopHandling = ReferenceLoopHandling,
				TypeNameAssemblyFormat = TypeNameAssemblyFormat,
				TypeNameHandling = TypeNameHandling,

				// This is the default for RestCake.
				// Note that if in the attribute they specify a value, this won't be used, because the attribute property assignments happen after.
				Converters = Converters.ToList()
			};
		}
	}
}
