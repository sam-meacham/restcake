<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Donate.aspx.cs" Inherits="ExampleServices.Donate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
	<div class="bar blue-bar">
		<div class="title">Donate to RestCake</div>
		If you're thinking of donating, please consider donating to <a href="http://json.codeplex.com">Json.NET</a> first, since it's been around longer,
		is used by more people, and RestCake would be impossible without it.

		<br />
		<br />

		<div class="fleft">If you still want to donate to RestCake, you can do so via PayPal.</div>

		<form action="https://www.paypal.com/cgi-bin/webscr" method="post">
			<input type="hidden" name="cmd" value="_s-xclick">
			<input type="hidden" name="hosted_button_id" value="GEK8GB7SMC458">
			<input type="image" src="https://www.paypal.com/en_US/i/btn/btn_donateCC_LG.gif" border="0" name="submit" alt="PayPal - The safer, easier way to pay online!">
			<img alt="" border="0" src="https://www.paypal.com/en_US/i/scr/pixel.gif" width="1" height="1">
		</form>

	</div>
	

</asp:Content>
