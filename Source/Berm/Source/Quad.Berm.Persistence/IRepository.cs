namespace Quad.Berm.Persistence
{
    using System.Collections.Generic;

    using Quad.Berm.Data.Specifications;

    public interface IRepository
    {
        IEnumerable<T> Enumerable<T>(IQueryData queryData);

        T Scalar<T>(IQueryData queryData);

        int Count<T>(IQueryData queryData);

        void Execute(IActionData actionData, bool flush = false);
    }
}