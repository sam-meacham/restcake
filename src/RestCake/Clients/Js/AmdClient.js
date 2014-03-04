define(["jquery", "modules/RestCakeClient"], function ($, RestCakeClient) {

	var $$JsClassName = function (serviceUrl) {
		// Private variables
		var self = this;
		this._client = new RestCakeClient(serviceUrl);

		this.defaultErrorHandler = function(err)
		{
			if (err && typeof err === "string")
			{
				alert("Error: " + err);
			}
			else if (err && typeof err === "object" && err.Message)
			{
				alert("Error: " + err.Message);
			}
		};

		$$ServiceMethods

	}; // end of $$JsClassName function

	return $$JsClassName;
});
