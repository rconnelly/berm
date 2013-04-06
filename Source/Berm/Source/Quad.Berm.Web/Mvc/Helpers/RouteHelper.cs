namespace Quad.Berm.Web.Mvc.Helpers
{
    using System.Diagnostics.Contracts;
    using System.Web.Routing;

    public static class RouteHelper
    {
        public const string Root = "Root";
        public const string Error404 = "Error404";
        public const string Error403 = "Error403";
        public const string Error401 = "Error401";

        public static RouteData CreateErrorRoute(int httpCode)
        {
            var routeData = new RouteData();
            InitErrorRoute(httpCode, routeData);
            return routeData;
        }
        
        public static void InitErrorRoute(int httpCode, RouteData routeData)
        {
            Contract.Assert(routeData != null);

            routeData.Values.Add("area", "Main");
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "Error" + httpCode);
        }
    }
}