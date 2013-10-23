<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Consulting.aspx.cs" Inherits="ExampleServices.Consulting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
	<div class="bar blue-bar">
		<div class="title">Custom App Stack</div>
		I am available for hire on small- to medium-sized projects.  I'm not interested in writing the code for your business rules.
		I simply create the layers for your application's architecture, and hand that off to your developers, and they can work the business
		logic into it.
		<ul>
			<li>Create a Data Layer based on Entity Framework v4, ready to go for use in ASP.NET and Service scenarios, complete with DTOs (data transfer objects)</li>
			<li>Create a working REST service layer for basic CRUD operations on your entities</li>
			<li>Create a few sample web pages that use the javascript proxies from the REST service layer, to demonstrate how to use it</li>
		</ul>

		The application stacks up like this:
		
		<br />

		<span class="code">
			SQL Server &nbsp;=&gt;&nbsp;
			EFv4 Data Layer / DTOs &nbsp;=&gt;&nbsp;
			REST Service Layer &nbsp;=&gt;&nbsp;
			js proxies / Web UI
		</span>

		<br />
		<br />

		The code is incredibly flexible and very straightforward, so it can be easily modified by your developers.  You'll own the code I deliver.
		If you're new to Entity Framework and REST services, and your developers aren't caught up with these technologies, having a working N-tier setup
		delivered to you is the way to go.  It's something your developers will be able to quickly learn and adapt to.

		<br />
		<br />

		Contact me at <a href="mailto:diesam@gmail.com">diesam@gmail.com</a> for inquiries.
	</div>
</asp:Content>
