namespace Quad.Berm.Common.Security
{
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading;

    public class ThreadApplicationPrincipal : IApplicationPrincipal
    {
        #region Properties

        public ClaimsIdentity Identity
        {
            get
            {
                var identity = CurrentPrincipal.Identity as ClaimsIdentity ?? new ClaimsIdentity();
                return identity;
            }
        }

        IIdentity IPrincipal.Identity
        {
            get
            {
                return this.Identity;
            }
        }

        private static ClaimsPrincipal CurrentPrincipal
        {
            get
            {
                var principal = Thread.CurrentPrincipal as ClaimsPrincipal;
                return principal ?? new ClaimsPrincipal();
            }
        }

        #endregion

        #region Public Methods and Operators

        public bool IsInRole(string role)
        {
            return CurrentPrincipal.IsInRole(role);
        }

        public bool HasPermission(string permission)
        {
            var hasPermission = this.Identity.HasClaim(permission, "1");
            return hasPermission;
        }

        #endregion
    }
}