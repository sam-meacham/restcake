// RestClient.cs template for RestCakeExamples.BodyStyle
namespace RestCakeExamples
{
	using RestSharp;

	public class BodyStyleClient : RestCake.RestSharpClientBase
	{
		public BodyStyleClient(string baseUrl)
			: base(baseUrl)
		{ }

		public RestCake.AddressBook.DataAccess.PersonDto WrappedTest(System.Int32 id)
		{
			RestCake.AddressBook.DataAccess.PersonDto result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.PersonDto>("wrapped", true, true, Method.POST, new Parameter[] {
				new Parameter() { Name = "id", Type = ParameterType.GetOrPost, Value = id }
			});
			return result;
		}

		public RestCake.AddressBook.DataAccess.PersonDto WrappedRequestTest(System.Int32 id)
		{
			RestCake.AddressBook.DataAccess.PersonDto result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.PersonDto>("wrappedRequest", false, false, Method.POST, new Parameter[] {
				new Parameter() { Name = "id", Type = ParameterType.GetOrPost, Value = id }
			});
			return result;
		}

		public RestCake.AddressBook.DataAccess.PersonDto WrappedResponseTest(System.Int32 id)
		{
			RestCake.AddressBook.DataAccess.PersonDto result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.PersonDto>("wrappedResponse", true, true, Method.POST, new Parameter[] {
				new Parameter() { Name = "id", Type = ParameterType.GetOrPost, Value = id }
			});
			return result;
		}

		public RestCake.AddressBook.DataAccess.PersonDto BareTest(System.Int32 id)
		{
			RestCake.AddressBook.DataAccess.PersonDto result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.PersonDto>("bare", false, false, Method.POST, new Parameter[] {
				new Parameter() { Name = "id", Type = ParameterType.GetOrPost, Value = id }
			});
			return result;
		}


	}
}

// RestClient.cs template for RestCakeExamples.AddressBook
namespace RestCakeExamples
{
	using RestSharp;

	public class AddressBookClient : RestCake.RestSharpClientBase
	{
		public AddressBookClient(string baseUrl)
			: base(baseUrl)
		{ }

		public System.Int32 test1()
		{
			System.Int32 result = InvokeServiceMethod<System.Int32>("test1", false, false, Method.GET, new Parameter[] {
				
			});
			return result;
		}

		public RestCake.AddressBook.DataAccess.PersonDto[] GetPeople()
		{
			RestCake.AddressBook.DataAccess.PersonDto[] result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.PersonDto[]>("GetPeople", false, false, Method.GET, new Parameter[] {
				
			});
			return result;
		}

		public RestCake.AddressBook.DataAccess.GroupDto[] GetGroups()
		{
			RestCake.AddressBook.DataAccess.GroupDto[] result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.GroupDto[]>("GetGroups", false, false, Method.GET, new Parameter[] {
				
			});
			return result;
		}

		public RestCake.AddressBook.DataAccess.EmailTypeDto[] GetEmailTypes()
		{
			RestCake.AddressBook.DataAccess.EmailTypeDto[] result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.EmailTypeDto[]>("GetEmailTypes", false, false, Method.GET, new Parameter[] {
				
			});
			return result;
		}

		public RestCake.AddressBook.DataAccess.PhoneTypeDto[] GetPhoneTypes()
		{
			RestCake.AddressBook.DataAccess.PhoneTypeDto[] result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.PhoneTypeDto[]>("GetPhoneTypes", false, false, Method.GET, new Parameter[] {
				
			});
			return result;
		}

		public RestCake.AddressBook.DataAccess.PersonDto UpdatePerson(System.Int32 id, System.Collections.Generic.Dictionary<System.String,System.Object> values)
		{
			RestCake.AddressBook.DataAccess.PersonDto result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.PersonDto>("" + id + "", false, false, Method.PUT, new Parameter[] {
				new Parameter() { Name = "values", Type = ParameterType.GetOrPost, Value = values }
			});
			return result;
		}


	}
}

