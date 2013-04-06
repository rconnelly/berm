namespace Quad.Berm.Business.Impl
{
    using System.Collections.Generic;
    using System.Security.Principal;

    using Microsoft.Practices.Unity;

    using Quad.Berm.Data;
    using Quad.Berm.Data.Specifications.Client;

    internal class ClientManager : ManagerBase<ClientEntity>, IClientManager
    {
        #region Public Properties

        [Dependency]
        public IPrincipal Principal { get; set; }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<ClientEntity> Enumerate()
        {
            IEnumerable<ClientEntity> result;
            if (this.Principal.HasPermission(AccessRight.ManageClients))
            {
                result = this.Repository.Enumerable(Clients.All());
            }
            else
            {
                var clientId = this.Principal.Identity.GetClientId();
                var client = clientId == null ? null : this.Repository.Get<ClientEntity>(clientId.Value);
                result = client == null ? new ClientEntity[0] : new[] { client };
            }

            return result;
        }

        #endregion
    }
}