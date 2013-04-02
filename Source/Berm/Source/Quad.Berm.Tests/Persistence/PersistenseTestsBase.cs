namespace Quad.Berm.Tests.Persistence
{
    using NHibernate;

    using NUnit.Framework;

    using Quad.Berm.Common.Transactions;
    using Quad.Berm.Common.Unity;
    using Quad.Berm.Persistence;
    using Quad.Berm.Tests.Common;

    using QuickGenerate.Primitives;

    public abstract class PersistenseTestsBase : TestBase
    {
        #region Constants and Fields

        protected readonly StringGenerator ShortStringGenerator = new StringGenerator(5, 20);

        private Transaction transaction;

        private AmbientContext context;

        #endregion

        #region Public Properties

        protected ISession Session
        {
            get
            {
                return this.Locator.GetInstance<ISession>();
            }
        }

        protected IRepository Repository
        {
            get
            {
                return this.Locator.GetInstance<IRepository>();
            }
        }

        #endregion

        #region Public Methods and Operators

        [SetUp]
        public virtual void SetUp()
        {
            this.context = new AmbientContext();
            this.transaction = new Transaction();
        }

        [TearDown]
        public virtual void TearDown()
        {
            this.transaction.Dispose();
            this.context.Dispose();
        }

        #endregion
    }
}