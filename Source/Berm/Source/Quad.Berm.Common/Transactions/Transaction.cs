namespace Quad.Berm.Common.Transactions
{
    using System;

    using Microsoft.Practices.ServiceLocation;

    public class Transaction : IDisposable
    {
        #region Constants and Fields

        private bool disposed;

        private bool isCompleted;

        private IDisposable transaction;

        private ITransactionManager transactionManager;

        #endregion

        #region Constructors and Destructors

        public Transaction()
        {
            this.transactionManager = ServiceLocator.Current.GetInstance<ITransactionManager>();
            this.transaction = this.transactionManager.BeginTransaction();
        }

        ~Transaction()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public Methods and Operators

        public void Complete()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("Transaction");
            }

            this.transactionManager.CommitTransaction(this.transaction);
            this.isCompleted = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                if (!this.isCompleted)
                {
                    this.transactionManager.RollbackTransaction(this.transaction);
                }

                this.transaction.Dispose();

                this.transaction = null;
                this.transactionManager = null;
            }

            this.disposed = true;
        }

        #endregion
    }
}