// RestClient.cs template for RestCakeExamples.DualService
namespace RestCakeExamples
{
	using RestSharp;

	public class DualServiceClient : RestCake.RestSharpClientBase
	{
		public DualServiceClient(string baseUrl)
			: base(baseUrl)
		{ }

		public RestCake.AddressBook.DataAccess.PersonDto[] GetPeople()
		{
			RestCake.AddressBook.DataAccess.PersonDto[] result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.PersonDto[]>("people", false, false, Method.GET, new Parameter[] {
				
			});
			return result;
		}

		public RestCake.AddressBook.DataAccess.PersonDto GetPerson(System.String sID)
		{
			RestCake.AddressBook.DataAccess.PersonDto result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.PersonDto>("person/" + sID + "", false, false, Method.GET, new Parameter[] {
				
			});
			return result;
		}

		public RestCake.AddressBook.DataAccess.AddressDto[] GetAddresses(System.String sID)
		{
			RestCake.AddressBook.DataAccess.AddressDto[] result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.AddressDto[]>("person/" + sID + "/addresses", false, false, Method.GET, new Parameter[] {
				
			});
			return result;
		}

		public RestCake.AddressBook.DataAccess.PersonDto CreatePerson(RestCake.AddressBook.DataAccess.PersonDto person)
		{
			RestCake.AddressBook.DataAccess.PersonDto result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.PersonDto>("person", false, false, Method.POST, new Parameter[] {
				new Parameter() { Name = "person", Type = ParameterType.GetOrPost, Value = person }
			});
			return result;
		}

		public RestCake.AddressBook.DataAccess.PersonDto UpdatePerson(RestCake.AddressBook.DataAccess.PersonDto person)
		{
			RestCake.AddressBook.DataAccess.PersonDto result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.PersonDto>("person", false, false, Method.PUT, new Parameter[] {
				new Parameter() { Name = "person", Type = ParameterType.GetOrPost, Value = person }
			});
			return result;
		}

		public RestCake.AddressBook.DataAccess.PersonDto UpdatePerson2(RestCake.AddressBook.DataAccess.PersonDto person)
		{
			RestCake.AddressBook.DataAccess.PersonDto result = InvokeServiceMethod<RestCake.AddressBook.DataAccess.PersonDto>("person2", false, false, Method.PUT, new Parameter[] {
				new Parameter() { Name = "person", Type = ParameterType.GetOrPost, Value = person }
			});
			return result;
		}

		public void DeletePerson(System.String sID)
		{
			InvokeServiceMethod<object>("person/" + sID + "", false, false, Method.DELETE, new Parameter[] {
				
			});
		}

		public System.String NoError_NoWrappedRequest(System.String dummyUriArg1, System.String dummyUriArg2, System.String qarg1, System.String qarg2, System.String postArg1)
		{
			System.String result = InvokeServiceMethod<System.String>("error/needsWrappedRequest/" + dummyUriArg1 + "/" + dummyUriArg2 + "?qarg1=" + qarg1 + "&" + "qarg2=" + qarg2 + "", false, false, Method.POST, new Parameter[] {
				new Parameter() { Name = "postArg1", Type = ParameterType.GetOrPost, Value = postArg1 }
			});
			return result;
		}

		public System.Object Test_MultiplePost_PlusDto(System.String dummyUriArg1, System.String dummyUriArg2, System.String qarg1, System.String qarg2, System.String postArg1, System.Int32 postArg2, System.DateTime postArg3, RestCake.AddressBook.DataAccess.PersonDto person)
		{
			System.Object result = InvokeServiceMethod<System.Object>("error/test_multiplePost_plusDto/" + dummyUriArg1 + "/" + dummyUriArg2 + "?qarg1=" + qarg1 + "&" + "qarg2=" + qarg2 + "", false, false, Method.POST, new Parameter[] {
				new Parameter() { Name = "postArg1", Type = ParameterType.GetOrPost, Value = postArg1 }, new Parameter() { Name = "postArg2", Type = ParameterType.GetOrPost, Value = postArg2 }, new Parameter() { Name = "postArg3", Type = ParameterType.GetOrPost, Value = postArg3 }, new Parameter() { Name = "person", Type = ParameterType.GetOrPost, Value = person }
			});
			return result;
		}


	}
}

