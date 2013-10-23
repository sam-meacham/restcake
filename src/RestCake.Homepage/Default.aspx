<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ExampleServices.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript" src="services/DualRest/_js?jquery?base=true"></script>

	<script type="text/javascript">
		var g_myService = new ExampleServices.DualServiceClient("services/DualRest/");

		function testServiceCall()
		{
			g_myService.SayHello("Sam", function (result)
			{
				alert(result);
			});
		}
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">

	Welcome to RestCake.
</asp:Content>
