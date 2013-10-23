// Create the <#= Namespace #> namespace if it doesn't exist
// TODO: Currently, this only works if the namespace is a single token.  Any subnamespaces will break this.  Need to create a createNamespace() method.
if (typeof (<#= Namespace #>) === "undefined")
	<#= Namespace #> = {};
	
if (typeof (Cake) === "undefined")
	Cake = {};

// Wrap in a closure, to be called immediately
(function ($)
{
	// Prevent multiple inclusions
	if (<#= Namespace #>.__included_<#= JsClassName #>)
		return;
	<#= Namespace #>.__included_<#= JsClassName #> = true;

	<#= Namespace #>.<#= JsClassName #> = function (serviceUrl)
	{
		// Private variables
		this._client = new RestCake.serviceClient(serviceUrl);

		// ********************************************************************************
		// *** Public service methods *****************************************************
		// ********************************************************************************

<#= ServiceMethods #>

	}; // end of "<#= Namespace #>.<#= JsClassName #> = function()..."
	
	// Create the client
	<#= JsClientDeclaration #>

})(jQuery); // end of closure wrapper (called immediately)
