namespace Quad.Berm.Business.Impl
{
    using System.Linq;

    using FluentValidation;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    using Quad.Berm.Business.Impl.Validation;
    using Quad.Berm.Data;
    using Quad.Berm.Data.Specifications;
    using Quad.Berm.Persistence;

    internal class ManagerBase<T> : IManager<T>
        where T : class
    {
        #region Public Properties

        [Dependency]
        public IServiceLocator Locator { get; set; }

        [Dependency]
        public IRepository Repository { get; set; }

        #endregion

        #region Methods

        public virtual T Create(T instance)
        {
            this.Repository.Create(instance);
            return instance;
        }

        public virtual void Update(T instance)
        {
            this.Repository.Update(instance);
        }

        public virtual void Delete(T instance)
        {
            this.Repository.Delete(instance);
        }

        public virtual int Count(IQueryData<T> queryData)
        {
            var count = this.Repository.Count(queryData);
            return count;
        }

        public virtual IQueryable<T> Query(IQueryData<T> queryData)
        {
            var result = this.Repository.Query(queryData);
            return result;
        }

        protected void DemandValid<TValidator, TInstance>(TInstance instance)
            where TValidator : IValidator<TInstance>
        {
            var validator = this.Locator.GetInstance<TValidator>();
            validator.DemandValid(instance);
        }

        #endregion
    }
}