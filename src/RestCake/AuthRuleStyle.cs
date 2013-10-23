namespace RestCake
{
	/// <summary>
	/// Used with the AuthorizeAttribute to determine if only a single rule has to pass, or if all applicable rules have to pass
	/// </summary>
	public enum AuthRuleStyle
	{
		Any,
		All
	}
}
