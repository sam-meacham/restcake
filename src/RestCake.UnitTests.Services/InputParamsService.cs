using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestCake.UnitTests.Services
{
	/// <summary>
	/// TODO: Hmmm, do I need to have more methods, to test the different body styles as well? bare requests that take only a single arg?
	/// 
	/// In RestHttpHandler.ProcessRequest(), once the method to be called is determined, we have to get the params to pass to that method.
	/// This is done with RestHttpHandler.getMethodArgs() (a private method), which creates a List{MethodParameter}.
	/// We need to test the code that gets the parameters from the request, and creates the typed args that will be passed to the service method.
	/// This is not trivial, because we have to test all combinations of the following variables.
	/// 
	/// VERBS (2)
	/// -----------------------------
	/// GET, POST (we can ignore PUT and DELETE. The different between GET and POST is that POST can have args in the request body)
	/// 
	/// TYPES (15) (note, when string types are tested, we should use really weird casing like "tRue" for bool, to make sure that any case will work)
	/// -----------------------------
	/// int, int[], List{int} (to represent all primitive value types)
	/// enum, enum[], List{enum} (TEST AS STRINGS AND INTS!)
	/// string, string[], List{string}
	/// bool, bool[], List{bool} (TEST AS STRINGS AND INTS!)
	/// Person, UserObject[], List{UserObject}
	/// 
	/// PARAM PASSING STYLES (5)
	/// -----------------------------
	/// query string values
	/// uri segments
	/// request body (forms encoded (query string format, a=foo&b=bar))
	/// request body (json, wrapped) (string values are quoted)
	/// request body (json, bare) (string values are quoted)
	/// 
	/// Not all combinations are valid. For instance, there is no request body for GET requests, so those param passing styles are not valid.
	/// Certain types need to be tested with various representations.  Arrays in the query string can have [] or not.
	/// enums and bools can be passed as ints or strings.
	/// </summary>
	[RestService(Namespace="RestCakeUnitTests")]
	public class InputParamsService : RestHttpHandler
	{
		// So here's the whole list of all the combinations we're going to test:

		// GET, int, query string
		// GET, int, uri segments
		// GET, int[], query string
		// GET, int[], uri segments
		// GET, List<int>, query string
		// GET, List<int>, uri segments

		// GET, enum, query string
		// GET, enum, uri segments
		// GET, enum[], query string
		// GET, enum[], uri segments
		// GET, List<enum>, query string
		// GET, List<enum>, uri segments

		// GET, string, query string
		// GET, string, uri segments
		// GET, string[], query string
		// GET, string[], uri segments
		// GET, List<string>, query string
		// GET, List<string>, uri segments

		// GET, bool, query string
		// GET, bool, uri segments
		// GET, bool[], query string
		// GET, bool[], uri segments
		// GET, List<bool>, query string
		// GET, List<bool>, uri segments

		// (experimental: this requires passing a json string in the url)
		// GET, Person, query string
		// GET, Person, uri segments
		// GET, Person[], query string
		// GET, Person[], uri segments
		// GET, List<Person>, query string
		// GET, List<Person>, uri segments

		// POST, int, query string
		// POST, int, uri segment
		// POST, int, request body (forms enc)
		// POST, int, request body (json wrapped)
		// POST, int, request body (json bare)
		// POST, int[], query string
		// POST, int[], uri segment
		// POST, int[], request body (forms enc)
		// POST, int[], request body (json wrapped)
		// POST, int[], request body (json bare)
		// POST, List<int>, query string
		// POST, List<int>, uri segment
		// POST, List<int>, request body (forms enc)
		// POST, List<int>, request body (json wrapped)
		// POST, List<int>, request body (json bare)

		// POST, enum, query string
		// POST, enum, uri segment
		// POST, enum, request body (forms enc)
		// POST, enum, request body (json wrapped)
		// POST, enum, request body (json bare)
		// POST, enum[], query string
		// POST, enum[], uri segment
		// POST, enum[], request body (forms enc)
		// POST, enum[], request body (json wrapped)
		// POST, enum[], request body (json bare)
		// POST, List<enum>, query string
		// POST, List<enum>, uri segment
		// POST, List<enum>, request body (forms enc)
		// POST, List<enum>, request body (json wrapped)
		// POST, List<enum>, request body (json bare)

		// POST, string, query string
		// POST, string, uri segment
		// POST, string, request body (forms enc)
		// POST, string, request body (json wrapped)
		// POST, string, request body (json bare)
		// POST, string[], query string
		// POST, string[], uri segment
		// POST, string[], request body (forms enc)
		// POST, string[], request body (json wrapped)
		// POST, string[], request body (json bare)
		// POST, List<string>, query string
		// POST, List<string>, uri segment
		// POST, List<string>, request body (forms enc)
		// POST, List<string>, request body (json wrapped)
		// POST, List<string>, request body (json bare)

		// POST, bool, query string
		// POST, bool, uri segment
		// POST, bool, request body (forms enc)
		// POST, bool, request body (json wrapped)
		// POST, bool, request body (json bare)
		// POST, bool[], query string
		// POST, bool[], uri segment
		// POST, bool[], request body (forms enc)
		// POST, bool[], request body (json wrapped)
		// POST, bool[], request body (json bare)
		// POST, List<bool>, query string
		// POST, List<bool>, uri segment
		// POST, List<bool>, request body (forms enc)
		// POST, List<bool>, request body (json wrapped)
		// POST, List<bool>, request body (json bare)

		// POST, Person, query string
		// POST, Person, uri segment
		// POST, Person, request body (forms enc)
		// POST, Person, request body (json wrapped)
		// POST, Person, request body (json bare)
		// POST, Person[], query string
		// POST, Person[], uri segment
		// POST, Person[], request body (forms enc)
		// POST, Person[], request body (json wrapped)
		// POST, Person[], request body (json bare)
		// POST, List<Person>, query string
		// POST, List<Person>, uri segment
		// POST, List<Person>, request body (forms enc)
		// POST, List<Person>, request body (json wrapped)
		// POST, List<Person>, request body (json bare)

		// Yikes!

		// *** GET int ********************************************************************

		// GET, int, query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public int get_int_qstr(int a, int b)
		{
			return a + b;
		}

		// GET, int, uri segments
		[Get(UrlStyle = UrlStyle.UriSegments)]
		public int get_int_uriseg(int a, int b)
		{
			return a + b;
		}

		// GET, int[], query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public int[] get_intar_qstr(int[] a, int[] b)
		{
			int[] result = new int[a.Length];
			for (int i = 0; i < a.Length; i++)
				result[i] = a[i] + b[i];
			return result;
		}

		// GET, int[], uri segments
		[Get(UrlStyle = UrlStyle.UriSegments )]
		public int[] get_intar_uriseg(int[] a, int[] b)
		{
			int[] result = new int[a.Length];
			for (int i = 0; i < a.Length; i++)
				result[i] = a[i] + b[i];
			return result;
		}

		// GET, List<int>, query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public IList<int> get_intlist_qstr(IList<int> a, IList<int> b)
		{
			List<int> result = new List<int>();
			for (int i = 0; i < a.Count; i++)
				result.Add(a[i] + b[i]);
			return result;
		}

		// GET, List<int>, uri segments
		[Get(UrlStyle = UrlStyle.UriSegments)]
		public IList<int> get_intlist_uriseg(IList<int> a, IList<int> b)
		{
			List<int> result = new List<int>();
			for (int i = 0; i < a.Count; i++)
				result.Add(a[i] + b[i]);
			return result;
		}

		// *** GET enum *******************************************************************

		// GET, enum, query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public MathOp get_enum_qstr(MathOp op1)
		{
			return op1;
		}

		// GET, enum, uri segments
		[Get(UrlStyle = UrlStyle.UriSegments)]
		public MathOp get_enum_uriseg(MathOp op1)
		{
			return op1;
		}

		// GET, enum[], query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public MathOp[] get_enumar_qstr(MathOp[] ops)
		{
			return ops;
		}

		// GET, enum[], uri segments
		[Get(UrlStyle = UrlStyle.UriSegments )]
		public MathOp[] get_enumar_uriseg(MathOp[] ops)
		{
			return ops;
		}

		// GET, List<enum>, query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public IList<MathOp> get_enumlist_qstr(IList<MathOp> ops)
		{
			return ops;
		}

		// GET, List<enum>, uri segments
		[Get(UrlStyle = UrlStyle.UriSegments)]
		public IList<MathOp> get_enumlist_uriseg(IList<MathOp> ops)
		{
			return ops;
		}

		// *** GET string *****************************************************************

		// GET, string, query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public string get_string_qstr(string s)
		{
			return s;
		}

		// GET, string, uri segments
		[Get(UrlStyle = UrlStyle.UriSegments)]
		public string get_string_uriseg(string s)
		{
			return s;
		}

		// GET, string[], query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public string[] get_stringar_qstr(string[] sArr)
		{
			return sArr;
		}

		// GET, string[], uri segments
		[Get(UrlStyle = UrlStyle.UriSegments )]
		public string[] get_stringar_uriseg(string[] sArr)
		{
			return sArr;
		}

		// GET, List<string>, query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public IList<string> get_stringlist_qstr(IList<string> list)
		{
			return list;
		}

		// GET, List<string>, uri segments
		[Get(UrlStyle = UrlStyle.UriSegments)]
		public IList<string> get_stringlist_uriseg(IList<string> list)
		{
			return list;
		}

		// *** GET bool *******************************************************************

		// GET, bool, query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public bool get_bool_qstr(bool b)
		{
			return b;
		}

		// GET, bool, uri segments
		[Get(UrlStyle = UrlStyle.UriSegments)]
		public bool get_bool_uriseg(bool b)
		{
			return b;
		}

		// GET, bool[], query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public bool[] get_boolar_qstr(bool[] bools)
		{
			return bools;
		}

		// GET, bool[], uri segments
		[Get(UrlStyle = UrlStyle.UriSegments )]
		public bool[] get_boolar_uriseg(bool[] bools)
		{
			return bools;
		}

		// GET, List<bool>, query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public IList<bool> get_boollist_qstr(IList<bool> list)
		{
			return list;
		}

		// GET, List<bool>, uri segments
		[Get(UrlStyle = UrlStyle.UriSegments)]
		public IList<bool> get_boollist_uriseg(IList<bool> list)
		{
			return list;
		}

		// *** GET Person *************************************************************

		// GET, Person, query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public Person get_person_qstr(Person b)
		{
			return b;
		}

		// GET, Person, uri segments
		[Get(UrlStyle = UrlStyle.UriSegments)]
		public Person get_person_uriseg(Person b)
		{
			return b;
		}

		// GET, Person[], query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public Person[] get_personar_qstr(Person[] people)
		{
			return people;
		}

		// GET, Person[], uri segments
		[Get(UrlStyle = UrlStyle.UriSegments )]
		public Person[] get_personar_uriseg(Person[] people)
		{
			return people;
		}

		// GET, List<Person>, query string
		[Get(UrlStyle = UrlStyle.QueryString)]
		public IList<Person> get_personlist_qstr(IList<Person> list)
		{
			return list;
		}

		// GET, List<Person>, uri segments
		[Get(UrlStyle = UrlStyle.UriSegments)]
		public IList<Person> get_personlist_uriseg(IList<Person> list)
		{
			return list;
		}

		// *** POST int *******************************************************************

		// POST, int, query string
		[Post(UrlStyle = UrlStyle.QueryString, BodyStyle = BodyStyle.WrappedRequest)]
		public int post_int_qstr(int a, int b)
		{
			return a + b;
		}

		// POST, int, uri segment
		// POST, int, request body (forms enc)
		// POST, int, request body (json wrapped)
		// POST, int, request body (json bare)
		// POST, int[], query string
		// POST, int[], uri segment
		// POST, int[], request body (forms enc)
		// POST, int[], request body (json wrapped)
		// POST, int[], request body (json bare)
		// POST, List<int>, query string
		// POST, List<int>, uri segment
		// POST, List<int>, request body (forms enc)
		// POST, List<int>, request body (json wrapped)
		// POST, List<int>, request body (json bare)

		// *** POST enum ******************************************************************

		// *** POST string ****************************************************************

		// *** POST bool ******************************************************************

		// *** POST Person ************************************************************



	}
}
