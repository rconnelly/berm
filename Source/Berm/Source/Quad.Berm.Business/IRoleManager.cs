namespace Quad.Berm.Business
{
    using System.Collections.Generic;

    using Quad.Berm.Data;

    public interface IRoleManager
    {
        IEnumerable<RoleEntity> Enumerate(RoleSet option);

        RoleEntity Get(long roleId);
    }
}