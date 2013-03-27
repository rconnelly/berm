namespace Quad.Berm.Data.Specifications
{
    public class CommonDeleteActionData<TEntity> : IInstanceActionData<TEntity>
    {
        public TEntity Instance { get; set; }
    }
}