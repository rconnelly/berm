namespace Quad.Berm.Business
{
    using System.Linq;

    using Quad.Berm.Data.Common;

    public interface IManager<T>
    {
        T Create(T instance);

        void Update(T instance);

        void Delete(T instance);

        int Count(IQueryData<T> queryData);

        IQueryable<T> Query(IQueryData<T> queryData);
    }
}