namespace Quad.Berm.Persistence.Impl.Mappings
{
    using FluentNHibernate.Automapping;
    using FluentNHibernate.Automapping.Alterations;

    using Quad.Berm.Data;

    public class OrganizationGroupMapping : IAutoMappingOverride<OrganizationGroupEntity>
    {
        public void Override(AutoMapping<OrganizationGroupEntity> mapping)
        {
            mapping.Map(m => m.Hierarchy)
                .CustomType<HierarchyId>();

            mapping.HasManyToMany(m => m.Roles)
                   .Table("OrganizationGroupRole")
                   .ChildKeyColumn("RoleId")
                   .ParentKeyColumn("OrganizationGroupId")
                   .LazyLoad();
        }
    }
}