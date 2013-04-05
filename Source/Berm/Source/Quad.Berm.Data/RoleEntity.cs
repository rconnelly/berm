namespace Quad.Berm.Data
{
    using System.Collections.Generic;
    using System.Linq;

    public class RoleEntity : BaseEntity
    {
        public virtual ICollection<PermissionEntity> Permissions { get; set; } 

        public virtual ClientEntity Client { get; set; }

        public virtual ICollection<OrganizationGroupEntity> OrganizationGroup { get; set; }

        public virtual string Name { get; set; }

        public virtual bool HasAny(params AccessRight[] rights)
        {
            return this.Permissions.Select(p => p.Name).Any(rights.Contains);
        }
    }
}