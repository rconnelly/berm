namespace Quad.Berm.Persistence.Impl.Commands
{
    using System;
    using System.Diagnostics.Contracts;

    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

    using Quad.Berm.Data.Specifications;
    using Quad.Berm.Persistence.Impl.Configuration;

    internal class RelationalDeleteCommand<TEntity> : RelationalActionCommand<CommonDeleteActionData<TEntity>>
    {
        #region Public Methods and Operators

        public override void Execute(CommonDeleteActionData<TEntity> queryData)
        {
            Contract.Assert(queryData != null);
            try
            {
                this.Session.Delete(queryData.Instance);
                this.Session.Flush();
            }
            catch (Exception ex)
            {
                var rethrow = ExceptionPolicy.HandleException(ex, PersistenceContainerExtension.DeletePolicy);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion
    }
}