<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Overview.aspx.cs" Inherits="ExampleServices.Overview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
	<div class="bar blue-bar">
		<div class="title">In This Document</div>
		<ul>
			<li><a href="#why-not-use-wcf">Why not just use WCF?</a></li>
			<li><a href="#wcf-date-formatting">Date Formatting with DataContractJsonSerializer</a></li>
			<li><a href="#no-anonymous-types">No Serialization of Anonymous Types</a></li>
			<li><a href="#too-much-configuration">Too Much Configuration</a></li>
			<li><a href="#no-graphs-with-cycles">WCF Can't Serialize Object Graphs That Contain Cycles</a></li>
			<li><a href="#no-global-error-handling">No Global Error Handling</a></li>
			<li><a href="#no-uri-non-string-parameters">Parameter Values for your Service Methods Have to be Passed in as Strings</a></li>
			<li><a href="#bad-dictionary-serialization">Serialization of Dictionaries is not "Sane" with WCF</a></li>
			<li><a href="#immutable-types-annoyance">DataContracts on Immutable Types are Annoying</a></li>
			<li><a href="#what-is-rest-cake"><strong>Enough hating on WCF REST! What is RestCake?</strong></a></li>
			<li><a href="#what-have-i-gained">So if the Service Class is Essentially the Same, What Have I Gained?</a></li>
			<li><a href="#automatic-js-proxies">But wait, there's more! (automatic javascript proxies)</a></li>
			<li><a href="#easy-to-try">It will take you less than five minutes.  Just try it.</a></li>
		</ul>
	</div>

	<a name="why-not-use-wcf"></a>
	<div class="bar light-blue-bar">
		<div class="light-blue-title"><a href="#why-not-use-wcf">Why not just use WCF?</a></div>

		<strong>RestCake</strong>  is a C# library for creating REST services without WCF.
		WCF has many pain points:
		<ul>
			<li>Date formatting is horrible (DataContractJsonSerializer)</li>
			<li>Can't serialize anonymous types</li>
			<li>Too much configuration</li>
			<li>Can't handle cycles in object graphs</li>
			<li>No global error handling</li>
			<li>UriTemplate params can only be passed in as strings</li>
		</ul>

		WCF is designed to be host agnostic, whereas REST implementations are very simple, and (for practical purposes) always use HTTP.
		<br />
		I love this quote (<a href="http://www.pluralsight-training.net/community/blogs/tewald/archive/2007/08/26/48298.aspx">source</a>):
		<blockquote>
			"I'm not sure I want to build on a layer [WCF REST] designed to factor HTTP in on top of a layer that was designed to factor it out [WCF]."
		</blockquote>

		All in all, WCF is overkill for REST servcies that are not going to have any other endpoints, and will not be hosted outside of IIS.
		All of its noble idealisms and abstractions get in the way.
		Let me elaborate a bit more about how WCF gets in the way.

	</div>

	<a name="wcf-date-formatting"></a>
	<div class="bar light-blue-bar">
		<div class="light-blue-title"><a href="#wcf-date-formatting">Date Formatting with DataContractJsonSerializer</a></div>
		<span class="code">"\/Date(1280555181054-0700)\/"</span>. Ouch! If you're using the MS Ajax runtime, sure they fix this up for you,
		but it's not very friendly. I've seen everything from hacks on the json2.js file to automatically fix these up to quick javascript
		utility methods to convert to/from this awful date format. The worst part is you don't even have a choice. RestCake uses
		the <a href="http://json.codeplex.com">Json.NET</a> serializer, which lets you choose how your dates will be formatted.
		The default format (ISO date format) can be parsed from its string representation directly to a javascript date, and a native javascript date
		can be passed to the service and deserialized into a DateTime object.  More than I can say for the MS AJAX format...

	</div>


	<a name="no-anonymous-types"></a>
	<div class="bar light-blue-bar">
		<div class="light-blue-title"><a href="#no-anonymous-types">No Serialization of Anonymous Types</a></div>
		Sometimes you want your service method to return a simple pair or triple of objects. With anonymous types, it's very easy to creates
		these kinds of tuples:

		<pre class="brush: csharp">
			var result = new
			{
				Person = somePerson,
				Status = Status.Success,
				NumProjects = projects.Count()
			};
		</pre>

		The DataContractJsonSerializer can't serialize anonymous types, so you'd have to create a class that represents each result type that you
		want your service methods to return. Json.NET, on the other hand, will serialize the above object just fine.
	</div>

	
	<a name="too-much-configuration"></a>
	<div class="bar light-blue-bar">
		<div class="light-blue-title"><a href="#too-much-configuration">Too Much Configuration</a></div>
		While they have cut down the amount of configuration for WCF significantly with recent releases, there is still a lot to do. The
		&lt;system.serviceModel&gt; section in your web.config can get pretty large sometimes. RestCake doesn't require any special settings
		in your web.config. The REST services are based on a very simple base class, <strong>RestHttpHandler</strong> that implements
		the <strong>System.Web.IHttpHandler</strong> interface. Calls to your service simply go to the <strong>ProcessRequest()</strong> method
		of that interface, and it's all based on standard HTTP. While REST is a "high level concept", in practice it's nearly always implemented using
		the HTTP protocol, and a basic IHttpHandler is more than enough to get a flexible service going.

		<br />
		<br />
		RestCake requires no configuration in your web.config. At some point there will probably be an option to specify certain global settings,
		but it's not required.
	</div>


	<a name="no-graphs-with-cycles"></a>
	<div class="bar light-blue-bar">
		<div class="light-blue-title"><a href="#no-graphs-with-cycles">WCF Can't Serialize Object Graphs That Contain Cycles</a></div>
		What's a cycle? An example would be if you had a Parent object with a List&lt;Child&gt; named Children. Each Child object
		in that list has a Parent property that refers back to the Parent. Parent.Children[0].Parent.Children[0].Parent.Children[0]....we're
		going in circles. That's a cycle, and because json can't represent references in an object literal, the DataContractJsonSerializer
		throws an exception when it encounters a cycle in your object graph. While that's very noble and idealistic, it's not very practical.
		Json.NET gives you many different options on how to deal with cycles, the easiest being to simply ignore them. In the example case,
		the Parent object would have a list of child objects, but those child objects would not have a reference back to the parent. For
		most people's usage scenarios (getting DTO objects to your client script to update the web UI), that's just fine.
	</div>


	<a name="no-global-error-handling"></a>
	<div class="bar light-blue-bar">
		<div class="light-blue-title"><a href="#no-global-error-handling">No Global Error Handling</a></div>
		There may be someway to easily handle all errors in your WCF service methods, but I certainly haven't found it. I'm really tired of
		creating FaultException&lt;ArgumentException&gt; objects. Really? An exception type that takes another exception type
		as a generic type parameter? Oh, and when you create that, you end up having to repeat all your string messages twice. Very fun. You
		can forget about <strong>Application_Error()</strong> in your Global.asax too. Doesn't fire when an error occurs in your WCF
		service method. But guess what! It <em>does</em> fire when an error occurs in an IHttpHandler! The
		<strong>RestHttpHandler</strong> in RestCake is based off of the simple <strong>IHttpHandler</strong> interface, and handling
		uncaught exceptions is a breeze. Once again, WCF's noble host agnosticism has made something that should be easy, quite difficult.
	</div>


	<a name="no-uri-non-string-parameters"></a>
	<div class="bar light-blue-bar">
		<div class="light-blue-title"><a href="#no-uri-non-string-parameters">Parameter Values for your Service Methods Have to be Passed in as Strings</a></div>
		Any variables in your UriTemplate that are part of the Uri and not the query string, have to have matching parameters in your service
		method, and the type <strong>has</strong> to be string. It's can't be any of the other primitive data types. So
		in your service methods, you end up doing a lot of validation and TryParsing, which is not fun. It gets in the way and creates noise.
	</div>


	<a name="bad-dictionary-serialization"></a>
	<div class="bar light-blue-bar">
		<div class="light-blue-title"><a href="#bad-dictionary-serialization">Serialization of Dictionaries is not "Sane" with WCF</a></div>
		By not "sane" I mean you don't get what you want 99% of the time. A bit of code is the easiest way to explain this one.

		<script type="syntaxhighlighter" class="brush: csharp">
			// WCF DCJS way
			var dict = new Dictionary<string, string>()
			{
				{ "Fname", "Sam" },
				{ "Lname", "Meacham" }
			};

			string dictJson;
			using (MemoryStream mstream = new MemoryStream())
			{
				DataContractJsonSerializer srlzr_srsly =
					new DataContractJsonSerializer(typeof(Dictionary<string,string>));
				srlzr_srsly.WriteObject(mstream, dict);
				dictJson = Encoding.ASCII.GetString(mstream.ToArray());
			}
			Console.WriteLine(dictJson);
		</script>

		This outputs: <span class="code">[{"Key":"Fname","Value":"Sam"},{"Key":"Lname","Value":"Meacham"}]</span>.
		What? Oh, it's a collection of (key,value) pairs. How noble! Yes, it will work if I use something totally
		unreasonable as a key value, such as "#$%^&amp;", but that would be stupid, and I wouldn't do it.

		<br />
		<br />
		
		Let's take a look at how Json.NET serializes that dictionary:

		<script type="syntaxhighlighter" class="brush: csharp">
			var dict = new Dictionary<string, string>()
			{
				{ "Fname", "Sam" },
				{ "Lname", "Meacham" }
			};
	
			Console.WriteLine(JsonConvert.SerializeObject(dict));
		</script>

		In addition to the actual serialization being much, much easier, the result is sane!
		<span class="code">{""Fname":"Sam","Lname":"Meacham"}</span>. Just what I wanted. Now, if for some reason I
		need the other behavior, a list of (key,value) pairs, it's easy enough:

		<script type="syntaxhighlighter" class="brush: csharp">
			var pairs = new List<KeyValuePair<string, string>>(dict);
			Console.WriteLine(JsonConvert.SerializeObject(dict));
		</script>

		So we get what we want 99% of the time with the default behavior, and write 1 extra line of effort when we want the 1% of
		the time behavior. Where with WCF, you get what you <em>don't</em> want 99% of the time, and since it can't
		serialize anything sane, you have to write a bunch of javascript code to "fix up" the dictionaries you get back from your
		services. And it's a LOT of extra effort. Especially when you have to "reverse fix up" the values before
		you send them back to your services. Ouch.
	</div>


	<a name="immutable-types-annoyance"></a>
	<div class="bar light-blue-bar">
		<div class="light-blue-title"><a href="#immutable-types-annoyance">DataContracts on Immutable Types are Annoying</a></div>
		This is a strange one, and a bit trivial, but it's very annoying. I like immutable types. Not having to deal
		with state change is a good thing, especially when you want to start doing a lot of parallel processing. Immutable
		types are immune to side effects. They don't require having locks taken out on them. Et cetera et
		cetera. Consider this simple immutable type:

		<script type="syntaxhighlighter" class="brush: csharp">
			[DataContract]
			public class Person
			{
				private readonly int m_id;
				private readonly string m_name;

				[DataMember] public int ID { get { return m_id; } }
				[DataMember] public string Name { get { return m_name; } }
	
				public Person(int id, string name)
				{
					m_id = id;
					m_name = name;
				}
			}
		</script>
		
		Trying to serialize or deserialize an object of type Person with the DataContractJsonSerializer will result
		in a runtime exception, because the [DataMember] properties don't have setters. While it does work if
		you have private setters, your type isn't really immutable then, is it? You can't have automatic properties
		that are based on readonly backing fields. The solution is to put the [DataMember] attributes on your backing
		fields (m_id and m_name in this case), but then you have to do this:

		<script type="syntaxhighlighter" class="brush: csharp">
			[DataMember(Name = "ID")]
			private readonly int m_id;
	
			[DataMember(Name = "Name")]
			private readonly string m_name;

			public int ID { get { return m_id; } }
			public string Name { get { return m_name; } }
		</script>
		Very annoying. And not refactor safe. Once again, Json.NET has none of these problems. In fact,
		your types don't need any special attributes at all.
	</div>



	<a name="what-is-rest-cake"></a>
	<div class="bar light-blue-bar">
		<h1 class="light-blue-title" style="font-size: 2em"><a href="#what-is-rest-cake">Enough hating on WCF REST! What is RestCake?</a></h1>
		Let's take a look at a very basic WCF REST service.

		<script type="syntaxhighlighter" class="brush: csharp">
			[ServiceContract(Namespace = "RestCake")]
			public class HelloWorldService
			{
				[WebGet(UriTemplate="SayHello/{name}")]
				public static string SayHello(string name)
				{
					return "Hello " + name;
				}
			}
		</script>

		Now, to get JSON, you'd either have to specify the WebMessageFormat in the WebGet attribute,
		or specify that you want the default to be JSON in your web.config, somewhere 	in the &lt;system.serviceModel&gt;
		section. That's not a bad thing, it's just something you have to remember to do, or you'll get XML back (hint: I'm
		not a huge fan of XML).

		<br />
		<br />

		To access that service, you've got quite a few options. You can create a .svc file that specifies what host factory
		you want to use, and then navigate to that .svc file in your browser. Or, you can ditch the .svc file, and set up some
		ServiceRoutes in your Global.asax file, using the WebServiceHostFactory. That last option is the best, because you get clean urls.

		<br />
		<br />

		Let's see how we'd create this same service with <strong>RestCake</strong>.

		<script type="syntaxhighlighter" class="brush: csharp">
			[ServiceContract(Namespace = "RestCake")]
			public class HelloWorldService : RestHttpHandler
			{
				[WebGet(UriTemplate="SayHello/{name}")]
				public static string SayHello(string name)
				{
					return "Hello " + name;
				}
			}
		</script>

		Do you see the difference? It's subtle! This HelloWorldService class inherits from <strong>RestHttpHandler</strong>,
		and <strong>that is the only difference.</strong> You don't even need a .ashx file for your IHttpHandler, though you can
		certainly use one if you want. If you have a .ashx file, you can just navigate right to it to use your service. If you don't,
		you can set up a <strong>GenericHandlerRoute</strong> (part of RestCake) in your Global.asax, like this:

		<script type="syntaxhighlighter" class="brush: csharp">
			protected void Application_Start(object sender, EventArgs e)
			{
				// A "headless" IHttpHandler route (no .ashx file required)
				RouteTable.Routes.Add(new GenericHandlerRoute<SuperHotService>("services/hot"));	
			}
		</script>

		RestCake uses the existing WCF attributes to figure everything out. It's based on the same <strong>UriTemplate</strong>
		processing. When this service is called for the first time, The <strong>RestHttpHandler</strong> base class will collect
		some metadata on all of the service methods in the service class. ProcessRequest() will figure out which method needs to
		be called, and call it with the parameter values that came with your service call (from the UriTemplate, the query string,
		or the POST, PUT, or DELETE data, either in query string or json format).

		<br />
		<br />

		Also, with the above RestCake service, we don't need any special settings in our web.config. You navigate to the ashx file,
		or the url of the route you set up, and it <strong>just works.</strong>
	</div>


	<a name="what-have-i-gained"></a>
	<div class="bar light-blue-bar">
		<div class="light-blue-title"><a href="#what-have-i-gained">So if the Service Class is Essentially the Same, What Have I Gained?</a></div>

		<ul>
			<li>You have control over date formatting.</li>
			<li>Your service methods can return anonymous types.</li>
			<li>On the whole, your services execute more quickly (Json.NET is faster than the DCJS).</li>
			<li>You don't have any configuration to worry about.</li>
			<li>You can return object graphs that have cycles without getting runtime errors (this is great for you Entity Framework guys...).</li>
			<li>Your Application_Error method will fire for any unhandled exceptions.</li>
			<li>Dictionaries are serialized that way you want 99% of the time.</li>
			<li>Your service methods can take input UriTemplate parameters of any type: strings, primitive types, user class types, etc.</li>
		</ul>
	</div>


	<a name="automatic-js-proxies"></a>
	<div class="bar light-blue-bar">
		<div class="light-blue-title"><a href="#automatic-js-proxies">But wait, there's more!</a></div>
		Using the <strong>JsClientCreator</strong>, you can either create a static javascript file from your assembly that has your services,
		or generate the javascript files on the fly by navigating to your service special url. The javascript file is a <strong>jquery client</strong>.
		It's based on the <a href="http://www.west-wind.com/weblog/posts/324917.aspx"> WCF proxy by Rick Strahl</a>, but takes it quite a bit
		further. Javascript client classes are created to mirror each of your service classes, and you have all of your service methods
		ready to be called. Here's what calling that HelloWorld service would look like in client script.

		<script type="syntaxhighlighter" class="brush: js">
			&lt;script type="text/javascript" src="/RestCakeTest/HelloWorldService/_js?jquery&base=true"&gt;&lt;/script&gt;

			&lt;script type="text/javascript"&gt;
				var g_svc = new TestServices.HelloWorldServiceClient("/RestCakeTest/HelloWorldService/");

				function test()
				{
					g_svc.SayHello("Sam", function (result)
					{
						alert("result: " + result);
					});
				}
			&lt;/script&gt;
		</script>

		Very easy. Include the service client script, create a new instance of the service client, providing the base url to the service,
		and call your method. In Visual Studio, you'll even get Intellisense for the method names, and the names of the arguments.
		You don't have to do any JSON.stringifying or parsing yourself, it's all wrapped up in the javascript client classes.
	</div>


	<a name="easy-to-try"></a>
	<div class="bar light-blue-bar">
		<div class="light-blue-title"><a href="#easy-to-try">It will take you less than five minutes.  Just try it.</a></div>
		<ol>
			<li>Download RestCake</li>
			<li>Add the reference to your existing WCF REST services project</li>
			<li>Open up one of your [ServiceContract] classes, add the <span class="code">using RestCake</span> directive, and change to class to inherit from <span class="code">RestHttpHandler</span></li>
			<li>Go to your Global.asax Application_Start(), and create a new <span class="code">GenericHandlerRoute&lt;T&gt;</span> route with a unique url ("services/myservice")</li>
			<li>In a web page, add a &lt;script&gt; tag with the source pointing to "yoursite/services/myservice/_js?jquery&amp;base=true" to include the automatic javascript client class</li>
			<li>Create a new service client: <span class="code">var g_svc = new MyNamespace.MyServiceClient("/services/myservice/");</span></li>
			<li>Call your service methods from the javascript client object: <span class="code">g_svc.SayHello(arg1, function(){ /* success handler */ });</span></li>
		</ol>

		<br />

		Once you're convinced, you can get rid of a bunch of WCF stuff in your web.config, cleaning things up a bit. You can change your service
		methods to have correctly typed input parameters. No more TryParsing all the strings you were passing in before. You can get rid of all
		the javascript code that "fixed up" the results of crappy, inflexible serialization. You can get rid of classes that you created just to
		create tuples of objects you wanted to return together. You can get rid of whatever crazy error handling you had to handle exceptions in
		your service methods. You can forget about faults altogether. Rest is fun again. As fun at eating cake.

	</div>
</asp:Content>
