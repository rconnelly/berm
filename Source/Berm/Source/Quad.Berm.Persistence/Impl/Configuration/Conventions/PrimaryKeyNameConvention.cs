namespace Quad.Berm.Persistence.Impl.Configuration.Conventions
{
    using FluentNHibernate.Conventions;
    using FluentNHibernate.Conventions.Instances;

    using Quad.Berm.Persistence.Impl.Utilities.Text;

    /// <summary>
    /// PrimaryKeyNameConvention - says that name of every column representing primary key should consist of entity name and “Id” suffix.
    /// </summary>
    internal class PrimaryKeyNameConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            var tableName = this.CleanTableName(instance.EntityType.Name);
            var column = Singularizer.Singularize(tableName) + "Id";
            instance.Column(column);
        }
    }
}