<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="InputUnitTests.aspx.cs" Inherits="ExampleServices.Examples.InputUnitTests" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">

<html>
<head>
	<title>RestCake javascript unit tests</title>
	<script type="text/javascript" src="http://code.jquery.com/jquery-latest.js"></script>
	<link rel="Stylesheet" type="text/css" href="../Public/css/qunit.css" />
	<script type="text/javascript" src="../Public/js/qunit.js"></script>

	<!-- the unit test service -->
	<script type="text/javascript" src="http://localhost/RestCake.UnitTests.Services/inputs/_js?jquery&base=true"></script>

	<script type="text/javascript">
		$(function () {
			var svc = new RestCakeUnitTests.InputParamsServiceClient("http://localhost/RestCake.UnitTests.Services/inputs/");

			var inp = {
				strings: ["string one", "string two"],
				ints: [1, 2, 3],
				people: [
					{ ID: 1, Fname: "Jane", Lname: "Doe" },
					{ ID: 2, Fname: "Bob", Lname: "Foo" }
				],
				values: { key1: "val1", key2: "val2" },
				testEnum: 1,
				bools: [true, false],
				bools_ints: [1, 0]
			};

			function person_eq(p1, p2) {
				return p1.ID === p1.ID && p1.Fname === p2.Fname && p1.Lname === p2.Lname;
			}

			function svc_error(restEx) {
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

			// *** GET int ********************************************************************

			asyncTest("GET, int, query string", function () {
				svc.get_int_qstr(inp.ints[0], inp.ints[1],
				// success callback
					function (result) {
						start();
						equals(result, inp.ints[0] + inp.ints[1]);
					},
					svc_error);
			});

			asyncTest("GET, int, uri segments", function () {
				svc.get_int_uriseg(inp.ints[0], inp.ints[1],
				// success callback
					function (result) {
						start();
						equals(result, inp.ints[0] + inp.ints[1]);
					},
					svc_error);
			});

			asyncTest("GET, int[], query string", function () {
				svc.get_intar_qstr(inp.ints, inp.ints,
				// success callback
					function (result) {
						start();
						equals(result.length, 3);
						equals(result[0], inp.ints[0] * 2);
						equals(result[1], inp.ints[1] * 2);
						equals(result[2], inp.ints[2] * 2);
					},
					svc_error);
			});

			asyncTest("GET, int[], uri segments", function () {
				svc.get_intar_uriseg(inp.ints, inp.ints,
				// success callback
					function (result) {
						start();
						equals(result.length, 3);
						equals(result[0], inp.ints[0] * 2);
						equals(result[1], inp.ints[1] * 2);
						equals(result[2], inp.ints[2] * 2);
					},
					svc_error);
			});

			asyncTest("GET, List{int}, query string", function () {
				svc.get_intlist_qstr(inp.ints, inp.ints,
				// success callback
					function (result) {
						start();
						equals(result.length, 3);
						equals(result[0], inp.ints[0] * 2);
						equals(result[1], inp.ints[1] * 2);
						equals(result[2], inp.ints[2] * 2);
					},
					svc_error);
			});

			asyncTest("GET, List{int} uri segments", function () {
				svc.get_intlist_uriseg(inp.ints, inp.ints,
				// success callback
					function (result) {
						start();
						equals(result.length, 3);
						equals(result[0], inp.ints[0] * 2);
						equals(result[1], inp.ints[1] * 2);
						equals(result[2], inp.ints[2] * 2);
					},
					svc_error);
			});

			// *** GET enum *******************************************************************

			asyncTest("GET, enum(str), query string", function () {
				svc.get_enum_qstr("ADD",
				// success callback
					function (result) {
						start();
						equals(result, 1);
					},
					svc_error);
			});

			asyncTest("GET, enum(int), query string", function () {
				svc.get_enum_qstr(1,
				// success callback
					function (result) {
						start();
						equals(result, 1);
					},
					svc_error);
			});

			asyncTest("GET, enum(str), uri segments", function () {
				svc.get_enum_uriseg("ADD",
				// success callback
					function (result) {
						start();
						equals(result, 1);
					},
					svc_error);
			});

			asyncTest("GET, enum(int), uri segments", function () {
				svc.get_enum_uriseg(1,
				// success callback
					function (result) {
						start();
						equals(result, 1);
					},
					svc_error);
			});

			asyncTest("GET, enum[](str), query string", function () {
				svc.get_enumar_qstr(["ADD", "SUBTRACT"],
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], 1);
						equals(result[1], 2);
					},
					svc_error);
			});

			asyncTest("GET, enum[](int), query string", function () {
				svc.get_enumar_qstr([1, 2],
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], 1);
						equals(result[1], 2);
					},
					svc_error);
			});

			asyncTest("GET, enum[](str), uri segments", function () {
				svc.get_enumar_uriseg(["ADD", "SUBTRACT"],
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], 1);
						equals(result[1], 2);
					},
					svc_error);
			});

			asyncTest("GET, enum[](int), uri segments", function () {
				svc.get_enumar_uriseg([1, 2],
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], 1);
						equals(result[1], 2);
					},
					svc_error);
			});

			asyncTest("GET, List{enum}(str), query string", function () {
				svc.get_enumlist_qstr(["ADD", "SUBTRACT"],
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], 1);
						equals(result[1], 2);
					},
					svc_error);
			});

			asyncTest("GET, List{enum}(int), query string", function () {
				svc.get_enumlist_qstr([1, 2],
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], 1);
						equals(result[1], 2);
					},
					svc_error);
			});

			asyncTest("GET, List{enum}(str), uri segments", function () {
				svc.get_enumlist_uriseg(["ADD", "SUBTRACT"],
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], 1);
						equals(result[1], 2);
					},
					svc_error);
			});

			asyncTest("GET, List{enum}(int), uri segments", function () {
				svc.get_enumlist_uriseg([1, 2],
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], 1);
						equals(result[1], 2);
					},
					svc_error);
			});

			// *** GET string *****************************************************************

			asyncTest("GET, string, query string", function () {
				svc.get_string_qstr(inp.strings[0],
				// success callback
					function (result) {
						start();
						equals(result, inp.strings[0]);
					},
					svc_error);
			});

			asyncTest("GET, string, uri segments", function () {
				svc.get_string_uriseg(inp.strings[0],
				// success callback
					function (result) {
						start();
						equals(result, inp.strings[0]);
					},
					svc_error);
			});

			asyncTest("GET, string[], query string", function () {
				svc.get_stringar_qstr(inp.strings,
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], inp.strings[0]);
						equals(result[1], inp.strings[1]);
					},
					svc_error);
			});

			asyncTest("GET, string[], uri segments", function () {
				svc.get_stringar_uriseg(inp.strings,
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], inp.strings[0]);
						equals(result[1], inp.strings[1]);
					},
					svc_error);
			});

			asyncTest("GET, List{string}, query string", function () {
				svc.get_stringlist_qstr(inp.strings,
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], inp.strings[0]);
						equals(result[1], inp.strings[1]);
					},
					svc_error);
			});

			asyncTest("GET, List{string} uri segments", function () {
				svc.get_stringlist_uriseg(inp.strings,
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], inp.strings[0]);
						equals(result[1], inp.strings[1]);
					},
					svc_error);
			});

			// *** GET bool *******************************************************************

			asyncTest("GET, bool, query string", function () {
				svc.get_bool_qstr(inp.bools[0],
				// success callback
					function (result) {
						start();
						equals(result, inp.bools[0]);
					},
					svc_error);
			});

			asyncTest("GET, bool(int), query string", function () {
				svc.get_bool_qstr(inp.bools_ints[0],
				// success callback
					function (result) {
						start();
						equals(result, inp.bools[0]);
					},
					svc_error);
			});

			asyncTest("GET, bool, uri segments", function () {
				svc.get_bool_uriseg(inp.bools[0],
				// success callback
					function (result) {
						start();
						equals(result, inp.bools[0]);
					},
					svc_error);
			});

			asyncTest("GET, bool(int), uri segments", function () {
				svc.get_bool_uriseg(inp.bools_ints[0],
				// success callback
					function (result) {
						start();
						equals(result, inp.bools[0]);
					},
					svc_error);
			});

			asyncTest("GET, bool[], query string", function () {
				svc.get_boolar_qstr(inp.bools,
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], inp.bools[0]);
						equals(result[1], inp.bools[1]);
					},
					svc_error);
			});

			asyncTest("GET, bool[](int), query string", function () {
				svc.get_boolar_qstr(inp.bools_ints,
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], inp.bools[0]);
						equals(result[1], inp.bools[1]);
					},
					svc_error);
			});

			asyncTest("GET, bool[], uri segments", function () {
				svc.get_boolar_uriseg(inp.bools,
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], inp.bools[0]);
						equals(result[1], inp.bools[1]);
					},
					svc_error);
			});

			asyncTest("GET, bool[](int), uri segments", function () {
				svc.get_boolar_uriseg(inp.bools_ints,
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], inp.bools[0]);
						equals(result[1], inp.bools[1]);
					},
					svc_error);
			});

			asyncTest("GET, List{bool}, query string", function () {
				svc.get_boollist_qstr(inp.bools,
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], inp.bools[0]);
						equals(result[1], inp.bools[1]);
					},
					svc_error);
			});

			asyncTest("GET, List{bool}(int), query string", function () {
				svc.get_boollist_qstr(inp.bools_ints,
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], inp.bools[0]);
						equals(result[1], inp.bools[1]);
					},
					svc_error);
			});

			asyncTest("GET, List{bool} uri segments", function () {
				svc.get_boollist_uriseg(inp.bools,
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], inp.bools[0]);
						equals(result[1], inp.bools[1]);
					},
					svc_error);
			});

			asyncTest("GET, List{bool}(int) uri segments", function () {
				svc.get_boollist_uriseg(inp.bools_ints,
				// success callback
					function (result) {
						start();
						equals(result.length, 2);
						equals(result[0], inp.bools[0]);
						equals(result[1], inp.bools[1]);
					},
					svc_error);
			});

			// *** GET Person *****************************************************************
			// (these are disabled for now, because they require passing a json string in the url)
			/*

			asyncTest("GET, Person, query string", function () {
			svc.get_person_qstr(inp.people[0],
			// success callback
			function (result) {
			start();
			ok(person_eq(result, inp.people[0]), "The 2 Person objects are different");
			},
			svc_error);
			});

			asyncTest("GET, Person, uri segments", function () {
			svc.get_person_uriseg(inp.people[0],
			// success callback
			function (result) {
			start();
			ok(person_eq(result, inp.people[0]), "The 2 Person objects are different");
			},
			svc_error);
			});

			asyncTest("GET, Person[], query string", function () {
			svc.get_personar_qstr(inp.people,
			// success callback
			function (result) {
			start();
			equals(result.length, 2);
			ok(person_eq(result[0], inp.people[0]), "The 2 Person objects are different");
			ok(person_eq(result[1], inp.people[1]), "The 2 Person objects are different");
			},
			svc_error);
			});

			asyncTest("GET, Person[], uri segments", function () {
			svc.get_personar_uriseg(inp.people,
			// success callback
			function (result) {
			start();
			equals(result.length, 2);
			ok(person_eq(result[0], inp.people[0]), "The 2 Person objects are different");
			ok(person_eq(result[1], inp.people[1]), "The 2 Person objects are different");
			},
			svc_error);
			});

			asyncTest("GET, List{Person}, query string", function () {
			svc.get_personlist_qstr(inp.people,
			// success callback
			function (result) {
			start();
			equals(result.length, 2);
			ok(person_eq(result[0], inp.people[0]), "The 2 Person objects are different");
			ok(person_eq(result[1], inp.people[1]), "The 2 Person objects are different");
			},
			svc_error);
			});

			asyncTest("GET, List{Person} uri segments", function () {
			svc.get_personlist_uriseg(inp.people,
			// success callback
			function (result) {
			start();
			equals(result.length, 2);
			ok(person_eq(result[0], inp.people[0]), "The 2 Person objects are different");
			ok(person_eq(result[1], inp.people[1]), "The 2 Person objects are different");
			},
			svc_error);
			});
			*/

			// *** POST int *******************************************************************

			asyncTest("POST, int, query string", function () {
				svc.post_int_qstr(inp.ints[0], inp.ints[1],
				// success callback
					function (result) {
						start();
						equals(result, inp.ints[0] + inp.ints[1]);
					},
					svc_error);
			});


			// *** POST enum ******************************************************************

			// *** POST string ****************************************************************

			// *** POST bool ******************************************************************

			// *** POST Person ****************************************************************


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
