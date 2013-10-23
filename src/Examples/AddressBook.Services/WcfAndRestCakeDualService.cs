using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using RestCake.AddressBook.DataAccess;

namespace RestCake.AddressBook.Services
{
	/// <summary>
	/// Remember that RestCake uses the same attributes as WCF for marking service classes and service methods (ServiceContract and WebGet/WebInvoke).
	/// For a service class to be accessible via both WCF and RestCake, you can't take advantage of all of the RestCake features, such as your service methods
	/// taking in non-string arguments.
	/// Look in Global.asax, in registerRoutes() to see how both the WCF and RestCake endpoints (routes) are set up for this class.
	/// </summary>
	[ServiceContract(Namespace = "RestCakeExamples", Name = "DualService")]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class WcfAndRestCakeDualService : RestHttpHandler
	{
		/// <summary>
		/// Since this class has no internal state, it can be reused.
		/// This is a RestCake thing, not a WCF thing.  WCF's way is the ServiceBehavior attribute on the class.
		/// </summary>
		public override bool IsReusable
		{
			get { return true; }
		}


		[WebGet(UriTemplate="people")]
		public PersonDto[] GetPeople()
		{
			// Note that we only have to strip the cycles because of WCF.  It would not be required for RestCake, which uses Json.NET
			return AddressBookDal.Instance.People.EagerLoad(p => p.Emails).ToDtos().StripCycles().ToArray();
		}


		/// <summary>
		/// Fetch an existing person record by id.
		/// Note that WCF can't pass an int into the service method, since the argument comes from the UriTemplate.
		/// It can do that with query string params, however.  If this service were just RestCake, we could just pass an int
		/// right in, instead of having to TryParse it.  We'll see this in other examples.
		/// </summary>
		/// <param name="sID"></param>
		/// <returns></returns>
		[WebGet(UriTemplate = "person/{sID}")]
		public PersonDto GetPerson(string sID)
		{
			int id;
			int.TryParse(sID, out id);
			if (id <= 0)
				throw new ArgumentException("id must be a valid positive integer");

			Person person = AddressBookDal.Instance.People
				.EagerLoad(p => p.Emails)
				.Where(p => p.ID == id)
				.SingleOrDefault();

			return (person == null) ? null : person.ToDto();
		}


		/// <summary>
		/// Create a new person record.  ID is assigned server side, and the new person record is sent back to the caller
		/// </summary>
		/// <param name="person"></param>
		/// <returns></returns>
		[WebInvoke(Method = "POST", UriTemplate = "person")]
		public PersonDto CreatePerson(PersonDto person)
		{
			// When creating a new person, we don't accept an ID value from the DTO passed in.  It's assigned here, as if it were an IDENTITY field in SQL server.
			Person newPerson = new Person()
			{
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				Fname = person.Fname,
				Lname = person.Lname,
				Emails = new EntityCollection<EmailAddress>()
			};

			if (person.Emails != null)
				person.Emails.ForEach(email => newPerson.Emails.Add(email.ToEntity()));

			AddressBookDal.Instance.People.AddObject(newPerson);
			AddressBookDal.Instance.SaveChanges();

			return newPerson.ToDto();
		}


		/// <summary>
		/// Update an existing person record by passing in a PersonDto.  Returns the modified record.
		/// This approach just does individual assignments, as in p.Fname = dto.Fname;, etc
		/// </summary>
		/// <param name="person"></param>
		/// <returns></returns>
		[WebInvoke(Method = "PUT", UriTemplate = "person")]
		public PersonDto UpdatePerson(PersonDto person)
		{
			// Make sure the person exists
			Person existing = Person.GetByKey(person.ID);
			if (existing == null)
				throw new ArgumentException("Person with id " + person.ID + " does not exist.  Cannot update.");

			existing.Fname = person.Fname;
			existing.Lname = person.Lname;
			if (person.Emails != null)
				person.Emails.ForEach(email => existing.Emails.Add(email.ToEntity()));
			existing.DateModified = DateTime.Now;
			AddressBookDal.Instance.SaveChanges();

			return existing.ToDto();
		}


		/// <summary>
		/// Update an existing person record by passing in a PersonDto.  Returns the modified record.
		/// This approach takes the dto and creates an entity object from it, then attaches that entity to the current
		/// (singleton) ObjectContext with an ObjectStateEntry of modified, and then saves the changes.
		/// 
		/// One reason I don't like this method, is you don't have fine grained control over which fields can or cannot be updated.
		/// Also, javascript is a dynamic language.  It's ok if a property is missing, like Lname.  But when that javascript object
		/// is deserialized to a PersonDto, if Lname is missing, it will contain null or "", and your object will be updated to that
		/// value, when what you probably really wanted was for the value to stay the same.
		///
		/// See UpdatePerson3() for my favorite way to update entities.
		/// </summary>
		/// <param name="person"></param>
		/// <returns></returns>
		[WebInvoke(Method = "PUT", UriTemplate = "person2")]
		public PersonDto UpdatePerson2(PersonDto person)
		{
			Person ent = person.ToEntity();

			// Attach the person to the current ObjectContext, and mark it as modified
			AddressBookDal.Instance.Attach(ent);
			AddressBookDal.Instance.ObjectStateManager.ChangeObjectState(ent, EntityState.Modified);

			// Save the changes
			AddressBookDal.Instance.SaveChanges();

			return ent.ToDto();
		}


		/// <summary>
		/// Update an existing person record by passing in the ID of the person, and a dictionary that contains the values
		/// for the person's properties that we're updating.  This uses Loef's ApplyValues() method.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		/// [WebInvoke(Method = "PUT", UriTemplate = "person2")]
		public PersonDto UpdatePerson3(int id, Dictionary<string, object> values)
		{
			Person person = Person.GetByKey(id);
			if (person == null)
				throw new ArgumentException("Person with id " + id + " could not be found.  Update failed.");

			// TODO: Test that the Emails collection updates properly as well
			string[] permittedFields = {"Fname", "Lname", "Emails"};
			person.ApplyValues(values.Where(pair => permittedFields.Contains(pair.Key)));
			person.DateModified = DateTime.Now;
			AddressBookDal.Instance.SaveChanges();
			return person.ToDto();
		}


		/// <summary>
		/// Delete a person record by id.
		/// </summary>
		/// <param name="sID"></param>
		[WebInvoke(Method = "DELETE", UriTemplate = "person/{sID}")]
		public void DeletePerson(string sID)
		{
			int id;
			int.TryParse(sID, out id);
			if (id <= 0)
				throw new ArgumentException("id must be a valid positive integer");

			// Make sure the person exists
			Person existing = Person.GetByKey(id);
			if (existing == null)
				throw new ArgumentException("Person with id " + id + " does not exist.  Cannot update.");

			existing.Delete();
		}


		/// <summary>
		/// Same as Error_NeedsWrappedRequest, but with only a single post arg, so it should work.
		/// This is just to illustrate that multiple args are ok, as long as only *one* are is the "posted" arg.  The others
		/// must either be uri args or query string args.
		/// </summary>
		[WebInvoke(Method = "POST", UriTemplate = "error/needsWrappedRequest/{dummyUriArg1}/{dummyUriArg2}?qarg1={qarg1}&qarg2={qarg2}")]
		public string NoError_NoWrappedRequest(string dummyUriArg1, string dummyUriArg2, string qarg1, string qarg2, string postArg1)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("dummyUriArg1: " + dummyUriArg1);
			sb.AppendLine("dummyUriArg2: " + dummyUriArg2);
			sb.AppendLine("qarg1: " + qarg1);
			sb.AppendLine("qarg2: " + qarg2);
			sb.AppendLine("postArg1: " + postArg1);
			return sb.ToString();
		}


