namespace Quad.Berm.Persistence.Impl.Commands
{
    using System.Diagnostics.Contracts;

    using Quad.Berm.Data.Common;

    internal class RelationalCreateCommand<TEntity> : RelationalActionCommand<CommonCreateActionData<TEntity>>
    {
        #region Public Methods and Operators

        public override void Execute(CommonCreateActionData<TEntity> queryData)
        {
            Contract.Assert(queryData != null);
            this.Session.Save(queryData.Instance);
        }

        #endregion
    }
}