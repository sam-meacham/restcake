using System;

namespace RestCake
{
	/// <summary>
	/// The base class for the http verb attribute classes that are placed on methods in a RestCakeHandler (a service class) to expose those methods over http.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public abstract class VerbAttributeBase : Attribute
	{
		/// <summary>
		/// You may want to specify a specific method name, without having to declare the whole UriTemplate.  This is handy if
		/// you want the default parameter convention (either UrlStyle.Segments or UrlSegment.QueryString), but you don't want to default
		/// to the name of the method (you want to specify a specific method name).
		/// Note that if a UriTemplate is declared, this value is completely ignored.
		/// </summary>
		public string MethodName { get; set; }


		public string UriTemplate { get; set; }

		/// <summary>
		/// Defaults to Bare, just like WCF.
		/// </summary>
		public BodyStyle BodyStyle { get; set; }

		/// <summary>
		/// Default to UrlSegment.
		/// </summary>
		public UrlStyle UrlStyle { get; set; }


		protected VerbAttributeBase()
		{
			// default values
			UriTemplate = null;
			BodyStyle = BodyStyle.Bare;
			UrlStyle = UrlStyle.UriSegments;
		}

	}
}
