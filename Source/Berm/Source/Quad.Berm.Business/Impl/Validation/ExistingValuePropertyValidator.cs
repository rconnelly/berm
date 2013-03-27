namespace Quad.Berm.Business.Impl.Validation
{
    using System.Diagnostics.Contracts;

    using FluentValidation.Validators;

    using Quad.Berm.Data.Common;
    using Quad.Berm.Persistence;

    internal class ExistingValuePropertyValidator<TResult, TIdentity> : PropertyValidator
    {
        #region Fields

        private readonly IRepository repository;

        private readonly CommonGetQueryData<TResult, TIdentity> queryData;

        #endregion

        #region Constructors

        public ExistingValuePropertyValidator(
            IRepository repository,
            CommonGetQueryData<TResult, TIdentity> queryData)
            : base("Value of '{PropertyName}' not exist")
        {
            Contract.Assert(repository != null);
            Contract.Assert(queryData != null);

            this.repository = repository;
            this.queryData = queryData;
        }

        #endregion

        #region Methods

        protected override bool IsValid(PropertyValidatorContext context)
        {
            this.queryData.Key = (TIdentity)context.PropertyValue;
            var count = this.repository.Count(this.queryData);
            var result = count > 0;
            return result;
        }

        #endregion
    }
}