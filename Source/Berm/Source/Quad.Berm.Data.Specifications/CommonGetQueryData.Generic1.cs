namespace Quad.Berm.Data.Specifications
{
    public class CommonGetQueryData<TEntity, TKey> : IQueryData<TEntity>
    {
        public TKey Key { get; set; }
    }
}