		[WebInvoke(Method = "POST", UriTemplate = "error/test_multiplePost_plusDto/{dummyUriArg1}/{dummyUriArg2}?qarg1={qarg1}&qarg2={qarg2}", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
		public object Test_MultiplePost_PlusDto(string dummyUriArg1, string dummyUriArg2, string qarg1, string qarg2, string postArg1, int postArg2, DateTime postArg3, PersonDto person)
		{
			return new
			{
				dummyUriArg1,
				dummyUriArg2,
				qarg1,
				qarg2,
				postArg1,
				postArg2,
				postArg3,
				person
			};
		}

		/*
		/// <summary>
		/// This would cause an error when trying to get the js client (RestCake), or when calling the service (WCF), because
		/// the last 2 args are both POST arguments (not part of the uri or the query string).
		/// For this to work, the body style would have to be wrapped or wrapped request.
		/// </summary>
		/// <param name="dummyUriArg1"></param>
		/// <param name="dummyUriArg2"></param>
		/// <param name="qarg1"></param>
		/// <param name="qarg2"></param>
		/// <param name="postArg1"></param>
		/// <param name="postArg2"></param>
		[WebInvoke(Method = "POST", UriTemplate = "error/needsWrappedRequest/{dummyUriArg1}/{dummyUriArg2}?qarg1={qarg1}&qarg2={qarg2}")]
		public string Error_NeedsWrappedRequest(string dummyUriArg1, string dummyUriArg2, string qarg1, string qarg2, string postArg1, string postArg2)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("dummyUriArg1: " + dummyUriArg1);
			sb.AppendLine("dummyUriArg2: " + dummyUriArg2);
			sb.AppendLine("qarg1: " + qarg1);
			sb.AppendLine("qarg2: " + qarg2);
			sb.AppendLine("postArg1: " + postArg1);
			sb.AppendLine("postArg2: " + postArg2);
			return sb.ToString();
		}
		*/


		[WebGet(UriTemplate = "person/{sID}/addresses")]
		public AddressDto[] GetAddresses(string sID)
		{
			int personID;
			int.TryParse(sID, out personID);
			if (personID <= 0)
				throw new ArgumentException("id must be a valid positive integer");

			return AddressBookDal.Instance.Addresses.Where(addr => addr.PersonID == personID).ToDtos();
		}

	}
}
