namespace Quad.Berm.Data
{
    using System.Collections.Generic;

    public class RoleEntity : BaseEntity
    {
        public virtual ICollection<PermissionEntity> Permissions { get; set; } 

        public virtual ClientEntity Client { get; set; }

        public virtual ICollection<OrganizationGroupEntity> OrganizationGroup { get; set; }

        public virtual string Name { get; set; }
    }
}