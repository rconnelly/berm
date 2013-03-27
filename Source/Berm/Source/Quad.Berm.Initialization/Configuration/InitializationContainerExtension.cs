namespace Quad.Berm.Initialization.Configuration
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Caching;

    using Quad.Berm.Business.Impl.Configuration;
    using Quad.Berm.Persistence.Impl.Configuration;

    using global::Common.Logging.EntLib;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
    using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
    using Microsoft.Practices.Unity;

    using Quad.Berm.Common.Unity;
    using Quad.Berm.Data;

    public class InitializationContainerExtension : UnityContainerExtension
    {
        #region Methods

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "As Designed")]
        protected override void Initialize()
        {
            this.Container
                .AddNewExtension<PersistenceContainerExtension>()
                .AddNewExtension<BusinessContainerExtension>();

            this.Container
                .RegisterType<ObjectCache>(new ContainerControlledLifetimeManager(), new InjectionFactory(c => MemoryCache.Default))
                .RegisterType<ObjectCache>(MetadataInfo.UnitOfWorkCache, new UnitOfWorkLifetimeManager(), new InjectionFactory(c => new MemoryCache(MetadataInfo.UnitOfWorkCache)))
                .RegisterType<Func<ObjectCache>>(MetadataInfo.UnitOfWorkCache, new ContainerControlledLifetimeManager(), new InjectionFactory(c => (Func<ObjectCache>)(() => c.Resolve<ObjectCache>(MetadataInfo.UnitOfWorkCache))));

            this.ConfigureLogging();
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "By Design")]
        private void ConfigureLogging()
        {
            var configurationSource = new DictionaryConfigurationSource();
            var builder = new ConfigurationSourceBuilder();
            const string DefaultListenerName = "Default";

            builder.ConfigureLogging()
                .WithOptions
                    .DoNotRevertImpersonation()
                .SpecialSources
                    .LoggingErrorsAndWarningsCategory
                        .SendTo.SharedListenerNamed(DefaultListenerName)
                .SpecialSources
                    .UnprocessedCategory
                        .SendTo.SharedListenerNamed(DefaultListenerName)
                .SpecialSources
                    .AllEventsCategory
                        .SendTo.SharedListenerNamed(DefaultListenerName)
                .LogToCategoryNamed("General")
                    .WithOptions.SetAsDefaultCategory()
                    .SendTo.SharedListenerNamed(DefaultListenerName);
            builder.UpdateConfigurationWithReplace(configurationSource);

            using (var configurator = new UnityContainerConfigurator(this.Container))
            {
                EnterpriseLibraryContainer.ConfigureContainer(configurator, configurationSource);
            }
            
            this.Container.RegisterType<TraceListener, CommonLoggingEntlibTraceListener>(
                DefaultListenerName,
                new ContainerControlledLifetimeManager(),
                new InjectionFactory(this.CreateListener));
        }

        private TraceListener CreateListener(IUnityContainer c)
        {
            var formatter = new TextFormatter("{message}{dictionary({key} - {value}{newline})}");
            var data = new CommonLoggingEntlibTraceListenerData(this.GetType().FullName, "{listenerName}.{sourceName}", "Text Formatter");
            var listener = new CommonLoggingEntlibTraceListener(data, formatter);
            return listener;
        }

        #endregion
    }
}
