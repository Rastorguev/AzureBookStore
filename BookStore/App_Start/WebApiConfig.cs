using System.Web.Http;
using JetBrains.Annotations;

namespace BookStore
{
    public static class WebApiConfig
    {
        public static void Register([NotNull]HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}