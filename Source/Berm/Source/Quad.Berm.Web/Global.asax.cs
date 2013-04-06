namespace Quad.Berm.Web
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.IdentityModel.Services;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using Microsoft.WindowsAzure.Diagnostics;
    using Microsoft.WindowsAzure.ServiceRuntime;

    using global::Mvc.JQuery.Datatables;

    using Newtonsoft.Json.Serialization;

    using Quad.Berm.Common.Exceptions;
    using Quad.Berm.Common.Unity;
    using Quad.Berm.Mvc.Configuration;
    using Quad.Berm.Web.Areas.Main.Controllers;
    using Quad.Berm.Web.Mvc;
    using Quad.Berm.Web.Mvc.Helpers;

    public class MvcApplication : HttpApplication
    {
        #region Fields

        private const string AmbientContextKey = "ac";

        #endregion

        #region Properties

        private bool IsJsonRequest
        {
            get
            {
                var isjson =
                    (this.Request.AcceptTypes ?? new string[0]).Any(
                        t => string.Equals(t, "application/json", StringComparison.OrdinalIgnoreCase));
                return isjson;
            }
        }

        #endregion

        #region Methods

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "As Designed")]
        protected void Application_Start()
        {
            // DiagnosticMonitorTraceListener should be added before any other actions
            // to log all necessary info to azure log table
            if (RoleEnvironment.IsAvailable)
            {
                Trace.Listeners.Add(new DiagnosticMonitorTraceListener());
            }

            Shell.Start<MvcContainerExtension>();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MvcHandler.DisableMvcResponseHeader = true;

            ModelBinders.Binders.Add(typeof(DataTablesParam), new DataTablesModelBinder());

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            
            FederatedAuthentication.WSFederationAuthenticationModule.SessionSecurityTokenCreated += this.WSFederationAuthenticationModule_SessionSecurityTokenCreated;
            FederatedAuthentication.SessionAuthenticationModule.SessionSecurityTokenReceived += this.SessionAuthenticationModule_SessionSecurityTokenReceived;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            this.Context.Items.Add(AmbientContextKey, new AmbientContext());
        }

        protected void WSFederationAuthenticationModule_SessionSecurityTokenCreated(object sender, SessionSecurityTokenCreatedEventArgs e)
        {
            // uncomment if you want to decrease cookie size
            // after uncommenting you should implement distributed sessionSecurityTokenCache (http://msdn.microsoft.com/en-us/library/hh545457.aspx)
            // e.SessionToken.IsReferenceMode = true;
        }

        protected void SessionAuthenticationModule_SessionSecurityTokenReceived(object sender, SessionSecurityTokenReceivedEventArgs e)
        {
            // sliding session implementation (based on http://www.cloudidentity.com/blog/2010/06/16/WARNING-SLIDING-SESSIONS-ARE-CLOSER-THAN-THEY-APPEAR/)
            var now = DateTime.UtcNow; 
            var validFrom = e.SessionToken.ValidFrom.ToUniversalTime();
            var validTo = e.SessionToken.ValidTo.ToUniversalTime();
            var timeout = validTo - validFrom;
            var window = timeout.TotalMinutes / 2;
            
            // do not want to set cookie each time
            if (now < validTo && now > validFrom.AddMinutes(window))
            {
                var sam = (SessionAuthenticationModule)sender;
                validFrom = now;
                validTo = validFrom.Add(timeout);
                var isReferenceMode = e.SessionToken.IsReferenceMode;
                e.SessionToken = sam.CreateSessionSecurityToken(
                    e.SessionToken.ClaimsPrincipal,
                    e.SessionToken.Context,
                    validFrom,
                    validTo, 
                    e.SessionToken.IsPersistent);
                e.SessionToken.IsReferenceMode = isReferenceMode;
                e.ReissueCookie = true; 
            } 
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (this.Response.StatusCode == 302 && this.Response.RedirectLocation != null && this.IsJsonRequest)
            {
                this.Response.Clear();
                this.Response.StatusCode = 401;
            }
            else if (StatusHasErrorPage(this.Response.StatusCode))
            {
                this.ShowSpecialErrorPage(this.Response.StatusCode);
            }

            var context = (AmbientContext)this.Context.Items[AmbientContextKey];
            Contract.Assert(context != null);
            context.Dispose();
            this.Context.Items.Remove(AmbientContextKey);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "As Designed")]
        protected void Application_Error()
        {
            var error = this.Server.GetLastError();
            var transformException = error.TransformException(MvcContainerExtension.DefaultPolicy);
            var httpException = transformException as HttpException;

            if (httpException != null && StatusHasErrorPage(httpException.GetHttpCode()))
            {
                this.ShowSpecialErrorPage(httpException.GetHttpCode());
            }
            else if (this.Context.IsCustomErrorEnabled)
            {
                this.ShowUnhandledErrorPage(transformException);
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "As Designed")]
        protected void Application_End()
        {
            Shell.Shutdown();
        }

        private static bool StatusHasErrorPage(int status)
        {
            // want to status != 500 be implicitly present here
            return (status != 500) && (status == 404 || status == 403 || status == 401);
        }

        private void ShowSpecialErrorPage(int status)
        {
            const string Key = "shown";

            if (!this.Context.Items.Contains(Key))
            {
                var isjson = this.IsJsonRequest;

                if (isjson)
                {
                    this.ShowJsonErrorPage(status);
                }
                else
                {
                    this.ShowErrorPage(
                        status,
                        (controller, context, routeData) =>
                            {
                                var requestContext = new RequestContext(context, routeData);
                                ((IController)controller).Execute(requestContext);
                            });
                }

                this.Context.Items.Add(Key, 1);
            }
        }

        private void ShowUnhandledErrorPage(Exception transformException)
        {
            // it is small duplication with WebHandleErrorAttribute. 
            // the only difference WebHandleErrorAttribute knows controller and action for sure,
            // while here we will use "undefined"
            this.ShowErrorPage(
                500, 
                (controller, context, routeData) =>
                {
                    // ReSharper disable Mvc.ControllerNotResolved
                    // ReSharper disable Mvc.ActionNotResolved
                    var model = new HandleErrorInfo(transformException, "undefined", "undefined");
                    // ReSharper restore Mvc.ActionNotResolved
                    // ReSharper restore Mvc.ControllerNotResolved
                    var vr = new ViewResult
                                 {
                                     ViewName = "Error", 
                                     ViewData = new ViewDataDictionary(model)
                                 };
                    var controllerContext = new ControllerContext(context, routeData, controller);
                    vr.ExecuteResult(controllerContext);
                });
        }

        private void ShowErrorPage(int status, Action<ControllerBase, HttpContextBase, RouteData> fill)
        {
            this.Server.ClearError();
            this.Response.Clear();
            this.Response.StatusCode = status;

            // Avoid IIS7 getting in the middle
            this.Response.TrySkipIisCustomErrors = true;

            using (var errorController = new ErrorController())
            {
                var httpContext = new HttpContextWrapper(this.Context);
                var routeData = RouteHelper.CreateErrorRoute(status);
                
                fill(errorController, httpContext, routeData);
            }
        }

        private void ShowJsonErrorPage(int status)
        {
            this.Server.ClearError();
            this.Response.StatusCode = status;

            // Avoid IIS7 getting in the middle
            this.Response.TrySkipIisCustomErrors = true;
        }

        #endregion
    }
}