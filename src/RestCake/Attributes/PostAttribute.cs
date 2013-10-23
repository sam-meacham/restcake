namespace RestCake
{
	/// <summary>
	/// Applying this attribute on a method in a class with the [RestService] attribute will expose
	/// that method to access via the HTTP POST verb
	/// </summary>
	public class PostAttribute : VerbAttributeBase
	{ }
}