// RestClient.cs template for RestCakeExamples.MathService
namespace RestCakeExamples
{
	using RestSharp;

	public class MathServiceClient : RestCake.RestSharpClientBase
	{
		public MathServiceClient(string baseUrl)
			: base(baseUrl)
		{ }

		public System.Double divide(System.Double a, System.Double b)
		{
			System.Double result = InvokeServiceMethod<System.Double>("divide/" + a + "/" + b + "", false, false, Method.GET, new Parameter[] {
				
			});
			return result;
		}

		public System.Int64 val_fail()
		{
			System.Int64 result = InvokeServiceMethod<System.Int64>("val_fail", false, false, Method.GET, new Parameter[] {
				
			});
			return result;
		}


	}
}

namespace RestCake
{
	using System;
	using System.IO;
	using System.Text;
	using Newtonsoft.Json;
	using RestSharp;


	public abstract class RestSharpClientBase
	{
		// Base url of the service.  Immutable.
		private readonly string m_baseUrl;
		public string BaseUrl { get { return m_baseUrl; } }

		// The RestSharp client.  Immutable.
		private readonly RestClient m_client;

		public RestRequest LastRequest { get; private set; }
		public RestResponse LastResponse { get; private set; }



		protected RestSharpClientBase(string baseUrl)
		{
			if (string.IsNullOrEmpty(baseUrl))
				throw new ArgumentNullException("baseUrl", "baseUrl cannot be null or empty");

			if (!baseUrl.Trim().EndsWith("/"))
				baseUrl += "/";
			m_baseUrl = baseUrl;

			m_client = new RestClient(m_baseUrl);
		}


		protected T InvokeServiceMethod<T>(string methodUrl, bool wrappedRequest, bool wrappedResponse, Method httpVerb, Parameter[] parameters)
		{
			if (parameters == null)
				parameters = new Parameter[0];

			RestRequest request = new RestRequest(methodUrl, httpVerb) { RequestFormat = DataFormat.Json };

			StringBuilder sbQueryString = new StringBuilder();

			StringBuilder sbRequestBody = new StringBuilder();
			JsonTextWriter jwriter = new JsonTextWriter(new StringWriter(sbRequestBody));

			/*
			if (wrappedRequest)
				jwriter.WriteStartObject();
			*/

			foreach (Parameter param in parameters)
			{
				if (param.Type == ParameterType.RequestBody)
				{
					request.AddBody(param.Value);
				}
				else
				{
					request.AddParameter(param);
				}

				/*
				switch (param.Type)
				{
					case ParameterType.UrlSegment:
						request.AddParameter(param.Name, param.Value, ParameterType.UrlSegment);
						break;

					case ParameterType.GetOrPost:
						if (httpVerb == Method.GET)
						{
							sbQueryString.AppendFormat("&{0}={1}", HttpUtility.UrlEncode(param.Name), HttpUtility.UrlEncode(param.Value.ToString()));
						}
						else if (wrappedRequest)
						{
							Type paramType = param.Value.GetType();

							jwriter.WritePropertyName(param.Name);

							if (paramType.IsPrimitive || paramType.Module.Name == "mscorlib.dll")
								jwriter.WriteValue(param.Value);
							else
								jwriter.WriteRaw(JsonConvert.SerializeObject(param.Value));
						}
						else
						{
							// TODO: Make sure this only happens once.  Otherwise, they have multiple params with a bare request (not allowed).
							jwriter.WriteValue(param.Value);
						}
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}
				*/
			}

			/*
			if (wrappedRequest)
				jwriter.WriteEndObject();
			*/

			LastRequest = request;
			RestResponse response = m_client.Execute(request);

			LastResponse = response;

			T result = JsonConvert.DeserializeObject<T>(response.Content);
			return result;
		}

	}
}

