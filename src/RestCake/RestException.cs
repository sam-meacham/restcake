using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace RestCake
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class RestException : Exception
	{
		/// <summary>
		/// This is only here so that it can be easily checked for in javascript, to easily identify an object as an error.
		/// Ex: if (result.IsRestException) { /* this must be a RestException object */ }
		/// </summary>
		[JsonProperty] public bool IsRestException { get; private set; }

		/// <summary>
		/// This can have any string value, to help determine the type of error on the client side.
		/// It's just a flag value.  If an InnerException is passed to the constructor, it's type name will be used.
		/// Notice the setter is public, so that value can be overridden in a subsequent assignment or object initializer.
		/// </summary>
		[JsonProperty] public string ErrorType { get; set; }

		/// <summary>
		/// This will have the complete stack trace of the current exception and all inner exceptions, and their Message values.
		/// </summary>
		[JsonProperty] public string CompleteStackTrace { get; private set; }

		[JsonProperty] public string ExtraInfo { get; private set; }

		// Overridden so I can tag it with the JsonProperty attribute
		[JsonProperty]
		public override string Message { get { return base.Message; } }

		// Overridden so I can tag it with the JsonProperty attribute
		[JsonProperty]
		public override System.Collections.IDictionary Data { get { return base.Data; } }

		/// <summary>
		/// If not set, defaults to 500 (Internal Server Error).  Whatever the value when this exception is thrown,
		/// that will be the response code sent back to the client.
		/// This is a code contribution from "thecutter" (Gerrit) on CodePlex (http://rest.codeplex.com/Thread/View.aspx?ThreadId=234885)
		/// </summary>
		[JsonIgnore]
		public HttpStatusCode ResponseStatusCode { get; set; }


		// ********************************************************************************
		// ** Constructors ****************************************************************
		// ********************************************************************************

		// Simple constructors
		public RestException()
			: this(HttpStatusCode.InternalServerError, "Unknown RestException error", null, null)
		{ }

		public RestException(string message)
			: this(HttpStatusCode.InternalServerError, message, null, null)
		{ }

		public RestException(HttpStatusCode responseStatusCode, string message)
			: this(responseStatusCode, message, null, null)
		{ }

		public RestException(string message, Exception innerException)
			: this(HttpStatusCode.InternalServerError, message, innerException, null)
		{ }

		public RestException(HttpStatusCode responseStatusCode, string message, Exception innerException)
			: this(responseStatusCode, message, innerException, null)
		{ }


		//
		// Now the main constructor, that all of the above constructors call.  This one calls the base Exception constructor.
		//
		public RestException(HttpStatusCode responseStatusCode, string message, Exception innerException, string extraInfo)
			: base(message, innerException)
		{
			ResponseStatusCode = responseStatusCode;
			IsRestException = true;
			ErrorType = innerException == null ? null : innerException.GetType().Name;
			CompleteStackTrace = getCompleteStackTrace(innerException);
			ExtraInfo = extraInfo;
		}


		private static string getCompleteStackTrace(Exception ex)
		{
			if (ex == null)
				return null;

			StringBuilder sb = new StringBuilder();
			while (ex != null)
			{
				sb.AppendLine("Type: " + ex.GetType().Name);
				sb.AppendLine("Message: " + ex.Message);
				sb.AppendLine("Stack Trace:");
				sb.AppendLine(ex.StackTrace).AppendLine();
				ex = ex.InnerException;
				if (ex != null)
					sb.AppendLine("Inner exception:");
			}
			return sb.ToString();
		}

	}
}
