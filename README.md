restcake
========

RestCake - quickly implemented http based clients &amp; services in ASP.NET.



## Example service class

Host in IIS (haven't tested with IIS Express) and once you build, navigate to http://localhost/your/service/route/_help to see the help page RestCake outputs for your service.

```
using System.IO;
using System.Web;
using RestCake;
using Newtonsoft.Json;

[RestService(Namespace = "MyTest", Route = "rest/helloservice", JsClientVarName="g_hellosvc")]
public class HelloService : RestCakeHandler
{
	// optional; great place to initialize things (dal, user request, dependency injection, etc) before the service method is called
	public override bool BeforeServiceMethod(MethodMetadata method, object[] args)
	{
		// do things
		return true;
	}

	// optional; chance to customize the Json.NET serializer settings for this class
	public override JsonSerializerSettings GetSerializerSettings()
	{
		var settings = base.GetSerializerSettings();
		settings.DateTimeZoneHandling = DateTimeZoneHandling.Unspecified; // etc
		return settings;
	}


	// basic GET method
	[Get]
	public string Hello()
	{
		return "Hello world";
	}

	// force query string style params for GET
	[Get(UrlStyle = UrlStyle.QueryString)]
	public string Hello2(string fname, string lname)
	{
		return "Hello " + fname + " " + lname;
	}


	// using POST
	[Post]
	public string CreateNewProject(string name)
	{
		return "Hello " + name;
	}

	// posting an object type
	[Post]
	public SomeResult (SomeObject obj)
	{
		return obj.GetResult();
	}

	// can use PUT if desired
	[Put]
	public SomeResult (SomeObject obj)
	{
		return obj.GetResult();
	}

	// controlling how the body is posted (body style)
	[Post(BodyStyle = BodyStyle.WrappedRequest)]
	public UserDto UpdateUser(int userID, SomePayloadType data)
	{
		// ...
		return updatedUserDto;
	}

	// will use DELETE verb
	[Delete]
	public bool DeleteThing(int thingID)
	{
		// ...
		return true;
	}

	// can hook into standard membership provider for user/role authorization
	[Authorize(Roles = new[] { "Admin" })]
	[Delete]
	public bool DeleteUser(int userID)
	{
		// ...
		return true;
	}

	// custom method authorization
	[Authorize(Method = "CustomAuthMethod")]
	[Get]
	public string CustomAuthExample()
	{
		return "foo";
	}

	public bool CustomAuthMethod()
	{
		// do custom auth here
		return true;
	}

}

```
