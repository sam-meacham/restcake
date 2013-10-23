<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AddressBook.aspx.cs" Inherits="ExampleServices.Examples.AddressBook" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
		.col
		{
			width: 150px;
			margin-right: 10px;
		}
		
		ul
		{
			margin-left: 0em;
			padding-left: 0em;
		}
		
		li
		{
			list-style: none;
			margin-bottom: 5px;
			clear: both;
		}
		
		ul li label
		{
			width: 80px;
			float: left;
			font-size: 0.8em;
		}
		
		
		#divPeople
		{
			max-height: 350px;
			overflow: auto;
		}
		
	</style>

	<script type="text/javascript" src="/AddressBook.Services/contacts/_js?jquery&base=true"></script>

	<script type="text/javascript">
		var g_contactsSvc = new RestCakeExamples.AddressBookClient("/AddressBook.Services/contacts/");

		var g_emailTypes = [];
		var g_phoneTypes = [];

		$(document).ready(function ()
		{
			dressUI();

			refreshContacts();
			refreshGroups();
			populateEnums();
		});

		function dressUI()
		{
			$("#lblAddresses").click(function ()
			{
				$("#divEditAddresses").toggle();
			});

			$("#lnkEditPerson").button({ icons: { primary: ""} });
		}

		function populateEnums()
		{
			g_contactsSvc.GetEmailTypes(function (types)
			{
				g_emailTypes = types;
			});

			g_contactsSvc.GetPhoneTypes(function (types)
			{
				g_phoneTypes = types;
			});
		}


		function refreshContacts()
		{
			g_contactsSvc.GetPeople(function (people)
			{
				var pplList = $("#divPeople");
				pplList.empty();
				$("#tContactLink").tmpl(people)
					.click(function ()
					{
						var person = $(this).tmplItem().data;
						viewPerson(person);
					})
					.appendTo(pplList);
			});
		}


		function refreshGroups()
		{
			g_contactsSvc.GetGroups(function (groups)
			{
				var grpList = $("#divGroups");
				grpList.empty();
				$("#tGroupLink").tmpl(groups).appendTo(grpList);
			});
		}


		function viewPerson(person)
		{
			$("#viewFname").html(person.Fname || "");
			$("#viewLname").html(person.Lname || "");
			$("#viewTitle").html(person.Title || "");
			$("#viewCompany").html(person.Company || "");
			$("#viewBirthday").html(person.Birthday || "");

			$("#viewAddresses").empty();
			//$("#tViewAddress").tmpl(person.Addresses).appendTo("#viewAddresses");

			// Emails

			// Phones

			// Websites

			$("#viewNotes").html(person.Notes || "");

			$("#divEditPerson").hide();
			$("#divViewPerson").show();

			$("#lnkEditPerson").unbind("click").click(function ()
			{
			editPerson(person);
			});
		}


		function editPerson(person)
		{
			$("#txtEditFirstName").val(person.Fname);
			$("#txtEditLastName").val(person.Lname);
			$("#txtEditTitle").val(person.Title);
			$("#txtEditCompany").val(person.Company);
			
			// Birthday
			$("#editDobMonth, #editDobDay, #txtEditDobYear").val("");
			if (person.Birthday)
			{
				var dob = new Date(person.Birthday);
				$("#editDobMonth").val(dob.getMonth() + 1);
				$("#editDobDay").val(dob.getDate());
				$("#txtEditDobYear").val(dob.getFullYear());
			}

			// Addresses
			$("#tEditAddress").tmpl(person.Addresses).appendTo("#divAddresses");

			// Emails

			// Phones

			// Websites


			$("#txtEditNotes").val(person.Notes);

			$("#divViewPerson").hide();
			$("#divEditPerson").show();
		}

		function addAddress()
		{
			$("#tEditAddress").tmpl().appendTo("#divAddresses");
		}
	</script>


	<!-- templates -->
	<script id="tContactLink" type="text/html">
		<div>
			<a href="javascript:void(0)">${Fname} ${Lname}</a>
		</div>
	</script>

	<script id="tGroupLink" type="text/html">
		<div>
			<a href="javascript:void(0)">${Name}</a>
		</div>
	</script>

	<script id="tViewAddress" type="text/html">
		<div class="address">
			<ul>
				<li>
					<label>Address Type</label>
					<select name="addressType">
						<option>TODO: Populate</option>
					</select>
				</li>

				<li>
					<label>Address</label>
					<input type="text" name="address1" value="${Address1}" />
				</li>

				<li>
					<label>Line 2</label>
					<input type="text" name="address2" value="${Address2}" />
				</li>

				<li>
					<label>City</label>
					<input type="text" name="city" value="${City}" />
				</li>

				<li>
					<label>State</label>
					<input type="text" name="state" value="${State}" />
				</li>

				<li>
					<label>Zip</label>
					<input type="text" name="zip" value="${Zip}" />
				</li>
			</ul>
		</div>
		<hr size="1" />
	</script>

	<script id="tEditAddress" type="text/html">
		<div class="address">
			<ul>
				<li>
					<label>Address Type</label>
					<select name="addressType">
						<option>TODO: Populate</option>
					</select>
				</li>

				<li>
					<label>Address</label>
					<input type="text" name="address1" value="${Address1}" />
				</li>

				<li>
					<label>Line 2</label>
					<input type="text" name="address2" value="${Address2}" />
				</li>

				<li>
					<label>City</label>
					<input type="text" name="city" value="${City}" />
				</li>

				<li>
					<label>State</label>
					<input type="text" name="state" value="${State}" />
				</li>

				<li>
					<label>Zip</label>
					<input type="text" name="zip" value="${Zip}" />
				</li>
			</ul>
		</div>
		<hr size="1" />
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
	<div class="bar red-bar">
		<div class="red-title">NOTE: This example is still incomplete.  Some things work, some don't.</div>
	</div>

	<div class="clear"></div>


	<div class="col bar blue-bar fleft">
		<div class="title">People</div>

		<div id="divPeople"></div>
	</div>

	<div class="col bar blue-bar fleft">
		<div class="title">Groups</div>
		
		<div id="divGroups"></div>
	</div>

	<div class="fleft" style="width: 630px">
		<div id="divViewPerson" class="bar red-bar" style="display:none">
			<div class="red-title">View Person</div>
			<div>
				<ul>
					<li>
						<label>First Name</label>
						<div id="viewFname"></div>
					</li>

					<li>
						<label>Last Name</label>
						<div id="viewLname"></div>
					</li>

					<li>
						<label>Title</label>
						<div id="viewTitle"></div>
					</li>

					<li>
						<label>Company</label>
						<div id="viewCompany"></div>
					</li>

					<li>
						<label>Birthday</label>
						<div id="viewBirthday"></div>
					</li>

					<li>
						<label>Addresses</label>
						<div id="viewAddresses"></div>
					</li>

					<li>
						<label>Emails</label>
						<div id="viewEmails"></div>
					</li>

					<li>
						<label>Phones</label>
						<div id="viewPhones"></div>
					</li>

					<li>
						<label>Websites</label>
						<div id="viewWebsites"></div>
					</li>

					<li>
						<label>Notes</label>
						<div id="viewNotes"></div>
					</li>
				</ul>
			</div>

			<div class="clear"></div>

			<a id="lnkEditPerson" href="javascript:void(0)">Edit</a>

		</div>

		<div id="divEditPerson" class="bar red-bar" style="display:none">
			<div class="red-title">Edit Person</div>
			<fieldset>
				<ul>
					<li>
						<label for="txtEditFirstName">First Name</label>
						<input id="txtEditFirstName" type="text" />
					</li>

					<li>
						<label for="txtEditLastName">Last Name</label>
						<input id="txtEditLastName" type="text" />
					</li>

					<li>
						<label for="txtEditTitle">Title</label>
						<input id="txtEditTitle" type="text" />
					</li>

					<li>
						<label for="txtEditCompany">Company</label>
						<input id="txtEditCompany" type="text" />
					</li>

					<li>
						<label>Birthday</label>

						Month
						<select id="editDobMonth">
							<option value=""></option>
							<option value="1">1</option>
							<option value="2">2</option>
							<option value="3">3</option>
							<option value="4">4</option>
							<option value="5">5</option>
							<option value="6">6</option>
							<option value="7">7</option>
							<option value="8">8</option>
							<option value="9">9</option>
							<option value="10">10</option>
							<option value="11">11</option>
							<option value="12">12</option>
						</select>

						Day
						<select id="editDobDay">
							<option value=""></option>
							<option value="1">1</option>
							<option value="2">2</option>
							<option value="3">3</option>
							<option value="4">4</option>
							<option value="5">5</option>
							<option value="6">6</option>
							<option value="7">7</option>
							<option value="8">8</option>
							<option value="9">9</option>
							<option value="10">10</option>
							<option value="11">11</option>
							<option value="12">12</option>
							<option value="13">13</option>
							<option value="14">14</option>
							<option value="15">15</option>
							<option value="16">16</option>
							<option value="17">17</option>
							<option value="18">18</option>
							<option value="19">19</option>
							<option value="20">20</option>
							<option value="21">21</option>
							<option value="22">22</option>
							<option value="23">23</option>
							<option value="24">24</option>
							<option value="25">25</option>
							<option value="26">26</option>
							<option value="27">27</option>
							<option value="28">28</option>
							<option value="29">29</option>
							<option value="30">30</option>
							<option value="31">31</option>
						</select>

						Year <input id="txtEditDobYear" type="text" size="4" />
					</li>

					<li>
						<label id="lblAddresses">
							Addresses
						</label>
						<div id="divEditAddresses">
							<div id="divAddresses" class="fleft">
							</div>

							<div class="clear"></div>

							<label>&nbsp;</label>
							<a href="javascript:addAddress()">Add an address</a>
						</div>
					</li>

					<li>
						<label>Emails</label>
					</li>

					<li>
						<label>Phones</label>
					</li>

					<li>
						<label>Websites</label>
					</li>

					<li>
						<label for="txtEditNotes">Notes</label>
						<textarea id="txtEditNotes" rows="5" cols="30"></textarea>
					</li>
				</ul>
			</fieldset>
		</div>
	</div>
	<div class="clear"></div>

</asp:Content>
