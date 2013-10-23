using System.Web;
using System.Web.Routing;

namespace RestCake.Routing
{
	public class GenericHandlerRouteHandler<T> : IRouteHandler where T : RoutedHttpHandler, new()
	{
		private readonly RouteBase m_route;
		private readonly string m_baseUrl;

		public GenericHandlerRouteHandler(RouteBase route, string baseUrl)
		{
			m_route = route;
			m_baseUrl = baseUrl;
		}

		public IHttpHandler GetHttpHandler(RequestContext requestContext)
		{
			return new T() { Route = m_route, BaseUrl = m_baseUrl };
		}
	}
}
