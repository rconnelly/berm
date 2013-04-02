namespace Quad.Berm.Persistence.Impl.Mappings
{
    using System;
    using System.Data;
    using System.Data.SqlTypes;

    using Microsoft.SqlServer.Types;

    using NHibernate;
    using NHibernate.SqlTypes;
    using NHibernate.UserTypes;

    public class HierarchyId : IUserType
    {
        #region Properties

        public SqlType[] SqlTypes
        {
            get { return new[] { NHibernateUtil.String.SqlType }; }
        }

        public Type ReturnedType
        {
            get { return typeof(SqlHierarchyId); }
        }

        public bool IsMutable
        {
            get { return true; }
        }

        #endregion Properties

        #region Methods

        public new bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            var prop1 = NHibernateUtil.String.NullSafeGet(rs, names[0]);

            if (prop1 == null)
            {
                return null;
            }

            return SqlHierarchyId.Parse(new SqlString(prop1.ToString()));
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else if (value is SqlHierarchyId)
            {
                ((IDataParameter)cmd.Parameters[index]).Value = ((SqlHierarchyId)value).ToString();
            }
        }

        public object DeepCopy(object value)
        {
            if (value == null)
            {
                return null;
            }

            return SqlHierarchyId.Parse(((SqlHierarchyId)value).ToString());
        }

        public object Replace(object original, object target, object owner)
        {
            return this.DeepCopy(original);
        }

        public object Assemble(object cached, object owner)
        {
            return this.DeepCopy(cached);
        }

        public object Disassemble(object value)
        {
            return this.DeepCopy(value);
        }

        #endregion Methods
    }
}