namespace Quad.Berm.Persistence.Impl.Configuration
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
    using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
    using Microsoft.Practices.Unity;

    using NHibernate;
    using NHibernate.Exceptions;

    using Quad.Berm.Common.Exceptions;
    using Quad.Berm.Common.Transactions;
    using Quad.Berm.Common.Unity;
    using Quad.Berm.Data;
    using Quad.Berm.Data.Specifications;
    using Quad.Berm.Persistence.Exceptions;
    using Quad.Berm.Persistence.Impl.Commands;

    using Unity.AutoRegistration;

    public class PersistenceContainerExtension : UnityContainerExtension
    {
        #region Fields

        public const string ExecutePolicy = "Persistence.Execute";

        public const string DeletePolicy = "Persistence.Delete";

        private readonly FlushMode flushMode;

        private readonly string connectionName;

        #endregion

        #region Constructors

        [InjectionConstructor]
        public PersistenceContainerExtension() : this(false, null)
        {
        }

        public PersistenceContainerExtension(bool batch, string connectionName)
        {
            // auto allows speed up mass create / update operations
            // but cause auto database update before each count operation,
            // so in auto mode we have problem with validation:
            // change value - validate - validate cause count() operation - auto flush occured - database constraint
            // that is why in application server we need auto mode
            this.flushMode = batch ? FlushMode.Auto : FlushMode.Commit;
            this.connectionName = connectionName;
        }

        #endregion

        #region Methods

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "As Designed")]
        protected override void Initialize()
        {
            this.Container
                .RegisterType<DatabaseConfigurator, MsSql2008DatabaseConfigurator>("System.Data.SqlClient")
                .RegisterType<DatabaseConfigurator>(new InjectionFactory(this.CreateDatabaseConfigurator))
                .RegisterType<ISessionFactory>(
                    new ContainerControlledLifetimeManager(),
                    new InjectionFactory(CreateSessionFactory))
                .RegisterType<ISession>(new AmbientContextLifetimeManager(), new InjectionFactory(this.CreateSession))
                .RegisterType<IStatelessSession>(new AmbientContextLifetimeManager(), new InjectionFactory(this.CreateStatelessSession))
                .RegisterType<Func<ISession>>(
                    new ContainerControlledLifetimeManager(), 
                    new InjectionFactory(
                        c =>
                        {
                            Func<ISession> func = () => c.Resolve<ISession>();
                            return func;
                        }))
                .RegisterType<Func<IStatelessSession>>(
                    new ContainerControlledLifetimeManager(),
                    new InjectionFactory(
                        c =>
                        {
                            Func<IStatelessSession> func = () => c.Resolve<IStatelessSession>();
                            return func;
                        }))
                .RegisterType<IRepository, Repository>(new ContainerControlledLifetimeManager())
                .RegisterType<ITransactionManager, TransactionManager>();

            LoggerProvider.SetLoggersFactory(new NHibernate.Logging.CommonLogging.CommonLoggingLoggerFactory());

            this.RegisterCommands();

            this.ConfigureExceptionHandling();
        }

        private static ISessionFactory CreateSessionFactory(IUnityContainer container)
        {
            var configurator = container.Resolve<DatabaseConfigurator>();
            var sessionFactory = configurator.CreateSessionFactory();
            return sessionFactory;
        }

        private static Type[] CommandToContract(Type commandType)
        {
            var query = from t in commandType.GetInterfaces()
                        let gd = t.IsGenericType ? t.GetGenericTypeDefinition() : null
                        where gd != null
                        && (gd == typeof(IQueryCommand<,>) || gd == typeof(IActionCommand<>))
                        select t;
            var result = query.ToArray();
            return result;
        }

        private static void RegisterSpecification(Type specificationType, IUnityContainer container)
        {
            var resulType =
                specificationType.GetInterfaces()
                .Single(
                    t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ISpecification<>))
                .GetGenericArguments()
                .Single();
            var queryDataType = specificationType;
            var interfaceType = typeof(IQueryCommand<,>).MakeGenericType(queryDataType, resulType);
            var commandType = typeof(RelationalSpecificationCommand<>).MakeGenericType(resulType);
            container.RegisterType(interfaceType, commandType);
        }

        private static void RegisterRelational(Type entityType, IUnityContainer container)
        {
            container.RegisterType(typeof(IActionCommand<>).MakeGenericType(typeof(CommonCreateActionData<>).MakeGenericType(entityType)), typeof(RelationalCreateCommand<>).MakeGenericType(entityType));
            container.RegisterType(typeof(IActionCommand<>).MakeGenericType(typeof(CommonUpdateActionData<>).MakeGenericType(entityType)), typeof(RelationalUpdateCommand<>).MakeGenericType(entityType));
            container.RegisterType(typeof(IActionCommand<>).MakeGenericType(typeof(CommonDeleteActionData<>).MakeGenericType(entityType)), typeof(RelationalDeleteCommand<>).MakeGenericType(entityType));
            container.RegisterType(typeof(IQueryCommand<,>).MakeGenericType(typeof(CommonGetQueryData<>).MakeGenericType(entityType), entityType), typeof(RelationalGetByIdCommand<>).MakeGenericType(entityType));
            container.RegisterType(typeof(IQueryCommand<,>).MakeGenericType(typeof(CommonLazyGetQueryData<>).MakeGenericType(entityType), entityType), typeof(RelationalLazyGetByIdCommand<>).MakeGenericType(entityType));
        }

        private DatabaseConfigurator CreateDatabaseConfigurator(IUnityContainer container)
        {
            using (var configurationSource = ConfigurationSourceFactory.Create())
            {
                var currentConnectionName = this.connectionName;
                if (string.IsNullOrEmpty(currentConnectionName))
                {
                    currentConnectionName = DatabaseConfigurator.DefaultDatabaseConnectionName;
                    var settings = DatabaseSettings.GetDatabaseSettings(configurationSource);
                    if (settings != null)
                    {
                        currentConnectionName = settings.DefaultDatabase;
                    }
                }

                var section = (ConnectionStringsSection)configurationSource.GetSection("connectionStrings");
                Contract.Assert(section != null);
                var css = section.ConnectionStrings[currentConnectionName];
                Contract.Assert(css != null);
                Contract.Assert(!string.IsNullOrEmpty(css.ProviderName));
                var configurator = container.Resolve<DatabaseConfigurator>(css.ProviderName);
                return configurator;
            }
        }
        
        private ISession CreateSession(IUnityContainer container)
        {
            var sessionFactory = container.Resolve<ISessionFactory>();
            var session = sessionFactory.OpenSession();
            Contract.Assert(session != null);
            session.FlushMode = this.flushMode;
            return session;
        }

        private IStatelessSession CreateStatelessSession(IUnityContainer container)
        {
            var sessionFactory = container.Resolve<ISessionFactory>();
            var session = sessionFactory.OpenStatelessSession();
            Contract.Assert(session != null);
            return session;
        }

        private void RegisterCommands()
        {
            this
                .ConfigureSelfAutoRegistration(typeof(ISpecification<>), typeof(IEntity))
                .Include(
                    t => t.IsClass && !t.IsGenericType && !t.IsAbstract && t.Name.EndsWith(WellKnownAppParts.Command, StringComparison.Ordinal),
                    Then.Register().As(t => CommandToContract(t)))
                .Include(
                    t => t.IsClass && !t.IsGenericType && !t.IsAbstract && t.ImplementsOpenGeneric(typeof(ISpecification<>)),
                    RegisterSpecification)
                .Include(
                    t => t.IsClass && !t.IsGenericType && !t.IsAbstract && t.Implements<IIdentified>(),
                    RegisterRelational)
                .ApplyAutoRegistration();
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent.IExceptionConfigurationWithMessage.UsingMessage(System.String)", Justification = "As Designed")]
        private void ConfigureExceptionHandling()
        {
            using (var configurationSource = new DictionaryConfigurationSource())
            {
                var builder = new ConfigurationSourceBuilder();
                builder.ConfigureExceptionHandling()
                        .GivenPolicyWithName(DeletePolicy)
                            .ForExceptionType<ConstraintViolationException>()
                                .WrapWith<DeleteConstraintException>()
                                .UsingMessage("Cannot delete object.")
                                .ThenThrowNewException()
                            .ForExceptionType<Exception>()
                                .ThenNotifyRethrow()
                        .GivenPolicyWithName(ExecutePolicy)
                            .ForExceptionType<ConstraintViolationException>()
                                .WrapWith<DeleteConstraintException>()
                                .UsingMessage("Cannot delete object.")
                                .ThenThrowNewException()
                            .ForExceptionType<GenericADOException>()
                                .HandleCustom<SqlExceptionHandler>()
                                .ThenThrowNewException()
                            .ForExceptionType<SqlException>()
                                .HandleCustom<SqlExceptionHandler>()
                                .ThenThrowNewException()
                            .ForExceptionType<Exception>()
                                .ThenNotifyRethrow();
                builder.UpdateConfigurationWithReplace(configurationSource);

                using (var configurator = new UnityContainerConfigurator(this.Container))
                {
                    EnterpriseLibraryContainer.ConfigureContainer(configurator, configurationSource);
                }
            }
        }

        #endregion

        #region Nested types

        // ReSharper disable ClassNeverInstantiated.Local
        private class SqlExceptionHandler : IExceptionHandler
        // ReSharper restore ClassNeverInstantiated.Local
        {
            private static readonly Regex IndexDuplicateInsertConstraintRegex = new Regex("'(?<ConstraintName>(?<ConstraintType>PK|FK|IX|UX|DF)_[a-z_]*)'", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

            [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "collection", Justification = "By design")]
            // ReSharper disable UnusedParameter.Local
            public SqlExceptionHandler(NameValueCollection collection)
            // ReSharper restore UnusedParameter.Local
            {
            }

            public Exception HandleException(Exception exception, Guid handlingInstanceId)
            {
                Exception result = null;
                Exception dispatched = null;
                var adoException = exception as GenericADOException;
                if (adoException != null)
                {
                    dispatched = adoException.InnerException;
                }

                var sqlException = (dispatched ?? exception) as SqlException;
                if (sqlException != null)
                {
                    if (sqlException.Errors
                        .Cast<SqlError>()
                        .Any(error =>
                            error.Number == (int)SqlExceptionNumber.IndexDuplicateInsert
                            || error.Number == (int)SqlExceptionNumber.KeyDuplicateInsert))
                    {
                        var matches = IndexDuplicateInsertConstraintRegex.Match(exception.Message);
                        var constraintNameGroup = matches.Groups["ConstraintName"];

                        result = new ExecutionConstraintException(exception)
                        {
                            ConstraintName = constraintNameGroup.Success ? constraintNameGroup.Value : null
                        };
                    }
                }

                return result ?? new PersistenceException(exception.Message, exception);
            }
        }

        #endregion
    }
}