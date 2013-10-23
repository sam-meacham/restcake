<%@ Page Title="" Language="C#" MasterPageFile="~/Examples/ExampleMaster.master" AutoEventWireup="true" CodeBehind="ErrorHandling.aspx.cs" Inherits="ExampleServices.Examples.ErrorHandling" %>

<asp:Content ID="Content2" ContentPlaceHolderID="contentExampleTitle" runat="server">
	Error Handling (Exceptions thrown in your service)
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
	<!-- math service client -->
	<script type="text/javascript" src="/AddressBook.Services/math/_js?jquery&base=true"></script>

	<script type="text/javascript">
		// create the math service client instance
		var g_mathClient = new RestCakeExamples.MathServiceClient("/AddressBook.Services/math/");

		function divide_success()
		{
			// call the divide function, providing an inline success handler
			g_mathClient.divide(10, 2, function (result)
			{
				$.growl("Success", "Result: " + result);
			});
		}

		function divide_fail()
		{
			// Call the divide function, providing inline success and error handlers.
			// Note we are dividing by 0, and we'll only get to the error handler from here.
			g_mathClient.divide(10, 0,
				// Success handler.  This will never execute (we're trying to divide by 0)
				function (result)
				{
					$.growl("Success", "Result: " + result);
				},
				// error handler
				on_error);
		}


		function wrong_type_fail()
		{
			// Call the divide function, with the wrong kind of data types
			g_mathClient.divide(10, "2a",
				// Success handler.  Will never be called because this call always fails.
				function (result)
				{
					$.growl("Success", "Result: " + result);
				},
				// Error handler
				on_error);
		}


		function on_error(err, userContext, xhr, textStatus, jsErrThrown, contentType)
		{
			if (err != null && err.IsRestException)
			{
				$.growl("Error", "See response details");
			}
			else
			{
				$.growl("Error", "Unknown Error");
			}
		}


		function validation_exception()
		{
			g_mathClient.val_fail(
				// Success handler.  Will never be called, because this call always fails.
				function (result)
				{
					$.growl("Success", "Result: " + result);
				},
				function (err)
				{
					$.growl("Validation Error", "See response details");
				});
		}


	</script>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="contentNotes" runat="server">
	RestCake has excellent error handling mechanisms.  From your service methods, you can throw any kind of exception you want.
	The <span class="code">RestHttpHandler</span>'s <span class="code">ProcessRequest</span> method calls your service method using reflection,
	so any unhandled exception in your service class comes back to the RestHttpHandler as a <span class="code">TargetInvocationException</span>.
	RestCake looks for those specifically, and does 2 things.  First, it creates a new <span class="code">RestException</span>, with the TargetInvocationException's InnerException
	as the wrapped exception, and it serializes that new RestException to json and sends it back to the calling client.  Then the response stream is flushed and closed,
	and the original TargetInvocationException is rethrown.  This causes your hosting applications Application_Error handler to fire, so that if you're doing
	any special handling or logging, it will still occur.  The response stream is closed, because an unhandled exception in .net will cause the asp.net runtime to
	try and send a yellow screen of death (YSOD) back to the client, as html.  Since we've already sent a json reponse (the serialized RestException), we don't want
	the YSOD to be tacked on to the end of that stream.

	<br />
	<br />

	Short version: errors are sent back to the client as serialized <span class="code">RestException</span>s, but errors are still rethrown in RestCake so that 
	<span class="code">Application_Error</span> still fires in your hosting application.

	<br />
	<br />

	<h2>Handling the error on the client</h2>
	If the javascript client receives json representing a RestException, it will deserialize the json, and call your error callback, passing these arguments:
	<div class="code">
		errorCallback(restException, userContext, xhr, textStatus, errorThrown, "json");
	</div>
	<strong>restException</strong> is the deserialized RestException.  <strong>userContext</strong> will be whatever object you passed in for userContext, or null.
	The next three, <strong>xhr, textStatus and errorThrown</strong> are values that jQuery normally sends to an error callback.  I just pass them along here in case they are useful.
	The last one is simply the content type.  It will always be "json" for RestExceptions that are sent back to the client.

	<br />
	<br />

	<h2>Validation Errors</h2>
	If you want an error to be sent back to the client, but do not want an unhandled exception in your .NET code (meaning you don't want
	<span class="code">Application_Error</span> to fire), then you can throw a new <span class="code">RestValidationException</span>, and
	the serialized exception will be sent down to the client, but the exception will not be rethrown in the .NET code.  The default status code of
	the response will be a 500 (internal server error), but you can override it in the RestValidationException's constructor if you want a different status code.
	Because the default status code is an error code, your error callback will be called, not your success handler.  <span class="code">RestException</span> has
	a boolean property, <span class="code">IsRestException</span> that is always set to true (to easily differentiate it from other objects that may have been passed back).
	<span class="code">RestValidationException</span> has an additional property, <span class="code">IsRestValidationException</span>, that is set to true, to easily differentiate
	it from regular RestException objects.

	<br />
	<br />

	Use the examples below to look at the structure of a <span class="code">RestExcpetion</span>
	and a <span class="code">RestValidationExcpetion</span>, to see how you'd use them in your client error callback.
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="contentOperations" runat="server">
	<ul>
		<li><a href="javascript:void(0)" onclick="divide_fail()">Divide fail (div by zero)</a></li>
		<li><a href="javascript:void(0)" onclick="divide_success()">Divide success</a></li>
		<li><a href="javascript:void(0)" onclick="wrong_type_fail()">Pass incorrect data type (pass string where double is expected)</a></li>
		<li><a href="javascript:void(0)" onclick="validation_exception()">Validation exception</a></li>
	</ul>
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="contentBottom" runat="server">
</asp:Content>
