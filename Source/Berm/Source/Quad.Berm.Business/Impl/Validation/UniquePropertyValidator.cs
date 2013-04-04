namespace Quad.Berm.Business.Impl.Validation
{
    using System.Diagnostics.Contracts;

    using FluentValidation.Validators;

    using Quad.Berm.Data;
    using Quad.Berm.Data.Specifications;
    using Quad.Berm.Persistence;

    internal class UniquePropertyValidator<T> : PropertyValidator
        where T : class
    {
        #region Fields

        private readonly IRepository repository;

        private readonly IInstanceQueryData<T> queryData;

        #endregion

        #region Constructors

        public UniquePropertyValidator(
            IRepository repository,
            IInstanceQueryData<T> queryData)
            : base("Property {PropertyName} should be unique")
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
            this.queryData.Instance = (T)context.Instance;
            var count = this.repository.Count(this.queryData);
            var result = count == 0;
            return result;
        }

        #endregion
    }
}