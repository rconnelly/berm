namespace Quad.Berm.Data.Specifications
{
    public class CommonUpdateActionData<TEntity> : IInstanceActionData<TEntity>
    {
        public TEntity Instance { get; set; }
    }
}