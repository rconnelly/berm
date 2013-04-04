namespace Quad.Berm.Persistence.Impl.Mappings
{
    using FluentNHibernate.Automapping;
    using FluentNHibernate.Automapping.Alterations;

    using Quad.Berm.Data;

    public class UserMapping : IAutoMappingOverride<UserEntity>
    {
        public void Override(AutoMapping<UserEntity> mapping)
        {
            mapping.Table("[User]");

            mapping.HasMany(m => m.StsCredentials).Cascade.AllDeleteOrphan().Inverse();
        }
    }
}