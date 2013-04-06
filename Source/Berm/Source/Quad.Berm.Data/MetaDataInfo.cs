namespace Quad.Berm.Data
{
    using System.Globalization;

    public static class MetadataInfo
    {
        public const int StringNormal = 255;

        public const string AmbientContextCache = "AmbientContext";

        public const string DefaultIdentityProvider = "Google";

        public static class ClaimTypes
        {
            public const string IdentityProvider = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/IdentityProvider";

            public const string Identity = "http://schemas.quad.io/berm/2013/02/claims/Identity";

            public const string ClientId = "http://schemas.quad.io/berm/2013/02/claims/ClientId";

            public const string UserId = "http://schemas.quad.io/berm/2013/02/claims/UserId";

            public const string AccessRightRoot = "http://schemas.quad.io/berm/2013/02/claims/AccessRights/";

            public static string ToClaim(AccessRight value)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}{1}", AccessRightRoot, value);
            }
        }
    }
}