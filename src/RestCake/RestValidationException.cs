using System;
using System.Net;
using Newtonsoft.Json;


namespace RestCake
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class RestValidationException : RestException
	{
		private bool m_isRestValidationException = true;

		public RestValidationException(string message)
			: base(message)
		{ }

		public RestValidationException(HttpStatusCode responseStatusCode, string message)
			: base (responseStatusCode, message)
		{ }

		public RestValidationException(string message, Exception innerException)
			: base(message, innerException)
		{ }

		public RestValidationException(HttpStatusCode responseStatusCode, string message, Exception innerException)
			: base(responseStatusCode, message, innerException)
		{ }


		/// <summary>
		/// This is only here so that it can be easily checked for in javascript, to easily identify an object as an error.
		/// Ex: if (result.IsRestValidationException) { /* this must be a RestValidationException object */ }
		/// </summary>
		[JsonProperty]
		public bool IsRestValidationException
		{
			get { return m_isRestValidationException; }
		}
	}
}
