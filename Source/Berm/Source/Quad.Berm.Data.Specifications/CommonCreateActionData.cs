namespace Quad.Berm.Data.Specifications
{
    public class CommonCreateActionData<TEntity> : IInstanceActionData<TEntity>
    {
        public TEntity Instance { get; set; }
    }
}