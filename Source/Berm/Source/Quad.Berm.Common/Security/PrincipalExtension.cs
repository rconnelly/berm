namespace Quad.Berm.Common.Security
{
    using System.Diagnostics.Contracts;
    using System.Security.Claims;
    using System.Security.Principal;

    public static class PrincipalExtension
    {
        public static bool HasPermission(this IPrincipal principal, string permission)
        {
            Contract.Assert(principal != null);

            var hasPermission = false;
            var claimsIdentity = principal.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                hasPermission = claimsIdentity.HasClaim(permission, "1");
            }

            return hasPermission;
        }
    }
}