using System.Web;
using System.Web.Routing;

namespace RestCake.Routing
{
	/// <summary>
	/// This class just adds some route data properties to an IHttpHandler, so that an implementing handler
	/// can be aware of the route url that was used.
	/// </summary>
	public abstract class RoutedHttpHandler : IHttpHandler
	{
		public RouteBase Route { get; internal set; }
		public string BaseUrl { get; internal set; }

		public abstract void ProcessRequest(HttpContext context);
		public abstract bool IsReusable { get; }
	}
}
