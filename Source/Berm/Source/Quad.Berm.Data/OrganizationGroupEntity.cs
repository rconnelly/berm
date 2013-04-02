namespace Quad.Berm.Data
{
    using System.Collections.Generic;

    using Microsoft.SqlServer.Types;

    public class OrganizationGroupEntity : BaseEntity
    {
        public virtual SqlHierarchyId Hierarchy { get; set; }

        public virtual ICollection<RoleEntity> Roles { get; set; }

        public virtual string Name { get; set; }
    }
}