namespace Quad.Berm.Mvc
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;

    using Quad.Berm.Common.Exceptions;
    using Quad.Berm.Mvc.Configuration;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class WebHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Contract.Assert(filterContext != null);
            if (filterContext.IsChildAction)
            {
                return;
            }

            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            var exception = filterContext.Exception;
            if (!this.ExceptionType.IsInstanceOfType(exception))
            {
                return;
            }

            var handledException = exception.TransformException(MvcContainerExtension.DefaultPolicy);

            // avoid showing death screen even for not 500 error in production
            var httpCode = new HttpException(null, exception).GetHttpCode();
            switch (httpCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    {
                        filterContext.Exception = handledException;
                        base.OnException(filterContext);
                        break;
                    }

                default:
                    {
                        var controllerName = (string)filterContext.RouteData.Values["controller"];
                        var actionName = (string)filterContext.RouteData.Values["action"];
                        var model = new HandleErrorInfo(handledException, controllerName, actionName);
                        filterContext.Result = new ViewResult
                            {
                                // if view is not defined - use action name by default
                                ViewName = "Error",
                                ViewData = new ViewDataDictionary(model)
                            };
                        filterContext.ExceptionHandled = true;
                        break;
                    }
            }
        }
    }
}