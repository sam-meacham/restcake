namespace RestCake
{
	/// <summary>
	/// Like WCF, unless specified otherwise on a method, BodyStyle defaults to Bare.
	/// </summary>
	public enum BodyStyle
	{
		Bare = 0,
		Wrapped,
		WrappedRequest,
		WrappedResponse
	}
}
