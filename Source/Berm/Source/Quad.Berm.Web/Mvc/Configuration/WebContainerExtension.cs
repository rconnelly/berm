namespace Quad.Berm.Web.Mvc.Configuration
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using Quad.Berm.Mvc;
    using Quad.Berm.Mvc.Configuration;
    using Quad.Berm.Web.Areas.Main.Controllers;
    using Quad.Berm.Web.Mvc.Helpers;

    public class WebContainerExtension : MvcContainerExtension
    {
        protected override IControllerFactory CreateControllerFactory()
        {
            return new AppControllerFactory();
        }

        private class AppControllerFactory : CustomControllerFactory
        {
            protected override IController HandleControllerNotFound(RequestContext requestContext)
            {
                var controller = this.GetControllerInstance(requestContext, typeof(ErrorController));
                requestContext.RouteData.Values.Clear();
                RouteHelper.InitErrorRoute(404, requestContext.RouteData);
                return controller;
            }
        }
    }
}
