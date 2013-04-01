namespace Quad.Berm.Mvc.Security
{
    using System.Linq;
    using System.Security.Claims;

    public class CustomClaimsAuthenticationManager : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            var principal = incomingPrincipal;
            if (incomingPrincipal != null)
            {
                principal = IsKnownPrincipal(incomingPrincipal)
                                    ? CreateAuthenticatedPrincipal(incomingPrincipal)
                                    : CreateUnauthenticatedPrincipal();
            }

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
            var role = GetRole(incomingPrincipal);

            var identity = (ClaimsIdentity)incomingPrincipal.Identity;
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
            return incomingPrincipal;
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

        private static ClaimsPrincipal CreateUnauthenticatedPrincipal()
        {
            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}