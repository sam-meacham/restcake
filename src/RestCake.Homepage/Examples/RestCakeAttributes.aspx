<%@ Page Title="RestCake Attributes (Abandoning WCF Completely)" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="RestCakeAttributes.aspx.cs" Inherits="ExampleServices.Examples.RestCakeAttributes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
	<h2>So you're sold on RestCake...</h2>
	RestCake allows for an easy transition from WCF by allowing you to reuse the <span class="code">[ServiceContract]</span>
	<span class="code">[WebGet]</span> and <span class="code">[WebInvoke]</span> attributes, but if you've decided to only use
	RestCake, and not have WCF endpoints at all, then it can be a little misleading to use those attributes.  RestCake has its own
	set of attributes that can be used to designate service classes and methods.  They support all of the options that the WCF attributes
	have, and have a few extra options as well.

	<br />
	<br />

	<h2>[RestService]</h2>

	<h2>[Get], [Put], [Post] and [Delete]</h2>
</asp:Content>
