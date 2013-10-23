<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Routing.aspx.cs" Inherits="ExampleServices.Routing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
	<div class="bar blue-bar">
		<div class="title">How Routing works with RestCake services (and ASP.NET in general)</div>
		TODO: Explain the standard .NET RouteBase class and IRouteHandler interface, and the RestCake implementations of them:
		GenericHandlerRoute&lt;T&gt; and GenericHandlerRouteHandler&lt;T&gt;.

		<br />
		<br />

		TODO: Explain how ASP.NET routing works, with each incoming request calling GetRouteData() for all of the registered routes.
		If it returns null, skip it, if it returns the RouteData object, we have the match, use the IRouteHandler to get the IHttpHandler,
		and pass the HttpRequest off for processing.

		<br />
		<br />

		TODO: Maybe get some diagrams.  People love diagrams.
	</div>
</asp:Content>
