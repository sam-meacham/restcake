namespace RestCake
{
	/// <summary>
	/// Applying this attribute on a method in a class with the [RestService] attribute will expose
	/// that method to access via the HTTP DELETE verb
	/// </summary>
	public class DeleteAttribute : VerbAttributeBase
	{ }
}
