namespace Quad.Berm.Data
{
    using System.Collections.Generic;

    public class ClientEntity : BaseEntity
    {
        public virtual ICollection<RoleEntity> Roles { get; set; }

        public virtual OrganizationGroupEntity OrganizationGroup { get; set; }

        public virtual string Name { get; set; }

        public virtual bool Disabled { get; set; }
    }
}