﻿namespace Quad.Berm.Persistence.Impl.Configuration.Conventions
{
    using System;
    using System.Globalization;

    using FluentNHibernate;
    using FluentNHibernate.Conventions;

    using Quad.Berm.Persistence.Impl.Utilities.Text;

    internal class ForeignKeyNameConvention : ForeignKeyConvention
    {
        protected override string GetKeyName(Member property, Type type)
        {
            string result;
            var tableName = Singularizer.Singularize(this.CleanTableName(type.Name));
            if (property == null 
                || property.Name == tableName)
            {
                var name = property != null ?
                            property.Name :
                            tableName;
                result = string.Format(CultureInfo.InvariantCulture, "{0}Id", name);
            }
            else
            {
                result = string.Format(CultureInfo.InvariantCulture, "{0}{1}Id", property.Name, tableName);
            }

            return result;
        }
    }
}