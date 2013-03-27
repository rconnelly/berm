namespace Quad.Berm.Tests.Common
{
    using Microsoft.Practices.ServiceLocation;

    using NUnit.Framework;

    using Quad.Berm.Common.Unity;
    using Quad.Berm.Initialization.Configuration;

    public abstract class TestBase
    {
        #region Properties

        protected IServiceLocator Locator
        {
            get
            {
                return ServiceLocator.Current;
            }
        }

        #endregion

        #region Public Methods and Operators

        [TestFixtureTearDown]
        public virtual void Deinitialize()
        {
            Shell.Shutdown();
        }

        [TestFixtureSetUp]
        public virtual void Initialize()
        {
            NoCategoryTraceListener.Install();
            Shell.Start<InitializationContainerExtension>();
        }

        #endregion
    }
}