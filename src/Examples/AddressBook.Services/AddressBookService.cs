using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using System.Web;
using RestCake.AddressBook.DataAccess;
using Group = RestCake.AddressBook.DataAccess.Group;


namespace RestCake.AddressBook.Services
{
	[ServiceContract(Namespace="RestCakeExamples", Name="AddressBook")]
	public class AddressBookService : RestHttpHandler
	{
		// Set up any extra regex overrides in the service class's static constructor
		static AddressBookService()
		{
			AddRegexOverride(typeof(AddressBookService), new Regex("/hello"), sayHello);
		}

		// This is the method that will be used if the incoming requests's url matches the regex
		private static void sayHello(Match match, Type type, string everythingAfterRouteUrl, HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			context.Response.Write(
				"Hello World!"
				+ Environment.NewLine
				+ everythingAfterRouteUrl
			);
		}


		[WebGet]
		public static int test1()
		{
			throw new Exception("This was thrown on purpose");
			return 37;
		}


		public override bool IsReusable
		{
			get { return true; }
		}


		[WebGet]
		public static PersonDto[] GetPeople()
		{
			List<Person> people = AddressBookDal.Instance.People
				.EagerLoad(p => p.Addresses)
				.OrderBy(p => p.Lname).ToList();
			return people.ToDtos();
		}


		[WebGet]
		public static GroupDto[] GetGroups()
		{
			List<Group> groups = AddressBookDal.Instance.Groups.OrderBy(g => g.Name).ToList();
			return groups.ToDtos();
		}


		[WebGet]
		public static EmailTypeDto[] GetEmailTypes()
		{
			List<EmailType> types = AddressBookDal.Instance.EmailTypes.OrderBy(t => t.Description).ToList();
			return types.ToDtos();
		}


		[WebGet]
		public static PhoneTypeDto[] GetPhoneTypes()
		{
			List<PhoneType> types = AddressBookDal.Instance.PhoneTypes.OrderBy(t => t.Description).ToList();
			return types.ToDtos();
		}


		[WebInvoke(Method="PUT", UriTemplate = "{id}")]
		public static PersonDto UpdatePerson(int id, Dictionary<string, object> values)
		{
			Person person = Person.GetByKey(id);

			if (person == null)
				throw new ArgumentException("Person with id " + id + " not found");

			string[] permittedFields = {"Fname", "Lname"};
			person.ApplyValues(values.Where(pair => permittedFields.Contains(pair.Key)));

			AddressBookDal.Instance.SaveChanges();

			return person.ToDto();
		}

	}
}
