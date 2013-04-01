namespace Quad.Berm.Web.Areas.Main
{
    using System.Web.Mvc;

    using Quad.Berm.Web.Mvc.Helpers;

    public class MainAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Main"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(RouteHelper.Root, string.Empty, new { controller = "Home", action = "Index" });
            context.MapRoute(RouteHelper.Error404, "Error404", new { controller = "Error", action = "Error404" });
            context.MapRoute(RouteHelper.Error403, "Error403", new { controller = "Error", action = "Error403" });
            context.MapRoute(RouteHelper.Error401, "Error401", new { controller = "Error", action = "Error401" });

            context.MapRoute(
                "Main_default",
                "main/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional });
        }
    }
}
