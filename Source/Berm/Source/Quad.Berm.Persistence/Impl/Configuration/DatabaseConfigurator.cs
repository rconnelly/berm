namespace Quad.Berm.Persistence.Impl.Configuration
{
    using System;
    using System.Configuration;
    using System.Globalization;

    using global::Common.Logging;

    using FluentNHibernate.Automapping;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using FluentNHibernate.Diagnostics;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
    using Microsoft.WindowsAzure.ServiceRuntime;

    using NHibernate;

    using Quad.Berm.Data;
    using Quad.Berm.Persistence.Impl.Configuration.Conventions;

    internal abstract class DatabaseConfigurator
    {
        #region Constants and Fields

        internal const string DefaultDatabaseConnectionName = "database";

        private string connectionString;

        #endregion

        #region Public Properties

        public string ConnectionString
        {
            get
            {
                return this.connectionString ?? (this.connectionString = DefaultConnectionString);
            }

            set
            {
                this.connectionString = value;
            }
        }

        #endregion

        #region Properties

        protected static string DefaultConnectionString
        {
            get
            {
                string connectionString = null;
                
                using (var configurationSource = ConfigurationSourceFactory.Create())
                {
                    var connectionName = DefaultDatabaseConnectionName;
                    var settings = DatabaseSettings.GetDatabaseSettings(configurationSource);
                    if (settings != null)
                    {
                        connectionName = settings.DefaultDatabase;
                    }
                    
                    if (RoleEnvironment.IsAvailable)
                    {
                        connectionString =
                            RoleEnvironment.GetConfigurationSettingValue(
                                string.Format(CultureInfo.InvariantCulture, "ConnectionString.{0}", connectionName));
                    }

                    if (connectionString == null)
                    {
                        var section = (ConnectionStringsSection)configurationSource.GetSection("connectionStrings");
                        if (section != null)
                        {
                            var css = section.ConnectionStrings[connectionName];
                            if (css != null)
                            {
                                connectionString = css.ConnectionString;
                            }
                        }
                    }

                    if (connectionString == null)
                    {
                        throw new ConfigurationErrorsException(
                            string.Format(
                                CultureInfo.InvariantCulture, "Connection string [{0}] cannot be found", connectionName));
                    }
                }

                return connectionString; 
            }
        }

        #endregion

        #region Public Methods and Operators

        public ISessionFactory CreateSessionFactory()
        {
            var configuration = this.CreateProductionSchema();
            var result = configuration.BuildSessionFactory();
            return result;
        }

        #endregion

        #region Methods

        protected abstract IPersistenceConfigurer CreatePersistenceConfigurator();

        protected virtual FluentConfiguration CreateProductionSchema()
        {
            var cfg = new AutomappingConfiguration();
            var configuration = Fluently.Configure()
                .Diagnostics(d => d.RegisterListener(new CommonLoggingDiagnosticListener()).Enable())
                .Database(this.CreatePersistenceConfigurator())
                .Mappings(
                    m => m
                        .AutoMappings.Add(
                        () => AutoMap
                                  .AssemblyOf<BaseEntity>(cfg)
                                  .UseOverridesFromAssemblyOf<TableNameConvention>()
                                  .Conventions
                                  .AddFromAssemblyOf<TableNameConvention>()))
                .Cache(
                    c => c
                        .ProviderClass<NHibernate.Cache.HashtableCacheProvider>()
                        .UseQueryCache())
                .ExposeConfiguration(
                        config =>
                            {
                                config.SetInterceptor(new AuditInterceptor());
                                var type = this.GetSqlExceptionConverterType();
                                if (type != null)
                                {                                    
                                    config.SetProperty(
                                        NHibernate.Cfg.Environment.SqlExceptionConverter, 
                                        type.AssemblyQualifiedName);
                                }
                            });
            return configuration;
        }

        protected virtual Type GetSqlExceptionConverterType()
        {
            return null;
        }

        protected void RuntimeConnectionString(ConnectionStringBuilder connectionStringBuilder)
        {
            connectionStringBuilder.Is(this.ConnectionString);
        }

        #endregion

        #region Nested types

        internal class CommonLoggingDiagnosticListener : IDiagnosticListener
        {
            private readonly IDiagnosticResultsFormatter formatter;

            private readonly ILog logger = LogManager.GetLogger("NHibernate.Automapper");

            public CommonLoggingDiagnosticListener()
                : this(new DefaultOutputFormatter())
            {
            }

            public CommonLoggingDiagnosticListener(IDiagnosticResultsFormatter formatter)
            {
                this.formatter = formatter;
            }

            public void Receive(DiagnosticResults results)
            {
                var output = this.formatter.Format(results);
                this.logger.Debug(output);
            }
        }

        #endregion
    }
}