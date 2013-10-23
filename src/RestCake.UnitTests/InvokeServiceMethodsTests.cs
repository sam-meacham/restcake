using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using RestCake.AddressBook.DataAccess;
using RestSharp;


namespace RestCake.UnitTests
{
	/// <summary>
	/// I haven't made any progress yet on unit tests.  The current tests don't pass, it's just a sandbox where I try a few things out
	/// here and there.
	/// </summary>
	[TestFixture]
	public class InvokeServiceMethodsTests
	{
		private RestClient m_client;


		[SetUp]
		public void Setup()
		{
			m_client = new RestClient("http://localhost/AddressBook.Services/contacts/");
		}


		[TearDown]
		public void TearDown()
		{
		}


		public void TempTest()
		{
			RestRequest req = new RestRequest("GetPeople");
			RestResponse response = m_client.Execute(req);
			PersonDto[] people = JsonConvert.DeserializeObject<PersonDto[]>(response.Content);

			foreach (PersonDto person in people)
				Console.WriteLine(person.Fname + " " + person.Lname);
		}


		[Test]
		public void DeserializeInterfaceTypeTest()
		{
			const int PERSON_ID = 10;

			RestRequest req = new RestRequest(PERSON_ID.ToString(), Method.PUT);
			var updatedValues = new Dictionary<string, object> {
				{"Fname", "asdf"},
				{"Lname", "jjjj"}
			};
			req.AddParameter("id", PERSON_ID, ParameterType.UrlSegment);
			req.AddParameter("values", updatedValues, ParameterType.GetOrPost);

			RestResponse<PersonDto> response = m_client.Execute<PersonDto>(req);

			Console.WriteLine(response.Content);

			PersonDto person = response.Data;

			Console.WriteLine(person.Fname + " " + person.Lname);
		}


		[Test]
		public void RestSharpTest()
		{
			RestClient client = new RestClient("http://localhost/AddressBook.Services/contacts/");

			RestRequest req = new RestRequest("GetPeople", Method.GET);

			RestResponse response = client.Execute(req);

			PersonDto[] people = JsonConvert.DeserializeObject<PersonDto[]>(response.Content);

			foreach (PersonDto person in people)
			{
				Console.WriteLine(person.Fname + " " + person.Lname);
			}

			var newValues = new Dictionary<string, object>()
			                	{
									{"Fname", "John"},
									{"Lname", "Doe"}
			                	};

			const int PERSON_ID = 3;
			Console.WriteLine();
			Console.WriteLine("Before update:");
			PersonDto p1 = people.Where(p => p.ID == PERSON_ID).SingleOrDefault();
			Console.WriteLine(p1.Fname + " " + p1.Lname);

			RestRequest reqUpdatePerson = new RestRequest("{id}", Method.PUT) { RequestFormat = DataFormat.Json };

			reqUpdatePerson.AddParameter("id", PERSON_ID, ParameterType.UrlSegment);
			//reqUpdatePerson.AddParameter("values", newValues, ParameterType.GetOrPost);

			reqUpdatePerson.AddParameter("values", JsonConvert.SerializeObject(newValues), ParameterType.GetOrPost);

			reqUpdatePerson.AddHeader("Content-Type", "application/json");


			RestResponse<PersonDto> updateResponse = client.Execute<PersonDto>(reqUpdatePerson);

			Console.WriteLine();
			Console.WriteLine("Response:");
			Console.WriteLine(updateResponse.Content);

			Console.WriteLine();
			Console.WriteLine("After update:");
			PersonDto p2 = updateResponse.Data;
			Console.WriteLine(p2.Fname + " " + p2.Lname);

			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Press any key to exit");
			Console.ReadKey();
		}
	}
}
