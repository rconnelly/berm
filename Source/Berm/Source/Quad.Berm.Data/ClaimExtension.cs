namespace Quad.Berm.Data
{
    using System.Diagnostics.Contracts;
    using System.Security.Claims;

    public static class ClaimExtension
    {
        public static long GetUserId(this ClaimsIdentity identity)
        {
            Contract.Assert(identity != null);
            var claim = identity.FindFirst(MetadataInfo.ClaimTypes.UserId);
            long userId = 0;
            if (!string.IsNullOrEmpty(claim.Value))
            {
                userId = long.Parse(claim.Value);
            }

            return userId;
        }

        public static long? GetClientId(this ClaimsIdentity identity)
        {
            Contract.Assert(identity != null);
            var claim = identity.FindFirst(MetadataInfo.ClaimTypes.ClientId);
            long? clientId = null;
            if (claim != null && !string.IsNullOrEmpty(claim.Value))
            {
                clientId = long.Parse(claim.Value);
            }

            return clientId;
        }
    }
}