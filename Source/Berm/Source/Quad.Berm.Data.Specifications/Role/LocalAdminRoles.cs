namespace Quad.Berm.Data.Specifications.Role
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class LocalAdminRoles : AllRoles
    {
        public long ClientId { get; set; }

        public override Expression<Func<RoleEntity, bool>> IsSatisfiedBy()
        {
            return m => 
                   m.Client != null
                   && (this.ClientId == 0 || m.Client.Id == this.ClientId)
                   && m.Permissions.Any(p => p.Name == AccessRight.ManageLocalAdmin);
        }
    }
}