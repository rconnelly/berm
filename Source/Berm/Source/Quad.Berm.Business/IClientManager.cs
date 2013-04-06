namespace Quad.Berm.Business
{
    using System.Collections.Generic;

    using Quad.Berm.Data;

    public interface IClientManager
    {
        IEnumerable<ClientEntity> Enumerate();
    }
}