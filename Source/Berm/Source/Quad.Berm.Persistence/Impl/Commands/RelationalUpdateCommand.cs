namespace Quad.Berm.Persistence.Impl.Commands
{
    using System.Diagnostics.Contracts;

    using Quad.Berm.Data.Specifications;

    internal class RelationalUpdateCommand<TEntity> : RelationalActionCommand<CommonUpdateActionData<TEntity>>
    {
        #region Public Methods and Operators

        public override void Execute(CommonUpdateActionData<TEntity> queryData)
        {
            Contract.Assert(queryData != null);
            this.Session.Update(queryData.Instance);
        }

        #endregion
    }
}