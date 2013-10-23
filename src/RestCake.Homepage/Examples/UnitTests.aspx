<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="UnitTests.aspx.cs" Inherits="ExampleServices.Examples.UnitTests" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">

<html>
<head>
	<title>RestCake javascript unit tests</title>
	<script type="text/javascript" src="http://code.jquery.com/jquery-latest.js"></script>
	<link rel="Stylesheet" type="text/css" href="../Public/css/qunit.css" />
	<script type="text/javascript" src="../Public/js/qunit.js"></script>

	<!-- the unit test service -->
	<script type="text/javascript" src="http://localhost/AddressBook.Services/unittest/_js?jquery&base=true"></script>

	<script type="text/javascript">
		$(function ()
		{
			var svc = new RestCakeServices.UnitTestServiceClient("http://localhost/AddressBook.Services/unittest/");
			var inp = {
				str: "a string",
				i: 10,
				arStrings: ["string one", "string two"],
				ints: [1, 2, 3],
				list: ["a", "b", "c"],
				person: { Fname: "Jane", Lname: "Doe" },
				values: { key1: "val1", key2: "val2" },
				testEnum: 1
			};

			function validate_response(result)
			{
				equals(inp.str, result.str);
				equals(inp.i, result.i);
				same(inp.arStrings, result.arStrings);
				same(inp.ints, result.ints);
				same(inp.list, result.list);
				equals(inp.person.Fname, result.person.Fname);
				equals(inp.person.Lname, result.person.Lname);
				same(inp.values, result.values);
				equals(inp.testEnum, result.testEnum);
			}

			function svc_error(restEx)
			{
				var msg = "";
				if (typeof (restEx) !== "undefined" && restEx != null && typeof (restEx.Message) !== "undefined")
					msg = restEx.Message;
				ok(false, "Service call failed. " + msg);
				start();
			}

			//
			// GET Tests
			//

			module("GET Tests");

			asyncTest("test_get_querystring", function ()
			{
				svc.test_get_querystring(inp.str, inp.i, inp.arStrings, inp.ints, inp.list, inp.person, inp.values, inp.testEnum,
				// success callback
					function (result)
					{
						start();
						validate_response(result);
					},
					svc_error);
			});


			asyncTest("test_get_uri", function ()
			{
				svc.test_get_uri(inp.str, inp.i, inp.arStrings, inp.ints, inp.list, inp.person, inp.values, inp.testEnum,
				// success callback
					function (result)
					{
						start();
						validate_response(result);
					},
					svc_error);
			});


			//
			// PUT Tests
			//

			module("PUT tests");

			asyncTest("test_put_querystring", function ()
			{
				svc.test_put_querystring(inp.str, inp.i, inp.arStrings, inp.ints, inp.list, inp.person, inp.values, inp.testEnum,
				// success callback
					function (result)
					{
						start();
						validate_response(result);
					},
					svc_error);
			});

			asyncTest("test_put_requestbody", function ()
			{
				svc.test_put_requestbody(inp.str, inp.i, inp.arStrings, inp.ints, inp.list, inp.person, inp.values, inp.testEnum,
				// success callback
					function (result)
					{
						start();
						validate_response(result);
					},
					svc_error);
			});

			//
			// Auth test
			//

			module("Auth tests");

			asyncTest("test_func_auth_attribute", function ()
			{
				svc.test_func_auth_attribute("123",
				// success callback
					function (result)
					{
						start();
						equals(result, "Hello World!");
					}
					,
					svc_error);
			});

		});
	</script>
</head>

<body>
	<h1 id="qunit-header">RestCake Service Unit Tests</h1>
	<h2 id="qunit-banner"></h2>
	<div id="qunit-testrunner-toolbar"></div>
	<h2 id="qunit-userAgent"></h2>
	<ol id="qunit-tests"></ol>
	<div id="qunit-fixture">test markup, will be hidden</div>
</body>
</html>
