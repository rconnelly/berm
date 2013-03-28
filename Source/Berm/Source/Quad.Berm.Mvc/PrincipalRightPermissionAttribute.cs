namespace Quad.Berm.Mvc
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;

    using Quad.Berm.Common.Security;
    using Quad.Berm.Data;

    [Obsolete("Use WIF")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class PrincipalRightPermissionAttribute : AuthorizeAttribute
    {
        #region Constants and Fields

        private string[] accessRights;

        #endregion

        #region Constructors and Destructors

        public PrincipalRightPermissionAttribute(params AccessRight[] accessRights)
        {
            this.AccessRights = accessRights;
        }

        #endregion

        #region Public Properties

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "As Designed")]
        public AccessRight[] AccessRights
        {
            get
            {
                return
                    (from accessRight in this.accessRights
                     select (AccessRight)Enum.Parse(typeof(AccessRight), accessRight)).ToArray();
            }

            private set
            {
                this.accessRights =
                    (from accessRight in value ?? new AccessRight[0] select accessRight.ToString("G")).ToArray();
            }
        }

        #endregion

        #region Methods

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Contract.Assert(httpContext != null);
            return base.AuthorizeCore(httpContext) && this.HasPermissions(httpContext.User);
        }

        private bool HasPermissions(IPrincipal user)
        {
            var principal = user as IApplicationPrincipal;
            var isEmptyRightsList = this.accessRights == null || this.accessRights.Length == 0;
            var hasPermissions = isEmptyRightsList
                                 || (principal != null && this.accessRights.Any(principal.HasPermission));
            return hasPermissions;
        }

        #endregion
    }
}