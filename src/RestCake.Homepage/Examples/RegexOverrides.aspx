<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="RegexOverrides.aspx.cs" Inherits="ExampleServices.Examples.RegexOverrides" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
	<a name="routing-overview"></a>
	<div class="bar light-blue-bar">
		<div class="title"><a href="#routing-overview">Brief Routing Overview</a></div>

		When a request comes from the client, the ASP.NET runtime will check the URL of the request against
		all of the registered routes in the <span class="code">RouteTable</span>.  If the request's url
		matches any of the <span class="code">UriTemplate</span>'s in those routes (meaning the <span class="code">RouteBase.GetRouteData()</span>
		method returned a valid <span class="code">RouteData</span> object), then ASP.NET uses the
		<span class="code">IRouteHandler</span> from the RouteData object that was returned from RouteBase.GetRouteData().
		That IRouteHandler is used to return a valid <span class="code">IHttpHandler</span> that ASP.NET will hand the incoming request
		off to for processing.

		<br />
		<br />

		With <span class="code">RestHttpHandler</span>, the <span class="code">ProcessRequest()</span> method usually takes the incoming url,
		and checks to see if it matches any of the UriTemplates for the service methods in the class (if it doesn't it returns an error).
		However, before it checks the incoming url against the methods' UriTemplates, it checks the Regex Overrides.
	</div>

	<a name="regex-overrides"></a>
	<div class="bar light-blue-bar">
		<div class="title"><a href="#regex-overrides">Regex Overrides</a></div>
		
		Registering a regex override is fairly simple, but some important concepts are involved.  Since an IHttpHandler may or may not be
		reusable, you may not know if the current handler is a new or old object.  The regex overrides are static in nature, however, so
		we don't want to create them as instance data.  They should only be registered once, so we'll create them in the static constructor.
		Take note, however, that we do this because we want our handler methods (the methods that are called when a regex matches) to
		be placed in the service class, because that's probably where it belongs.  If you have an override for some other reason, you
		might want to register the regex overrides in your Global.asax.  That's ok too.

		<br />
		<br />

		Here's an example from the AddressBookService class (in the AddressBook.Services project).

		<script type="syntaxhighlighter" class="brush: csharp">
			[ServiceContract(Namespace="RestCakeExamples", Name="AddressBook")]
			public class AddressBookService : RestHttpHandler
			{
				// Set up any extra regex overrides in the service class's static constructor
				static AddressBookService()
				{
					AddRegexOverride(typeof(AddressBookService), new Regex("/hello"), sayHello);
				}

				// This is the method that will be used if the incoming requests's url matches the regex
				private static void sayHello(Type type, string everythingAfterRouteUrl, HttpContext context)
				{
					context.Response.ContentType = "text/plain";
					context.Response.Write(
						"Hello World!"
						+ Environment.NewLine
						+ everythingAfterRouteUrl
					);
				}
					
				// ...
			}
		</script>

		Notice that the regex starts with <span class="code">/</span>.  They should all start that way.  The regex will start to match
		<em>after</em> what already matched from the UriTemplate for the route.  Also note in the example that the regex is "open ended" on the right side.
		So navigating to <span class="code">/hello</span> or <span class="code">/hello_asdf</span> will both match.  That's why the handler has a
		parameter named <span class="code">everythingAfterRouteUrl</span>.  It will contain the initial "/", and everything that comes after.

		<br />
		<br />

		To see the result of this particular override, go to
		<a href="http://restcake.net/AddressBook.Services/contacts/hello_asdf">http://restcake.net/AddressBook.Services/contacts/hello_asdf</a>
		or browse the same thing on
		<a href="http://localhost/AddressBook.Services/contacts/hello_asdf">localhost</a>
		if you're running this on your own machine.

		<br />
		<br />

		The dynamic javascript proxies make use of the regex overrides to return the javascript code.

	</div>
</asp:Content>
