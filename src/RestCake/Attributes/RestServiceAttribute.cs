using System;

namespace RestCake
{
	[AttributeUsage(AttributeTargets.Class)]
	public class RestServiceAttribute : Attribute
	{
		public string Namespace { get; set; }
		public string Name { get; set; }
		public bool EnableHelp { get; set; }
		public string Route { get; set; }
		public string JsClientVarName { get; set; }
	}
}
