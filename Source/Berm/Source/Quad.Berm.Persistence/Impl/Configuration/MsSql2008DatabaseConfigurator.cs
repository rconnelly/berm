namespace Quad.Berm.Persistence.Impl.Configuration
{
    using System;
    using System.Data.SqlClient;

    using FluentNHibernate.Cfg.Db;

    using NHibernate;
    using NHibernate.Exceptions;

    internal class MsSql2008DatabaseConfigurator : DatabaseConfigurator
    {
        protected override IPersistenceConfigurer CreatePersistenceConfigurator()
        {
            var configuration = MsSqlConfiguration.MsSql2008
                .DefaultSchema("berm")
                .ConnectionString(this.RuntimeConnectionString);
#if DEBUG
            configuration = configuration.FormatSql();
#endif
            return configuration;
        }

        protected override Type GetSqlExceptionConverterType()
        {
            return typeof(MsSqlExceptionConverter);
        }

        protected class MsSqlExceptionConverter : ISQLExceptionConverter
        {
            public Exception Convert(AdoExceptionContextInfo contextInfo)
            {
                Exception result = null;
                var sqle = ADOExceptionHelper.ExtractDbException(contextInfo.SqlException) as SqlException;
                if (sqle != null)
                {
                    switch (sqle.Number)
                    {
                        case (int)SqlExceptionNumber.StatementConflicted:
                            result = new ConstraintViolationException(
                                sqle.Message,
                                sqle, 
                                contextInfo.Sql, 
                                null);
                            break;
                        case (int)SqlExceptionNumber.InvalidObjectName:
                            result = new SQLGrammarException(
                                contextInfo.Message,
                                sqle, 
                                contextInfo.Sql);
                            break;
                        case 3960:
                            result = new StaleObjectStateException(
                                contextInfo.EntityName, 
                                contextInfo.EntityId);
                            break;
                    }
                }

                return result ?? SQLStateConverter.HandledNonSpecificException(
                    contextInfo.SqlException,
                    contextInfo.Message, 
                    contextInfo.Sql);
            }
        }
    }
}