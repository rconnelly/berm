namespace Quad.Berm.Common.Security
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;

    public class ApplicationPrincipal : IApplicationPrincipal
    {
        #region Constants and Fields

        private readonly ClaimsPrincipal principal;

        #endregion

        #region Constructors and Destructors

        public ApplicationPrincipal(ClaimsPrincipal principal)
        {
            Contract.Assert(principal != null);
            this.principal = principal;
            this.Identity = new ApplicationIdentity(principal.Identities.FirstOrDefault() ?? new ClaimsIdentity());
        }

        #endregion

        #region Public Properties

        public IApplicationIdentity Identity { get; private set; }

        #endregion

        #region Explicit Interface Properties

        IIdentity IPrincipal.Identity
        {
            get
            {
                return this.Identity;
            }
        }

        #endregion

        #region Public Methods and Operators

        public bool HasPermission(string permission)
        {
            Contract.Assert(!string.IsNullOrEmpty(permission));
            var hasPermission = this.principal.Identities.Any(i => i.HasClaim(permission, "1"));
            return hasPermission;
        }

        public bool IsInRole(string role)
        {
            Contract.Assert(!string.IsNullOrEmpty(role));
            var isInRole = this.principal.IsInRole(role);
            return isInRole;
        }

        #endregion
    }
}