namespace Quad.Berm.Persistence.Impl.Commands
{
    using System.Collections.Generic;

    using Quad.Berm.Data.Specifications;

    internal interface IQueryCommand<in TQueryData, out TResult> : IQueryCommand<TResult>
        where TQueryData : IQueryData
    {
        IEnumerable<TResult> Execute(TQueryData queryData);

        TResult ExecuteScalar(TQueryData queryData);

        int RowCount(TQueryData queryData);
    }
}