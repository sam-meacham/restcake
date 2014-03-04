define(["jquery", "restcake-client"], function ($, RestCakeClient) {

	var $$JsClassName = function (serviceUrl) {
		// Private variables
		this._client = new RestCakeClient(serviceUrl);

		$$ServiceMethods

	}; // end of $$JsClassName function

	return $$JsClassName;
});
