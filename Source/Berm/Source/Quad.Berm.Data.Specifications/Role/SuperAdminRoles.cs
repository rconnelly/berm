namespace Quad.Berm.Data.Specifications.Role
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class SuperAdminRoles : AllRoles
    {
        public override Expression<Func<RoleEntity, bool>> IsSatisfiedBy()
        {
            return m => m.Client == null && m.Permissions.Any(p => p.Name == AccessRight.ManageSuperAdmin);
        }
    }
}