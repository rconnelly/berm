namespace Quad.Berm.Persistence.Impl.Commands
{
    using Microsoft.Practices.Unity;

    using NHibernate;

    using Quad.Berm.Data.Common;

    internal abstract class RelationalQueryCommand<TQueryData, TResult> : QueryCommandBase<TQueryData, TResult>
        where TQueryData : IQueryData<TResult>
    {
        #region Public Properties

        [Dependency]
        public ISession Session { get; set; }

        #endregion
    }
}