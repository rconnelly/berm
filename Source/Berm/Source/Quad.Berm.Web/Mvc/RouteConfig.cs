namespace Quad.Berm.Web.Mvc
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // bind other to Error404
            routes.MapRoute("All", "{*catchall}", new { area = "Main", controller = "Error", action = "Error404" });
        }
    }
}