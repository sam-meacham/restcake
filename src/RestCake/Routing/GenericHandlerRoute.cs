using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace RestCake.Routing
{
	/// <summary>
	/// For info on subclassing RouteBase, check Pro Asp.NET MVC Framework, page 252.
	/// Google books link: http://books.google.com/books?id=tD3FfFcnJxYC&pg=PA251&lpg=PA251&dq=.net+RouteBase&source=bl&ots=IQhFwmGOVw&sig=0TgcFFgWyFRVpXgfGY1dIUc0VX4&hl=en&ei=z61UTMKwF4aWsgPHs7XbAg&sa=X&oi=book_result&ct=result&resnum=6&ved=0CC4Q6AEwBQ#v=onepage&q=.net%20RouteBase&f=false
	/// 
	/// It explains how the asp.net runtime will call GetRouteData() for every route in the route table.
	/// GetRouteData() is used for inbound url matching, and should return null for a negative match (the current requests url doesn't match the route).
	/// If it does match, it returns a RouteData object describing the handler that should be used for that request, along with any data values (stored in RouteData.Values) that
	/// that handler might be interested in.
	/// 
	/// The book also explains that GetVirtualPath() (used for outbound url generation) is called for each route in the route table, but that is not my experience,
	/// as mine used to simply throw a NotImplementedException, and that never caused a problem for me.  In my case, I don't need to do outbound url generation,
	/// so I don't have to worry about it in any case.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GenericHandlerRoute<T> : RouteBase where T : RoutedHttpHandler, new()
	{
		private readonly string m_routeUrl;
		private readonly Regex m_routeRegex;

		public string RouteUrl
		{
			get { return m_routeUrl; }
		}


		public GenericHandlerRoute(string routeUrl)
		{
			m_routeUrl = routeUrl;

			// Beginning of string followed by routeUrl followed by: (end of string OR literal forward slash)
			m_routeRegex = new Regex(@"^" + m_routeUrl + "($|/)");
		}


		public override RouteData GetRouteData(HttpContextBase httpContext)
		{
			// See if the current request matches this route's url

			// Get the relative execution path, minus the "~/" prefix (this is what will match the route url)
			string relUrl = httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2);
			if (!m_routeRegex.IsMatch(relUrl))
				return null;

			string baseUrl = httpContext.Request.CurrentExecutionFilePath;
			int ix = baseUrl.IndexOf(m_routeUrl, StringComparison.Ordinal);
			baseUrl = baseUrl.Substring(0, ix + m_routeUrl.Length);

			GenericHandlerRouteHandler<T> routeHandler = new GenericHandlerRouteHandler<T>(this, baseUrl);
			RouteData rdata = new RouteData(this, routeHandler);

			return rdata;
		}


		public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
		{
			// This route entry doesn't generate outbound Urls.
			return null;
		}
	}
}