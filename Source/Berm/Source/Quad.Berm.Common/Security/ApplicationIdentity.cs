namespace Quad.Berm.Common.Security
{
    using System.Diagnostics.Contracts;
    using System.Security.Claims;

    public class ApplicationIdentity : IApplicationIdentity
    {
        #region Fields

        private readonly ClaimsIdentity identity;

        #endregion

        #region Constructors and Destructors

        public ApplicationIdentity(ClaimsIdentity identity)
        {
            Contract.Assert(identity != null);
            this.identity = identity;
        }

        #endregion

        #region Public Properties

        public string AuthenticationType
        {
            get
            {
                return this.identity.AuthenticationType;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return this.identity.IsAuthenticated;
            }
        }

        public long UserId
        {
            get
            {
                var claim = this.identity.FindFirst(ClaimTypes.UserId);
                long userId = 0;
                if (!string.IsNullOrEmpty(claim.Value))
                {
                    userId = long.Parse(claim.Value);
                }

                return userId;
            }
        }

        public long? ClientId
        {
            get
            {
                var claim = this.identity.FindFirst(ClaimTypes.ClientId);
                long? clientId = null;
                if (!string.IsNullOrEmpty(claim.Value))
                {
                    clientId = long.Parse(claim.Value);
                }

                return clientId;
            }
        }

        public string Name
        {
            get
            {
                return this.identity.Name;
            }
        }

        #endregion
    }
}
