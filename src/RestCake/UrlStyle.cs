namespace RestCake
{
	/// <summary>
	/// When a service method does not specify a UriTemplate, a default one is created.
	/// The default style is to use UriSegment parameters, /{param1}/{param2}, etc.
	/// If a style of QueryString is used, it will be ?p1={p1}&amp;p2={p2}, etc.
	/// </summary>
	public enum UrlStyle
	{
		UriSegments,
		QueryString
	}
}
