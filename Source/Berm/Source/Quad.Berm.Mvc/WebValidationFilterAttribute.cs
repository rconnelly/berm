namespace Quad.Berm.Mvc
{
    using System.Web.Mvc;

    using Quad.Berm.Business.Exceptions;
    using Quad.Berm.Common.Exceptions;
    using Quad.Berm.Mvc.Configuration;
    using Quad.Berm.Mvc.Data;

    // we could use Controller.OnException but I havn't found the way how to know what view is being rendered if it's not the same as action
    public class WebValidationFilterAttribute : ActionFilterAttribute
    {
        private readonly string view;

        public WebValidationFilterAttribute(string view = null)
        {
            this.view = view;
            this.Order = 10;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
            {
                return;
            }

            var transformedException = filterContext.Exception.TransformException(PolicyMetadata.WebValidationPolicy);
            var exception = transformedException as BusinessValidationException;
            if (exception != null)
            {
                // put error into ViewData for custom handling - it's not used right now
                filterContext.Controller.ViewData.Add("Error", exception.Errors);

                filterContext.Controller.ViewData.ModelState.FillFrom(exception);

                filterContext.ExceptionHandled = true;
                filterContext.Result = new ViewResult
                                            {
                                                // if view is not defined - use action name by default
                                                ViewName =
                                                    this.view
                                                    ?? filterContext.RouteData.Values["action"].ToString(),
                                                TempData = filterContext.Controller.TempData,
                                                ViewData = filterContext.Controller.ViewData
                                            };
            }
        }
    }
}