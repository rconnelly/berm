namespace Quad.Berm.Data.Common
{
    public class CommonGetQueryData<TEntity, TKey> : IQueryData<TEntity>
    {
        public TKey Key { get; set; }
    }
}