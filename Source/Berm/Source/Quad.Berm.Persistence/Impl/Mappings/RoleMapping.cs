namespace Quad.Berm.Persistence.Impl.Mappings
{
    using FluentNHibernate.Automapping;
    using FluentNHibernate.Automapping.Alterations;

    using Quad.Berm.Data;

    public class RoleMapping : IAutoMappingOverride<RoleEntity>
    {
        public void Override(AutoMapping<RoleEntity> mapping)
        {
            mapping.HasManyToMany(m => m.Permissions)
                .Table("PermissionRole")
                .ChildKeyColumn("PermissionId")
                .ParentKeyColumn("RoleId")
                .LazyLoad();

            mapping.HasManyToMany(m => m.OrganizationGroup)
                .Table("OrganizationGroupRole")
                .ChildKeyColumn("OrganizationGroupId")
                .ParentKeyColumn("RoleId")
                .LazyLoad();
        }
    }
}