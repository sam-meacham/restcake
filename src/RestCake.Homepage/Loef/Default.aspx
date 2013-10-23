<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ExampleServices.Loef.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" runat="server">
	<div class="bar blue-bar">
		<div class="title">In This Document</div>
		<ul>
			<li><a href="#what-is-loef">What is Loef?</a></li>
			<li>
				<a href="#the-templates">The Templates</a>
				<ul>
					<li><a href="#DtosTemplate-template">DtosTemplate.tt</a></li>
					<li><a href="#DTO-strip-cycles">StripCycles()</a></li>
					<li><a href="#AutoMapperConfig-template">AutoMapperConfig.tt</a></li>
					<li><a href="#EntityToDto-template">EntityToDto.tt</a></li>
				</ul>
			</li>
			<li>
				<a href="#utility-classes">The Utility Classes</a>
				<ul>
					<li><a href="#PagedData-class">PagedData</a></li>
					<li><a href="#PerRequestSingleton-class">PerRequestSingleton</a></li>
				</ul>
			</li>
			<li><a href="#loef-example">Let me see an example!</a></li>
		</ul>
	</div>


	<a name="what-is-loef"></a>
	<div class="bar light-blue-bar">
		<div class="title"><a href="#what-is-loef">What is Loef?</a></div>
		Loef (said "loaf") is a small set of utility classes and
		<a href="http://msdn.microsoft.com/en-us/library/bb126445.aspx">T4 (.tt) templates</a> (for code generation)
		to help with setting up a proper data access layer (DAL) using
		Microsoft's <a href="http://msdn.microsoft.com/en-us/library/bb399572.aspx">Entity Framework v4</a>.
		Based on your .edmx model, the templates will generate the following for you:
		<ul>
			<li>DTO (<a href="http://en.wikipedia.org/wiki/Data_transfer_object">data transfer object</a>) classes (marked with <span class="code">[DataContract]</span>, so they are ready for use with WCF if you please)</li>
			<li><a href="http://automapper.codeplex.com/">AutoMapper</a> mappings to automatically convert to/from your entity and DTO classes</li>
			<li>Strongly typed eager fetching, over multiple levels, for full customization of object graph retrieval</li>
			<li>A statically accessible singleton instance of your ObjectContext class, ready for use in ASP.NET or WCF with automatic lifetime management</li>
			<li>A <span class="code">StripCycles()</span> method in all your DTO classes, that will remove all cycles in the object graph for easy serialization with WCF (without errors)</li>
			<li><span class="code">GetByKey()</span> methods in your entity classes, for easy retrieval of entities by primary key, with optional eager loading (strongly typed) of related entities</li>
		</ul>

		Loef is not a large overbearing framework.  It's small, unobtrustive, and easily modified.  You can use all of it, or just a few parts.
		It's <strong>6 utility classes, and 3 .tt templates</strong>, but it will help you get your Data Layer rolling in just a few minutes.
	</div>

	
	<a name="the-templates"></a>
	<div class="bar light-blue-bar">
		<div class="title"><a href="#the-templates">The Templates</a></div>

		There is a <strong>DtoTemplateSettings.ttinclude</strong> file
		that looks like this:

		<script type="syntaxhighlighter" class="brush: csharp">
			<#
				//
				// CONFIG SECTION.  Change these values to be specific to your project.
				//

				// This should be the relative path from this template to your .edmx file
				string inputFile = "../YourEntityModel.edmx";

				// The namespace where your DTO classes are contained
				string dtoClassNamespace = "Test.DataAccess";

				// The namespace where your entity classes are contained
				string entityClassNamespace = "Test.DataAccess";
	
				string dalClassName = "TestDal";

				// Additional settings
				// ...
			#>
		</script>

		So you setup the path to your .emdx data model in that config file, and set a few namespace names and other settings.  You'll want to add that file and
		the other .tt templates to your data access project in a folder named "Dtos" or "Templates" (or whatever you'd like).

		<br />
		<img src="../Public/images/AddLoefTemplatesToProject.png" border="0" alt="" />
		<br />

		<br />
		<br />

		<a name="DtosTemplate-template"></a>
		<div class="title"><a href="#DtosTemplate-template">DtosTemplate.tt</a></div>
		Consider a basic Person class that has contact information.  On the left we see the diagram for the Entity object that Entity Framework
		modeled for us, based on the database.  On the right is the DTO class that was automatically generated from the T4 template.

		<div class="clear"></div>

		<div class="bar light-blue-bar fleft" style="margin-right: 30px;">
			<img src="../Public/images/PersonClassDiagram.png" border="0" alt="Person Class Diagram" />
		</div>

		<div class="bar light-blue-bar fleft" style="width: 765px">
			<script type="syntaxhighlighter" class="brush: csharp">
				[DataContract]
				public  partial class PersonDto
				{
					/// <summary>Parameterless constructor (important for serialization)</summary>
					public PersonDto()
					{ }
	
					public Person ToEntity()
					{
						return AutoMapper.Mapper.Map<PersonDto, Person>(this);
					}
	
					// This is a StripCycles() method in here too, which I'll cover in detail later

					// ***************************************************************************
					// *** Primitive properties **************************************************
					// ***************************************************************************

					[DataMember] public int ID { get; set; }
					[DataMember] public string Fname { get; set; }
					[DataMember] public string Lname { get; set; }
					[DataMember] public string Title { get; set; }
					[DataMember] public string Company { get; set; }
					[DataMember] public Nullable<System.DateTime> Birthday { get; set; }
					[DataMember] public string Notes { get; set; }
					[DataMember] public System.DateTime DateCreated { get; set; }
					[DataMember] public System.DateTime DateModified { get; set; }

					// ***************************************************************************
					// *** Navigation properties *************************************************
					// ***************************************************************************

					[DataMember] public List<EmailAddressDto> Emails { get; set; }
					[DataMember] public List<PhoneDto> Phones { get; set; }
					[DataMember] public List<WebsiteDto> Websites { get; set; }
					[DataMember] public List<GroupDto> Groups { get; set; }
					[DataMember] public List<AddressDto> Addresses { get; set; }
				} // end class
			</script>
		</div>

		<div class="clear"></div>

		Note that the navigation properties that are collections have lists of DTO types: so when you get a DTO object graph,
		it's all DTO types.  A DTO type never references an Entity type.  Getting a DTO type is the easiest thing in the world.

		<script type="syntaxhighlighter" class="brush: csharp">
			// First we get the actual entity type using the EF model.
			// We'll use Loef's GetByKey() method that's added to every entity class to do this, and
			// we'll do a strongly typed eager fetch of the Person's Addresses collection as well.
			Person sam = Person.GetByKey(15, p => p.Addresses); // Get person with ID=15, including their Addresses

			// Now we'll use the entity's ToDto() method that Loef generated for us, which uses AutoMapper to
			// automatically create our PersonDto object based on the values in the Person entity object.
			// This includes the Person's Addresses collection, which will be converted to a List<AddressDto>.
			PersonDto samDto = sam.ToDto();

			// You can even do it for an IEnumerable<TEntityType>.
			PersonDto[] dtos = context.People.Where(p => p.Lname.StartsWith("A")).ToDtos();
		</script>

		<br />
		<br />
		<a name="DTO-strip-cycles"></a>
		<div class="title"><a href="#DTO-strip-cycles">StripCycles()</a></div>

		The DTO classes will all have a <span class="code">StripCycles()</span> method as well.  I left it out of the code sample above,
		because depending on the class, and the number of relationships it has with other classes, it can become complicated.  They are recursive
		methods, though indirectly (A calls B which calls A which calls B, etc) that strip any cycles out of your DTO graph.  This is handy when you need
		to serialize an object graph, and your particular flavor of serializer throws exceptions when it encounters a cycle (like anything with WCF).
		
		<br />
		<br />
		<div class="title">What exactly is a cycle?</div>
		If you had a parent object with a collection of child objects, and each child object has a reference back to its parent, you've
		got cycles.  <span class="code">parent.children[0].parent == parent</span>.  You could keep coding yourself in circles.  Well, certain formats don't support
		cycles, like json.  So these cycles often need to be removed (unless you're using the much esteemed <a href="http://json.codeplex.com">Json.NET</a> serializer,
		which can simply be told to ignore them for you), before you serialize into those formats.

		<script type="syntaxhighlighter" class="brush: csharp">
			// StripCycles() can be called on individual DTO objects, or an IEnumerable of them.
			PersonDto[] dtos = context.People.Where(p => p.Lname.StartsWith("A")).ToDtos().StripCycles();

			// dtos is now safe to serialize, as it contains no cyclic references.
		</script>


		<br />
		<br />
		<a name="AutoMapperConfig-template"></a>
		<div class="title"><a href="#AutoMapperConfig-template">AutoMapperConfig.tt</a></div>

		This template creates a single static class named <span class="code">AutoMapperConfig</span> that contains a single public static method: <span class="code">CreateMappings()</span>.
		Here's a snippet of what's in that method for our <span class="code">Person</span> to <span class="code">PersonDto</span> mapping.

		<script type="syntaxhighlighter" class="brush: csharp">
			// AutoMapper config for Person => PersonDto
			AutoMapper.Mapper.CreateMap<Person, PersonDto>()
				.ForMember(dto => dto.Emails,
					options => options.MapFrom(obj => obj.Emails.IsLoaded ? obj.Emails : null))
				.ForMember(dto => dto.Phones,
					options => options.MapFrom(obj => obj.Phones.IsLoaded ? obj.Phones : null))
				.ForMember(dto => dto.Websites,
					options => options.MapFrom(obj => obj.Websites.IsLoaded ? obj.Websites : null))
				.ForMember(dto => dto.Groups,
					options => options.MapFrom(obj => obj.Groups.IsLoaded ? obj.Groups : null))
				.ForMember(dto => dto.Addresses,
					options => options.MapFrom(obj => obj.Addresses.IsLoaded ? obj.Addresses : null));
				
			// Reverse config (dto => entity)
			AutoMapper.Mapper.CreateMap<PersonDto, Person>();
		</script>

		So all <span class="code">CreateMappings()</span> does is set up the <a href="http://automapper.codeplex.com">AutoMapper</a> maps for going back
		and forth between your DTO and Entity types automatically.  What's even better, is the AutoMapper usage is all wrapped up in
		the code generated by Loef, with the Entity classes having <span class="code">ToDto()</span> methods, and the DTO classes having <span class="code">ToEntity()</span> methods.
		Just make sure you call <span class="code">AutoMapperConfig.CreateMappings()</span> when your application starts (say, in your Global.asax Application_Start method).

		<br />
		<br />

		Also worth noting is that calling <span class="code">someEntity.ToDto()</span> will not trigger lazy loading in your model.
		Look at the lambdas used in MapFrom() that make sure that <span class="code">EntityCollection</span> and <span class="code">EntityObject</span>
		types are loaded before including them in the DTO.  This means your DTO graphs will always exactly match your Entity graphs.



		<br />
		<br />
		<a name="EntityToDto-template"></a>
		<div class="title"><a href="#EntityToDto-template">EntityToDto.tt</a></div>

		This template creates partial classes for all of your entity types, to add a few convenience methods and properties to each of your entity classes.
		<br />
		<span class="code">GetByKey()</span> (and its various overloads) allow for eager fetching of related entities over multiple levels.
		Here are a few examples.

		<script type="syntaxhighlighter" class="brush: csharp">
			// Load up a specific Person object, with their Addresses and Groups collections eagerly fetched, as well
			// as the People collection for each Group.  So this illustrates eager loading a graph with multiple
			// branches, and multiple levels for one of those branches, all strongly typed.
			Person sam = Person.GetByKey(15, p => p.Addresses, p => p.Groups.People);

			// Sometimes what you're going to eagerly load is not known.  Someone might make a service call, and
			// the relationships to load are passed as a string.  This will fetch the exact same data as the
			// previous example.  It's just not strongly typed, because it has to be dynamic.
			// Note how the separate branches are separated by commas, and levels by dots.
			Person sam2 = Person.GetByKey(15, "Addresses,Groups.People");
		</script>

		The EntityToDto.tt also creates a static class named <span class="code">EntityExtensionMethods</span> that contains specific extension methods for
		eager loading in a fashion similar to EF's <span class="code">Include()</span> extension method, just with strong typing.

		<script type="syntaxhighlighter" class="brush: csharp">
			// This will return the exact same results as the 2 examples above, and in fact uses the same
			// metadata in the entity partial classes to resolve the association paths to strings
			var q =
				from people in context.People.EagerLoad(p => p.Addresses).EagerLoad(p => p.Groups.People)
				select people;
			List<Person> peeps = q.ToList();
		</script>


		<br />
		<br />
		<a name="utility-classes"></a>
		<div class="bar light-blue-bar">
			<div class="title"><a href="#utility-classes">The Utility Classes</a></div>

			There a 6 simple utility classes used in Loef.  Four of them, you'll probably never even use directly, as they are mostly used internally
			by the code generated from the T4 templates.
			<ul>
				<li>
					<span class="code">IncludeChainBase</span> (this is used for the strongly typed eager fetching, so things like p.Groups.People resolves to a string).
					Each entity class has a nested class that inherits from this type.  The nested class simply contains the association paths to
					other related entities.
				</li>
				<li>
					<span class="code">QueryExtensions</span>.  This class contains various extension methods for ObjectQuery and IQueryable.  One of the extension
					methods you might use directly is <span class="code">Order&lt;T&gt;</span>.  The default OrderBy lets you order by a column given as a string,
					but OrderByDescending does not.  This method helps with that.
				</li>
				<li>
					<span class="code">UnitOfWorkStore</span>.  HttpContext.Current.Items is a little gem of a storage bag.  It's basically a storage location
					that's unique per user, per postback.  The lifetime of objects placed in this bag (which are stored by a string key) is limited to the
					duration of a single request and response.  Why, that sounds like exactly what we need regarding our ObjectContext instances, doesn't it?
					This class is used to store the globally accessible singleton instance of your ObjectContext, so that each user has their own context for
					each request.
				</li>
				<li>
					<span class="code">Singleton</span>.  This one you'll probably never use, and it's actually not even used by Loef currently.
					It was an interim class used here and there until the <span class="code">PerRequestSingleton</span> (see below) class was created.
				</li>
			</ul>

			There are 2 more classes that are quite handy, and that you'll want to use directly in your code.

			<br />
			<br />
			<a name="PagedData-class"></a>
			<div class="title"><a href="#PagedData-class">PagedData</a></div>
			If any of your operations deal with paging data, this class is your friend.  It has a couple of overloaded constructors that make
			returning paged data based on an existing <span class="code">IQueryable</span> or <span class="code">IEnumerable</span> a snap.

			<script type="syntaxhighlighter" class="brush: csharp">
				var people = context.People
					.EagerLoad(p => p.Addresses)
					.Where(p => p.Lname.StartsWith("Z"));

				// using the IQuerable, give me 10 records (page size), starting on the first page (index 0).
				PagedData page = new PagedData(people, 10, 0);
			</script>

			The PagedData class is very small and simple.  You can understand it at a glance.
			

			<br />
			<br />
			<a name="PerRequestSingleton-class"></a>
			<div class="title"><a href="#PerRequestSingleton-class">PerRequestSingleton</a></div>
			This is a fun one.  Let's assume that with this whole AddressBook example, our <span class="code">ObjectContext</span> class
			in our .edmx model is named <span class="code">AddressBookEntities</span>.  With <span class="code">PerRequestSingleton</span>,
			we can do this:

			<script type="syntaxhighlighter" class="brush: csharp">
				public class AddressBookDal : PerRequestSingleton<AddressBookEntities>
				{ }
			</script>

			We create a Dal class that is our "entry point" to an always available instance of the ObjectContext, which is
			unique per user, per request (in an ASP.NET environment).  It's used like this:

			<script type="syntaxhighlighter" class="brush: csharp">
				List<Person> people = AddressBookDal.Instance.People.Where(p => p.Emails.Count() > 0).ToList();

				var q =
					from p in AddressBookDal.Instance.People.EagerLoad(p => p.Addresses)
					orderby p.Lname
					select p;
			</script>

			That's it.  <span class="code">AddressBookDal.Instance</span> will refer to the singleton instance of the AddressBookEntities
			class.  It's usable in your code behind files, in your IHttpHandlers, in your RestCake services, in your WCF services, etc. It's
			always there, and you'll never have to do this again:

			<script type="syntaxhighlighter" class="brush: csharp">
				using (AddressBookEntities context = new AddressBookEntities())
				{
					// ...
				}
			</script>

		</div>


		<br />
		<br />
		<a name="loef-example"></a>
		<div class="bar light-blue-bar">
			<div class="title"><a href="#loef-example">Let me see an example!</a></div>
			You're looking at it.  All of the <a href="../Examples.aspx">examples</a> use the <strong>AddressBook.DataAccess</strong>
			project that's part of the RestCake source download at CodePlex.  The AddressBook.DataAccess project uses the templates
			and utility classes from Loef to set up the data layer.  If you take a look at the <strong>AddressBook.Services</strong>
			project (which uses RestCake to create the REST services), there are no references to <span class="code">AddressBookEntities</span>.
			It's always <span class="code">AddressBookDal</span>, which uses the <span class="code">PerSingletonRequest</span>.  Take a look
			at those two projects, and you'll have a good idea of how easy it is to start using Loef and RestCake, and how much these little
			libraries can do for you.
			
			<br />
			<br />

			You don't have to use them together either.  They have no dependencies on one another.  If you're using
			Entity Framework, but aren't doing anything with RESTful services, grab Loef.  If you're doing RESTful services, but aren't using 
			Entity Framework, grab RestCake.  If you're using both, grab both.
		</div>

	</div>
</asp:Content>
