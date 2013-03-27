namespace Quad.Berm.Persistence.Impl.Configuration
{
    using System;

    using FluentNHibernate.Automapping;

    using Quad.Berm.Data;

    internal class AutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return typeof(IEntity).IsAssignableFrom(type);
        }

        public override bool AbstractClassIsLayerSupertype(Type type)
        {
            return type == typeof(IEntity) || type == typeof(BaseEntity);
        }
    }
}