namespace Quad.Berm.Tests.Persistence
{
    using Quad.Berm.Persistence;
    using Quad.Berm.Tests.Common;

    public abstract class PersistenseTestsBase : TestBase
    {
        protected IRepository Repository
        {
            get
            {
                return this.Locator.GetInstance<IRepository>();
            }
        }
    }
}