using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestCake.Examples.RestServices
{
	/// <summary>
	/// TODO: RestCake gives a terrible error when no route is present...lol
	/// </summary>
	[RestService(Namespace="RestCakeExamples", Route="TestService", JsClientVarName = "RestCakeTestSvc")]
	public class TestService : RestCakeHandler
	{
		[Get]
		public string Hello()
		{
			return "Hello world";
		}

	}
}