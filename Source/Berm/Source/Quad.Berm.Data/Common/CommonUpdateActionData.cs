namespace Quad.Berm.Data.Common
{
    public class CommonUpdateActionData<TEntity> : IInstanceActionData<TEntity>
    {
        public TEntity Instance { get; set; }
    }
}