namespace Quad.Berm.Data.Common
{
    public class CommonDeleteActionData<TEntity> : IInstanceActionData<TEntity>
    {
        public TEntity Instance { get; set; }
    }
}