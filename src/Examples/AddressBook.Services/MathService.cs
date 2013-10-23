using System;
using System.Collections.Generic;

namespace RestCake.AddressBook.Services
{
	[RestService(Namespace = "RestCakeExamples", Name = "MathService")]
	public class MathService : RestHttpHandler
	{
		public override bool IsReusable
		{
			get { return true; }
		}

		public override string AdditionalHelpPageContent()
		{
			return "<br /><h2>Custom Help Page Content</h2>This is an example of extra content on the help page...<br /><br />"
				+ EverythingAfterRouteUrl;
		}



		/// <summary>
		/// This uses the default UrlStyle
		/// </summary>
		[Get]
		public double divide(double a, double b)
		{
			if (b == 0)
				throw new DivideByZeroException("The denominator must be non-zero");
			return a/b;
		}

		/// <summary>
		/// This is exactly the same as divide, but explicitly uses UrlStyle.UriSegments.
		/// </summary>
		[Get(UrlStyle = UrlStyle.UriSegments)]
		public double divide_uriseg(double a, double b)
		{
			if (b == 0)
				throw new DivideByZeroException("The denominator must be non-zero");
			return a / b;
		}

		/// <summary>
		/// This is exactly the same as divide, but explicitly uses UrlStyle.QueryString.
		/// </summary>
		[Get(UrlStyle = UrlStyle.QueryString)]
		public double divide_qstr(double a, double b)
		{
			if (b == 0)
				throw new DivideByZeroException("The denominator must be non-zero");
			return a / b;
		}



		[Get]
		public static long val_fail()
		{
			throw new RestValidationException("This is a validation exception, thrown on purpose");
		}

	}
}