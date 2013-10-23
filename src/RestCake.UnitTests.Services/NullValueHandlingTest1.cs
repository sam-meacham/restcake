using Newtonsoft.Json;
using RestCake.AddressBook.DataAccess;

namespace RestCake.UnitTests.Services
{
	/// <summary>
	/// TODO: This is going to be a grind, but I think it would be a good idea to set up unit test services for each of the JsonSerializerSettings properties:
	///		ConstructorHandling
	///		DefaultValueHandling
	///		MissingMemberHandling
	///		NullValueHandling
	///		ObjectCreationHandling
	///		PreserveReferencesHandling
	///		ReferenceLoopHandling
	///		Converters
	/// and create a suite of tests for each.  The tests would try each combination of possible JsonNetSettings:
	///		Set on the service class vs not
	///		Set on a service method vs not
	///		Having the GetSerializerSettings() method overridden vs not
	/// 
	/// Then I'd have to call all the services to make sure the correct behavior is seen in each of them.
	/// 
	/// This is a definite TODO, but like I said, it'll be a grind...not going to get to it right away.
	/// </summary>
	[RestService(Name = "NullValueHandlingTest1", Namespace = "RestCakeUnitTests")]
	[JsonNetSettings(NullValueHandling = NullValueHandling.Ignore)]
	public class NullValueHandlingTest1 : RestHttpHandler
	{
		public override JsonSerializerSettings GetSerializerSettings()
		{
			JsonSerializerSettings settings = base.GetSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Include;
			return settings;
		}

		[Get]
		[JsonNetSettings(NullValueHandling = NullValueHandling.Include)]
		public PersonDto person1()
		{
			PersonDto person = new PersonDto()
			{
				Fname = "Sam",
				Lname = null
			};
			return person;
		}


		[Get]
		[JsonNetSettings(NullValueHandling = NullValueHandling.Ignore)]
		public PersonDto person2()
		{
			PersonDto person = new PersonDto()
			{
				Fname = "Sam",
				Lname = null
			};
			return person;
		}


		[Get]
		public PersonDto person3()
		{
			PersonDto person = new PersonDto()
			{
				Fname = "Sam",
				Lname = null
			};
			return person;
		}

	}
}