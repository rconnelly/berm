namespace Quad.Berm.Mvc
{
    using System.Net;
    using System.Web.Mvc;

    using Quad.Berm.Business.Exceptions;
    using Quad.Berm.Common.Exceptions;
    using Quad.Berm.Mvc.Configuration;
    using Quad.Berm.Mvc.Data;

    public class ApiValidationFilterAttribute : ActionFilterAttribute
    {
        public ApiValidationFilterAttribute()
        {
            this.Order = 10;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception == null)
            {
                return;
            }

            var transformedException = filterContext.Exception.TransformException(PolicyMetadata.ApiValidationPolicy);

            HttpStatusCode statusCode;
            OperationError operationError;

            var exception = transformedException as BusinessValidationException;
            if (exception != null)
            {
                operationError = new OperationError();
                operationError.Fill(exception);
                
                statusCode = HttpStatusCode.Conflict;
            }
            else
            {
                var message = (transformedException ?? filterContext.Exception).Message;
                operationError = new OperationError(HttpStatusCode.InternalServerError.ToString(), message);

                statusCode = HttpStatusCode.InternalServerError;
            }

            filterContext.Result = new JsonResult { Data = operationError, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = (int)statusCode;
        }
    }
}