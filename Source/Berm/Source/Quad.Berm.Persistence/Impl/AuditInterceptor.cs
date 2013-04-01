namespace Quad.Berm.Persistence.Impl
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;

    using NHibernate;
    using NHibernate.Type;

    using Quad.Berm.Data;
    using Quad.Berm.Persistence.Impl.Utilities.Linq;

    internal class AuditInterceptor : EmptyInterceptor
    {
        #region Fields

        private static readonly string CreatedDatePropertyName =
            ((Expression<Func<IAuditable, DateTime>>)(p => p.Created)).GetFullPropertyName();

        private static readonly string ModifiedDatePropertyName =
            ((Expression<Func<IAuditable, DateTime?>>)(p => p.Modified)).GetFullPropertyName();

        #endregion

        #region Methods

        public override bool OnSave(
            object entity,
            object id,
            object[] state,
            string[] propertyNames,
            IType[] types)
        {
            var changed = false;
            if (entity is IAuditable)
            {
                var dateIndex = Array.FindIndex(
                    propertyNames, n => CreatedDatePropertyName.Equals(n, StringComparison.Ordinal));
                Contract.Assert(dateIndex > -1);
                state[dateIndex] = DateTime.UtcNow;

                changed = true;
            }

            return changed;
        }

        public override bool OnFlushDirty(
            object entity,
            object id,
            object[] currentState,
            object[] previousState,
            string[] propertyNames,
            IType[] types)
        {
            var changed = false;
            if (entity is IAuditable)
            {
                var dateIndex = Array.FindIndex(
                    propertyNames, n => ModifiedDatePropertyName.Equals(n, StringComparison.Ordinal));
                Contract.Assert(dateIndex > -1);
                currentState[dateIndex] = DateTime.UtcNow;

                changed = true;
            }

            return changed;
        }

        #endregion
    }
}