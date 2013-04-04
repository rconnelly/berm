namespace Quad.Berm.Tests.Common
{
    using Microsoft.Practices.ServiceLocation;

    using NUnit.Framework;

    using Quad.Berm.Common.Unity;
    using Quad.Berm.Initialization.Configuration;

    using QuickGenerate.Primitives;

    public abstract class TestBase
    {
        protected readonly StringGenerator ShortStringGenerator = new StringGenerator(5, 20);

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

        [TestFixtureSetUp]
        public virtual void Initialize()
        {
            NoCategoryTraceListener.Install();
            Shell.Start<InitializationContainerExtension>();
        }

        [TestFixtureTearDown]
        public virtual void Deinitialize()
        {
            Shell.Shutdown();
        }

        #endregion
    }
}