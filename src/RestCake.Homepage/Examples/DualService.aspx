<%@ Page Title="" Language="C#" MasterPageFile="~/Examples/ExampleMaster.Master" AutoEventWireup="true" CodeBehind="DualService.aspx.cs" Inherits="ExampleServices.Examples.DualService" %>
<%--
	TODO: Get rid of all the alerts, and set up proper error handling, either by showing the results in the rawResults box, or with a jgrowl, or both.
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head2" runat="server">
	<!-- These styles are just used for laying out the create and edit forms -->
	<style type="text/css">
		.edit .label
		{
			float: left;
			width: 100px;
			margin-bottom: 10px;
		}
		
		.edit .field, .edit input
		{
			float: left;
			margin-bottom: 10px;
		}
	</style>

	<!-- Include the dynamically created js client classes, including the client "base" class they depend on (based on Rick Strahl's wcf proxy class) -->
	<script type="text/javascript" src="/AddressBook.Services/dual_cake/_js?jquery&base=true"></script>

	<script type="text/javascript">
		// ********************************************************************************
		// *** Create the javascript client object, so we can easily call our service methods
		// ********************************************************************************
		var g_dualService = new RestCakeExamples.DualServiceClient("/AddressBook.Services/dual_cake/");

		// This changes the javascript client's url, which is considered "private", but for the sake of this dual service example,
		// I'm changing it directly so I can switch between the 2 services.  This works, because they have the exact same API.
		function useWcf()
		{
			g_dualService._client._serviceUrl = "/AddressBook.Services/dual_wcf/";
		}

		function useRestCake()
		{
			g_dualService._client._serviceUrl = "/AddressBook.Services/dual_cake/";
		}

		// This method simply takes a PersonDto object, and creates a <tr> element that shows the data.
		function getPersonTr(person)
		{
			if (person == null)
				return null;
			var tr = $("#t_personTr").tmpl(person);
			return tr;
		}

		// This takes a person object, and adds it to the results table
		function addPersonToTable(person)
		{
			var tr = getPersonTr(person);
			if (tr == null)
				return;
			$("#tblPersonResults").append(tr);
			$(tr).effect("highlight", {}, 1700);
		}

		// ********************************************************************************
		// *** Uses the js client to call the service method GetPeople(). ******************
		// ********************************************************************************
		function getPeople()
		{
			g_dualService.GetPeople(function (people)
			{
				$.each(people, function (i, person)
				{
					addPersonToTable(person);
				});
			},
			// error
			function (xhr)
			{
				alert("error");
			});
		}

		// ********************************************************************************
		// *** Uses the js client to call the service method GetPerson() *******************
		// ********************************************************************************
		function getPerson()
		{
			var personID = $("#txtPersonID").val();
			//
			// Use the javascript client to call the REST service
			//
			g_dualService.GetPerson(personID, function (person)
			{
				addPersonToTable(person);
			},
			// error
			function (xhr)
			{
				alert("error");
			});
		}


		function loadAddresses(el)
		{
			var td = $(el).closest("td");
			var tr = td.parent();
			var person = tr.tmplItem().data;

			g_dualService.GetAddresses(person.ID, function (addresses)
			{
				$(td).empty();
				$("#t_address").tmpl(addresses).appendTo(td);
			});
		}

		// ********************************************************************************
		// *** Uses the js client to call the service method DeletePerson() ****************
		// ********************************************************************************
		function deletePerson(td)
		{
			var tr = $(td).closest("tr");
			var person = tr.tmplItem().data;
			g_dualService.DeletePerson(person.ID, function ()
			{
				tr.remove();
			},
			// error
			function (xhr)
			{
				alert("error");
			});
		}

		// Loads a person into the edit form, and shows the edit form.
		// We get the parent <tr> element from the <td> that's passed in, and the person object
		// is stored in the <tr> at .data("person")
		function editPerson(el)
		{
			var td = $(el).closest("td");
			var tr = td.closest("tr");
			var person = tr.tmplItem().data;

			$("#divPersonID").html(person.ID);
			$("#editPersonFname").val(person.Fname);
			$("#editPersonLname").val(person.Lname);
			$("#editPersonEmail").val(person.Email);

			var divEdit = $("#divEditPerson");
			divEdit.show();
			divEdit.data("person", person);
		}

		// ********************************************************************************
		// *** Gets the person object both that's bound to the edit form in .data("person"),
		// *** and creates a new one based on the form field values.  Then using the js client,
		// *** the person object is updated by calling the UpdatePerson() service method.
		// ********************************************************************************
		function savePersonEdit()
		{
			var divEdit = $("#divEditPerson");
			var person = divEdit.data("person");

			var personData =
			{
				ID: person.ID,
				Fname: $("#editPersonFname").val(),
				Lname: $("#editPersonLname").val(),
				Email: $("#editPersonEmail").val()
			};

			//
			// Use the javascript client to call the REST service
			//
			g_dualService.UpdatePerson(personData, function (updatedPerson)
			{
				$("#divEditPerson").hide();
				addPersonToTable(updatedPerson);
			});
		}

		// Hides the edit person form
		function cancelPersonEdit()
		{
			$("#divEditPerson").hide();
		}

		// Shows the create person form
		function createPerson()
		{
			var divNew = $("#divNewPerson").show();
		}

		// ********************************************************************************
		// *** Creates a new person object based on the form values, and using the js client,
		// *** calls the service method CreatePerson()
		// ********************************************************************************
		function saveNewPerson()
		{
			var personData =
			{
				Fname: $("#createPersonFname").val(),
				Lname: $("#createPersonLname").val(),
				Email: $("#createPersonEmail").val()
			};

			//
			// Use the javascript client to call the REST service
			//
			g_dualService.CreatePerson(personData, function (newPerson)
			{
				$("#divNewPerson").hide();
				addPersonToTable(newPerson);
			});
		}

		// Hides the create person form
		function cancelNewPerson()
		{
			$("#divNewPerson").hide();
		}

		// Clears all the <tr>s in the results table
		function clearPersonTable()
		{
			$("#tblPersonResults tbody").empty();
		}


		function NoError_NoWrappedRequest()
		{
			g_dualService.NoError_NoWrappedRequest("first", "second", "third", "fourth", "fifth", function (result)
			{
				$.growl("Success", "NoError_NoWrappedRequest() completed successfully");
			});
		}


		function Test_MultiplePost_PlusDto()
		{
			var person = {
				Fname: "Sam",
				Lname: "Meacham",
				Email: "diesam@gmail.com"
			};

			g_dualService.Test_MultiplePost_PlusDto("first", "second", "third", "fourth", "fifth", 66, new Date(), person, function (result)
			{
				$.growl("Success", "Test_MultiplePost_PlusDto() completed successfully");
			});
		}


		$(document).ready(function ()
		{
			$("#lnkSavePersonEdit").button({ icons: { primary: "ui-icon-disk"} });
			$("#lnkSaveNewPerson").button({ icons: { primary: "ui-icon-plusthick"} });

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

	<!-- jquery templates -->
	<script id="t_personTr" type="text/html">
		<tr>
			<td>
				{{if (g_dualService._client._serviceUrl.indexOf("dual_cake") > 0)}}
					RestCake
				{{else}}
					WCF
				{{/if}}
			</td>
			<td>${ID}</td>
			<td>${DateCreated}</td>
			<td>${DateModified}</td>
			<td>${Fname}</td>
			<td>${Lname}</td>
			<td>
				{{each Emails}}
					<div><a href="mailto:${Email}">${Email}</a></div>
				{{/each}}
			</td>
			<td><a href='javascript:void(0)' onclick='loadAddresses(this)'>(load)</a></td>
			<td><a href='javascript:void(0)' onclick='editPerson(this)'>edit</a></td>
			<td><a href='javascript:void(0)' onclick='deletePerson(this)'>delete</a></td>
		</tr>
	</script>
		
	<script id="t_address" type="text/html">
		<div>${Address1}</div>
		{{if Address2}}
			<div>${Address2}</div>
		{{/if}}
		${City}, ${State} &nbsp;${Zip}
		<div style="height: 10px"></div>
	</script>
</asp:Content>

<asp:Content ContentPlaceHolderID="contentExampleTitle" runat="server">Dual Service Example</asp:Content>

<asp:Content ContentPlaceHolderID="contentNotes" runat="server">
	This example shows how easy it is to start using <strong>RestCake</strong> by modifying your existing WCF services to also work through RestCake.
	There is only a single service class at work here (see <span class="code">/Examples/Services/WcfAndRestCakeDualService.cs</span>).  It was a WCF REST
	service to start with.  To add RestCake functionality, we made the class inherit from <span class="code">RestHttpHandler</span> (previously, the class
	had no base class), and then set up a <span class="code">GenericHandlerRoute&lt;T&gt;</span> in the Global.asax in <span class="code">registerRoutes()</span>.

	<br />
	<br />

	The base service url for WCF is <strong><%= HttpRuntime.AppDomainAppVirtualPath %>/services/dual_wcf/</strong>
	<br />
	The base service url for RestCake is <strong><%= HttpRuntime.AppDomainAppVirtualPath %>/services/dual_rest/</strong>
		
	<br />
	<br />

	I recommend using Firefox with <a href="http://getfirebug.com/" target="_blank">Firebug</a> to watch the net traffic (select the Net tab, and filter by XHR).
	You'll be able to easily see the GET, PUT, POST and DELETE async requests, inspect the headers, and look at the responses (both raw and as json objects).

	<br />
	<br />
		
	Notice the difference in the date formats when switching between WCF and RestCake (I don't do any date "fixing up" for WCF here).
	Also, if you look at the service implementation, you'll see that it's necessary to strip out all cycles from the object graph before
	returning the result.  This is because the WCF REST DataContractJsonSerializer cannot handle cycles, and it would cause a runtime exception.
	The default behavior for RestCake (which uses <a href="http://json.codeplex.com" target="_blank">Json.NET</a> for serialization) is to ignore
	cycles, so they would get stripped out automatically, and you wouldn't have to worry about doing this from your service methods.

	<br />
	<br />

	<strong class="red-fg">Note:</strong> If your existing WCF REST service uses <span class="code">WebOperationContext</span>, it will throw a NullReferenceException.
	RestCake currently has no support for this (I don't know how to create a WebOperationContext based on an existing HttpContext).  If your WCF servcie was
	only being used via HTTP in the first place (an no other hosting mechanisms), then you can safely trade out any usage of WebOperationContext.Current with HttpContext.Current.
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentOperations" runat="server">
	<div id="divServiceType">
		<input id="rdoWcf" type="radio" name="grpServiceType" />
		<label for="rdoWcf">WCF</label>

		<input id="rdoRestCake" type="radio" name="grpServiceType" checked="checked" />
		<label for="rdoRestCake">RestCake</label>
	</div>

	<ul>
		<li>
			<a href="javascript:getPeople()">Get all person objects</a>
		</li>

		<li>
			Get person with ID <input id="txtPersonID" type="text" size="2" value="1" />.  <a href="javascript:getPerson()">Do it</a>
		</li>

		<li>
			<a href="javascript:NoError_NoWrappedRequest()">NoError_NoWrappedRequest (single post arg)</a>
		</li>

		<li>
			<a href="javascript:Test_MultiplePost_PlusDto()">Test_MultiplePost_PlusDto</a> (this one fails for WCF because it can't handle a date in ISO format.
			It will only recognize it's own \/Date(12345-000)\/ format.
		</li>
	</ul>
</asp:Content>


<asp:Content ContentPlaceHolderID="contentBottom" runat="server">
	<div class="bar red-bar clear">
		<div class="title">PersonDto Results</div>
		
		<div>
			<a href="javascript:clearPersonTable()">Clear table</a>
		</div>

		<table id="tblPersonResults" class="justrows small" cellpadding="4" style="min-width: 800px">
			<thead class="light-gray-bg">
				<tr>
					<th>Method</th>
					<th>ID</th>
					<th>DateCreated</th>
					<th>DateModified</th>
					<th>Fname</th>
					<th>Lname</th>
					<th>Emails</th>
					<th>Addresses</th>
					<th>&nbsp;</th>
					<th>&nbsp;</th>
				</tr>
			</thead>
			<tbody>
			</tbody>
		</table>
		<a href="javascript:createPerson()">Create new person</a>
		<br />

		<div id="divEditPerson" class="bar red-bar fleft" style="display: none; margin-top: 10px; margin-right: 20px;">
			<div class="title">Edit Person</div>

			<div class="edit">
				<div class="label">ID</div>
				<div id="divPersonID" class="field"></div>
				<div class="clear"></div>

				<div class="label">Fname</div>
				<input id="editPersonFname" type="text" />
				<div class="clear"></div>

				<div class="label">Lname</div>
				<input id="editPersonLname" type="text" />
				<div class="clear"></div>

				<div class="label">Email</div>
				<input id="editPersonEmail" type="text" />

				<div class="clear"></div>

				<a id="lnkSavePersonEdit" onclick="savePersonEdit()">Save</a> or <a href="javascript:cancelPersonEdit()">cancel</a>
			</div>
		</div>

		<div id="divNewPerson" class="bar red-bar fleft" style="display: none; margin-top: 10px;">
			<div class="title">New Person</div>

			<div class="edit">
				<div class="label">Fname</div>
				<input id="createPersonFname" type="text" />
				<div class="clear"></div>

				<div class="label">Lname</div>
				<input id="createPersonLname" type="text" />
				<div class="clear"></div>

				<div class="label">Email</div>
				<input id="createPersonEmail" type="text" />

				<div class="clear"></div>

				<a id="lnkSaveNewPerson" onclick="saveNewPerson()">Create</a> or <a href="javascript:cancelNewPerson()">cancel</a>
			</div>
		</div>
	</div>
</asp:Content>

