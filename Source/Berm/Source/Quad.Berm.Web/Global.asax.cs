namespace Quad.Berm.Web
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.Caching;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;

    using global::Common.Logging;

    using Microsoft.Practices.ServiceLocation;

    using Newtonsoft.Json.Serialization;

    using Quad.Berm.Common.Security;
    using Quad.Berm.Common.Unity;
    using Quad.Berm.Mvc.Data;
    using Quad.Berm.Web.Areas.Main.Controllers;
    using Quad.Berm.Web.Mvc;
    using Quad.Berm.Web.Mvc.Configuration;
    using Quad.Berm.Web.Mvc.Helpers;

    public class MvcApplication : HttpApplication
    {
        #region Fields

        private const string AmbientContextKey = "ac";

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        #endregion

        #region Methods

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "As Designed")]
        protected void Application_Start()
        {
            Shell.Start<WebContainerExtension>();

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // ModelBinders.Binders.Add(typeof(DataTablesParam), new DataTablesModelBinder());
            MvcHandler.DisableMvcResponseHeader = true;
            
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            this.Context.Items.Add(AmbientContextKey, new AmbientContext());
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            return;
            this.Context.User = GetPrincipal();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var uow = (AmbientContext)this.Context.Items[AmbientContextKey];
            Contract.Assert(uow != null);
            uow.Dispose();
            this.Context.Items.Remove(AmbientContextKey);
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "As Designed")]
        protected void Application_Error()
        {
            var error = this.Server.GetLastError();
            try
            {
                Log.ErrorFormat(
                    CultureInfo.InvariantCulture, 
                    "Server error has been occured while processing page: {0} ", 
                    error,
                    HttpContext.Current != null ? HttpContext.Current.Request.Url.ToString() : "unknown");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            this.HandleCustomErrors(error);
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "As Designed")]
        protected void Application_End()
        {
            Shell.Shutdown();
        }

        private static IPrincipal GetPrincipal()
        {
            IPrincipal principal = ApplicationPrincipal.Anonymous;
            var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null && !authTicket.Expired)
                {
                    var login = authTicket.Name;
                    var cache = ServiceLocator.Current.GetInstance<ObjectCache>();

                    var session = cache.Get(login) as PrincipalSession;
                    if (session == null)
                    {
                        throw new NotImplementedException();
                        /*var manager = ServiceLocator.Current.GetInstance<IUserManager>();
                        var user = manager.FindByLogin(login);

                        if (user != null && user.UserPasswordCredential != null)
                        {
                            session = user.Convert();
                            cache.Add(
                                login, 
                                session, 
                                new CacheItemPolicy { SlidingExpiration = new TimeSpan(0, 0, 60) });
                        }*/
                    }

                    if (session != null)
                    {
                        principal = session.Convert();
                    }
                }
            }

            return principal;
        }

        private void HandleCustomErrors(Exception exception)
        {
            var httpException = exception as HttpException;

            if (httpException != null)
            {
                var status = httpException.GetHttpCode();

                if (status == 404 || status == 403 || status == 401)
                {
                    // Clear the error on server.
                    this.Server.ClearError();

                    // Avoid IIS7 getting in the middle
                    this.Response.StatusCode = status;
                    this.Response.TrySkipIisCustomErrors = true;

                    using (var errorController = new ErrorController())
                    {
                        var httpContext = new HttpContextWrapper(this.Context);
                        var routeData = RouteHelper.CreateErrorRoute(status);
                        var requestContext = new RequestContext(httpContext, routeData);
                        ((IController)errorController).Execute(requestContext);
                    }
                }
            }
        }

        #endregion
    }
}