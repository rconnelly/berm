namespace Quad.Berm.Mvc
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class CacheAttribute : OutputCacheAttribute
    {
        #region Constants and Fields

        private OutputCacheLocation? originalLocation;

        #endregion

        #region Constructors and Destructors

        public CacheAttribute()
        {
            this.Location = OutputCacheLocation.Any;
            this.Duration = 300; /*default cache time*/
            this.DisableForAuthenticatedUser = false;
        }

        #endregion

        #region Properties

        public bool DisableForAuthenticatedUser { get; set; }

        #endregion

        #region Public Methods and Operators

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            this.OnCacheEnabled(filterContext);
            base.OnResultExecuting(filterContext);
        }

        #endregion

        #region Methods

        // This method is called each time when cached page is going to be
        // served and ensures that cache is ignored for authenticated users.
        private static void IgnoreAuthenticated(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = context.User.Identity.IsAuthenticated
                                   ? HttpValidationStatus.IgnoreThisRequest
                                   : HttpValidationStatus.Valid;
        }

        private void OnCacheEnabled(ControllerContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            if (this.IsCacheDisabled(httpContext))
            {
                // it's crucial not to cache Authenticated content
                this.originalLocation = this.originalLocation ?? this.Location;
                this.Location = OutputCacheLocation.None;
            }
            else
            {
                this.Location = this.originalLocation ?? this.Location;
            }

            if (this.DisableForAuthenticatedUser)
            {
                // this smells a little but it works
                httpContext.Response.Cache.AddValidationCallback(IgnoreAuthenticated, null);
            }
        }

        private bool IsCacheDisabled(HttpContextBase httpContext)
        {
            return 
                httpContext.IsDebuggingEnabled || 
                (this.DisableForAuthenticatedUser && httpContext.User.Identity.IsAuthenticated);
        }

        #endregion
    }
}