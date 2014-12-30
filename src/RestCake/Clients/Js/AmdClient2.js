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

		this.getDefaultErrorCallback = function (msg) {
			return function (err) { self.defaultErrorHandler(msg); };
		};

		$$ServiceMethods

	}; // end of $$JsClassName function

	var baseurl = "$$BaseUrl";
	var client = new $$JsClassName(baseurl);
	return client;
});
