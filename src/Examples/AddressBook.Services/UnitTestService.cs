using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestCake.AddressBook.DataAccess;
using RestCake.AddressBook.Dtos;


namespace RestCake.AddressBook.Services
{
	[RestService(Namespace="RestCakeServices")]
	public class UnitTestService : RestHttpHandler
	{
		/*
		 * test every verb (GET, PUT, POST, DELETE)
		 * 
		 * test each request BodyStyle (Bare, Wrapped)
		 * 
		 * test every method of passing in params
		 * (uri template param, query string param, request body, request header)
		 * 
		 * test all param types (simple scalars, arrays, generics, enums, user types)
		 * 
		 * should probably test all param types for return values as well
		 * 
		 * for all combinations of param types and all methods of passing in params, test what happens when null or an empty string is passed.
		 * Also test what happens if the param is absent for each param-passing-method.
		 * 
		 * TODO: Add a date param to the services, since that's a special data type and we need to make sure it works in all scenarios.
		 * 
		 */

		//
		// GET methods
		//

		[Get(UrlStyle = UrlStyle.QueryString)]
		public object testNullAndEmptyArrays(string[] ar1, string[] ar2)
		{
			string result1;
			if (ar1 == null)
				result1 = "ar1 is null";
			else
				result1 = String.Join(",", ar1);


			string result2;
			if (ar2 == null)
				result2 = "ar2 is null";
			else
				result2 = String.Join(",", ar2);

			return new
			{
				result1,
				result2
			};
		}

		// GET: All params are passed in the query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public object test_get_querystring(string str, int i, string[] arStrings, int[] ints, List<string> list, PersonDto person, Dictionary<string, object> values, TestEnum testEnum)
		{
			return new {str, i, arStrings, ints, list, person, values, testEnum};
		}

		// GET: All params are passed as UriSegment parameters.
		// This only works with some edits to the web config that allow any character to be part of the uri.
		// Normally IIS would return an error because of illegal characters (:, etc).
		// I have that disabled in this project, because I want to make sure the uri segments work even for weird cases like this.
		// You never know what someone's going to try.  I want it to work when they try it.
		// If the machine this is running on has something like UrlScan installed, this will fail.
		[Get(UrlStyle = UrlStyle.UriSegments)]
		public object test_get_uri(string str, int i, string[] arStrings, int[] ints, List<string> list, PersonDto person, Dictionary<string, object> values, TestEnum testEnum)
		{
			return new {str, i, arStrings, ints, list, person, values, testEnum};
		}


		// PUT: All params are passed in the query string
		// TODO: BUG: By specifying UrlStyle.QueryString, all params should default to that, but for PUT, they aren't.  They are still being treated as RequestBody params.
		// TODO: Once the bug is fixed, get rid of the BodyStyle.WrappedRequest, since it shouldn't be required (since there will be no request body params at all)
		// The JsClientWriter is the problem.  It's not treating the params as url params (either query string or uri template params).  It seems to think they should all be data params (request body).
		[Put(UrlStyle = UrlStyle.QueryString, BodyStyle = BodyStyle.WrappedRequest)]
		public object test_put_querystring(string str, int i, string[] arStrings, int[] ints, List<string> list, PersonDto person, Dictionary<string, object> values, TestEnum testEnum)
		{
			return new {str, i, arStrings, ints, list, person, values, testEnum};
		}

		// PUT: All params are passed as part of the request body (wrapped request)
		[Put(UriTemplate="test_put_requestbody", BodyStyle = BodyStyle.WrappedRequest)]
		public object test_put_requestbody(string str, int i, string[] arStrings, int[] ints, List<string> list, PersonDto person, Dictionary<string, object> values, TestEnum testEnum)
		{
			return new {str, i, arStrings, ints, list, person, values, testEnum};
		}


		[Get(UriTemplate = "test_func_auth_attribute?apikey={apikey}")]
		[Authorize(Method = "CustomAuthCheck")]
		public string test_func_auth_attribute(string apikey)
		{
			return "Hello World!";
		}


		public bool CustomAuthCheck()
		{
			string[] VALID_KEYS = new [] {"123", "456"};
			string apikey = Request.QueryString["apikey"];
			return VALID_KEYS.Contains(apikey);
		}

	}
}
