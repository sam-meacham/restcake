<%@ Page Title="" Language="C#" MasterPageFile="~/Examples/ExampleMaster.master" AutoEventWireup="true" CodeBehind="WebMessageBodyStyleComparisons.aspx.cs" Inherits="ExampleServices.Examples.WebMessageBodyStyleComparisons" %>

<asp:Content ContentPlaceHolderID="head2" runat="server">
	<!-- Include the dynamically created js client classes, including the client "base" class they depend on (based on Rick Strahl's wcf proxy class) -->
	<script type="text/javascript" src="/AddressBook.Services/bodyStyle_cake/_js?jquery&base=true"></script>

	<script type="text/javascript">
		// ********************************************************************************
		// *** Create the javascript client object, so we can easily call our service methods
		// ********************************************************************************
		var g_dualService = new RestCakeExamples.BodyStyleClient("/AddressBook.Services/bodyStyle_cake/");

		// This changes the javascript client's url, which is considered "private", but for the sake of this dual service example,
		// I'm changing it directly so I can switch between the 2 services.  This works, because they have the exact same API.
		function useWcf()
		{
			g_dualService._client._serviceUrl = "/AddressBook.Services/bodyStyle_wcf/";
		}

		function useRestCake()
		{
			g_dualService._client._serviceUrl = "/AddressBook.Services/bodyStyle_cake/";
		}

		function wrappedTest()
		{
			var id = $("#txtPersonID").val();
			g_dualService.WrappedTest(id, function (person)
			{
				// Do nothing
			});
		}

		function wrappedRequestTest()
		{
			var id = $("#txtPersonID").val();
			g_dualService.WrappedRequestTest(id, function (person)
			{
				// Do nothing
			});
		}

		function wrappedResponseTest()
		{
			var id = $("#txtPersonID").val();
			g_dualService.WrappedResponseTest(id, function (person)
			{
				// Do nothing
			});
		}

		function bareTest()
		{
			var id = $("#txtPersonID").val();
			g_dualService.BareTest(id, function (person)
			{
				// Do nothing
			});
		}

		$(document).ready(function ()
		{
			$("#rawResult").attr("rows", 18);

			useRestCake();
			$("#rdoRestCake").attr("checked", true);
			$("#divServiceType").buttonset();
			$("#divServiceType :input").change(function ()
			{
				if ($("#rdoWcf").attr("checked"))
					useWcf();
				else
					useRestCake();
				$.growl("Service Url Changed", g_dualService._client._serviceUrl);
			});
		});
	</script>
</asp:Content>

<asp:Content ContentPlaceHolderID="contentExampleTitle" runat="server">
	WebMessageBodyStyle Comparisons
</asp:Content>

<asp:Content ContentPlaceHolderID="contentNotes" runat="server">
	The <span class="code">WebMessageBodyStyle</span> parameter on your <span class="code">WebGet</span> and <span class="code">WebInvoke</span>
	attribute determines how the data will be packaged.  The four service methods in this example all do the exact same thing:  return the PersonDto with ID = 1.
	The only difference is how the data is packaged.

	<br />
	<br />

	Note that <strong>RestCake</strong> abstracts all this away from you when you use the javascript client classes.  You simply need to call the service
	methods using a js client instance, and the client will take care of wrapping the request or unwrapping the response if necessary.
	When using the js proxies, you really don't have to know what WebMessageBodyStyle is being used.   You only need to be aware of it when you
	create your actual service methods in <strong>RestHttpHandler</strong> classes.  If you have multiple POSTed arguments, you are required to use
	<span class="code">WebMessageBodyStyle.Wrapped</span> or <span class="code">WebMessageBodyStyle.WrappedRequest</span>, since only a "single" posted argument
	can be sent.
</asp:Content>

<asp:Content ContentPlaceHolderID="contentOperations" runat="server">
	<div id="divServiceType">
		<input id="rdoWcf" type="radio" name="grpServiceType" />
		<label for="rdoWcf">WCF</label>

		<input id="rdoRestCake" type="radio" name="grpServiceType" checked="checked" />
		<label for="rdoRestCake">RestCake</label>
	</div>

	<div class="title">WebMessageBodyStyle Values</div>
	Notice the differences in data sent and received at the right.
	<br />
	ID of Person to fetch <input id="txtPersonID" type="text" size="2" />
	<ul>
		<li><a href="javascript:wrappedTest()">Wrapped</a></li>
		<li><a href="javascript:wrappedRequestTest()">WrappedRequest (bare response)</a></li>
		<li><a href="javascript:wrappedResponseTest()">WrappedResponse (bare request)</a></li>
		<li><a href="javascript:bareTest()">Bare</a></li>
	</ul>
</asp:Content>

<asp:Content ContentPlaceHolderID="contentBottom" runat="server">
</asp:Content>
