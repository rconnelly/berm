namespace Quad.Berm.Data
{
    using System.Diagnostics.Contracts;
    using System.Security.Claims;
    using System.Security.Principal;

    public static class IdentityExtension
    {
        public static long GetUserId(this IIdentity identity)
        {
            Contract.Assert(identity != null);
            long userId = 0;
            var claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var claim = claimsIdentity.FindFirst(MetadataInfo.ClaimTypes.UserId);

                if (!string.IsNullOrEmpty(claim.Value))
                {
                    userId = long.Parse(claim.Value);
                }
            }

            return userId;
        }

        public static long? GetClientId(this IIdentity identity)
        {
            Contract.Assert(identity != null);
            long? clientId = null;
            var claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var claim = claimsIdentity.FindFirst(MetadataInfo.ClaimTypes.ClientId);

                if (claim != null && !string.IsNullOrEmpty(claim.Value))
                {
                    clientId = long.Parse(claim.Value);
                }
            }

            return clientId;
        }
    }
}