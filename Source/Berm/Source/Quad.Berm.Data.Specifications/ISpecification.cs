namespace Quad.Berm.Data.Specifications
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;

    public interface ISpecification<T> : IQueryData<T>
    {
        #region Public Properties

        int PageIndex { get; }

        int PageSize { get; }

        #endregion

        #region Public Methods and Operators

        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "As Designed")]
        Expression<Func<T, bool>> IsSatisfiedBy();

        IQueryable<T> Order(IQueryable<T> query);

        #endregion
    }
}