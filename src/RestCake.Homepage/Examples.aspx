<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Examples.aspx.cs" Inherits="ExampleServices._Examples" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
	<div class="bar blue-bar">
		<div class="blue-title">"Dual" services (a single service class with both WCF and RestCake endpoints (routes))</div>
		<ul>
			<li><a href="Examples/DualService.aspx">Dual Service</a></li>
			<li><a href="Examples/WebMessageBodyStyleComparisons.aspx">WebMessageBodyStyle Comparisons</a></li>
		</ul>

		<div class="blue-title">RestCake only examples (no WCF)</div>
		<ul>
			<li><a href="Examples/AddressBook.aspx">Complete Address Book Application</a></li>
			<li><a href="Examples/RestCakeAttributes.aspx">Using the RestCake Attributes (adandoning WCF completely)</a></li>
		</ul>

		<ul>
		</ul>

		<div class="blue-title">Calling from a server</div>
		<ul>
			<li>Using RestSharp (TODO: WRITE)</li>
			<li>How to bypass FormsAuthentication (or any other standard authentication) (TODO: WRITE)</li>
			<li>Settings up internal and external endpoints for different kinds of security (FormsAuth/Whitelist on internal, Authorization header on external)</li>
		</ul>

		<div class="blue-title">Error Handling</div>
		<ul>
			<li><a href="Examples/ErrorHandling.aspx">Overview of error handling in RestCake</a></li>
		</ul>

		<div class="blue-title">Regex Overrides</div>
		<ul>
			<li><a href="Examples/RegexOverrides.aspx">Overview of Regex Overrides</a></li>
		</ul>
	</div>
</asp:Content>
