using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using RestCake.AddressBook.DataAccess;

namespace RestCake.AddressBook.Services
{
	/// <summary>
	/// This service class has 4 method, that all take the same argument and return the exact same data.
	/// The difference is in how the responses and requests are "wrapped" using different values for WebMessageBodyStyle.
	/// On the consuming page, take a look at how the data is packaged in the raw json.
	/// </summary>
	[ServiceContract(Namespace = "RestCakeExamples", Name = "BodyStyle")]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class WebMessageBodyStyleComparisonService : RestHttpHandler
	{
		public override bool IsReusable
		{
			get { return true; }
		}

		[WebInvoke(Method = "POST", UriTemplate = "wrapped", BodyStyle = WebMessageBodyStyle.Wrapped)]
		public PersonDto WrappedTest(int id)
		{
			Person person = Person.GetByKey(id);
			return (person == null) ? null : person.ToDto();
		}

		[WebInvoke(Method = "POST", UriTemplate = "wrappedRequest", BodyStyle = WebMessageBodyStyle.WrappedRequest)]
		public PersonDto WrappedRequestTest(int id)
		{
			Person person = Person.GetByKey(id);
			return (person == null) ? null : person.ToDto();
		}

		[WebInvoke(Method = "POST", UriTemplate = "wrappedResponse", BodyStyle = WebMessageBodyStyle.WrappedResponse)]
		public PersonDto WrappedResponseTest(int id)
		{
			Person person = Person.GetByKey(id);
			return (person == null) ? null : person.ToDto();
		}

		[WebInvoke(Method = "POST", UriTemplate = "bare", BodyStyle = WebMessageBodyStyle.Bare)]
		public PersonDto BareTest(int id)
		{
			Person person = Person.GetByKey(id);
			return (person == null) ? null : person.ToDto();
		}
	}
}