namespace Quad.Berm.Mvc.Security
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.Practices.ServiceLocation;

    using Quad.Berm.Business;
    using Quad.Berm.Data;

    public class CustomClaimsAuthenticationManager : ClaimsAuthenticationManager
    {
        private static IUserManager UserManager
        {
            get
            {
                return ServiceLocator.Current.GetInstance<IUserManager>();
            }
        }

        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            var principal = incomingPrincipal;
            if (incomingPrincipal != null)
            {
                var claims = GetUserClaims(incomingPrincipal);
                principal = claims.Any()
                                    ? CreateAuthenticatedPrincipal(incomingPrincipal, claims)
                                    : CreateUnauthenticatedPrincipal();
            }

            return principal;
        }

        private static IList<Claim> GetUserClaims(ClaimsPrincipal incomingPrincipal)
        {
            var provider = incomingPrincipal.FindFirst(MetadataInfo.ClaimTypes.IdentityProvider);
            var email = incomingPrincipal.FindFirst(ClaimTypes.Email);

            IList<Claim> claims = null;
            if (provider != null && email != null && provider.Value != null && email.Value != null)
            {
                var user = UserManager.FindActive(provider.Value, email.Value);
                if (user != null)
                {
                    claims = user.ToClaims().ToList();
                }
            }

            return claims ?? new List<Claim>();
        }

        private static ClaimsPrincipal CreateAuthenticatedPrincipal(ClaimsPrincipal incomingPrincipal, IEnumerable<Claim> claims)
        {
            Contract.Assert(claims != null);
            
            var identity = (ClaimsIdentity)incomingPrincipal.Identity;
            identity.AddClaims(claims);
            
            return incomingPrincipal;
        }

        private static ClaimsPrincipal CreateUnauthenticatedPrincipal()
        {
            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}