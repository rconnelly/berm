namespace Quad.Berm.Mvc.Security
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    public class CustomClaimsAuthenticationManager : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            Contract.Assert(incomingPrincipal != null);
            var principal = IsKnownPrincipal(incomingPrincipal) 
                ? CreateAuthenticatedPrincipal(incomingPrincipal) 
                : CreateUnauthenticatedPrincipal(incomingPrincipal);

            return principal;
        }

        private static bool IsKnownPrincipal(ClaimsPrincipal incomingPrincipal)
        {
            // todo: check user exist in database
            var known =
                incomingPrincipal.Claims.Any(
                    c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
                    && c.Value == "dmitriy@quad.io");
            return known;
        }

        private static ClaimsPrincipal CreateAuthenticatedPrincipal(ClaimsPrincipal incomingPrincipal)
        {
            var claims = incomingPrincipal.Claims.ToList();
            var role = GetRole(incomingPrincipal);
            claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
            var identity = new ClaimsIdentity(claims, "Federated");
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }

        private static string GetRole(ClaimsPrincipal incomingPrincipal)
        {
            // todo: read role from database
            var role = "User";
            if (
                incomingPrincipal.Claims.Any(
                    c =>
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
                    && c.Value == "dmitriy@quad.io"))
            {
                role = "Admin";
            }

            return role;
        }

        private static ClaimsPrincipal CreateUnauthenticatedPrincipal(ClaimsPrincipal incomingPrincipal)
        {
            var identity = new ClaimsIdentity(incomingPrincipal.Claims);
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}