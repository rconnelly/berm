namespace Quad.Berm.Data.Common
{
    public class CommonCreateActionData<TEntity> : IInstanceActionData<TEntity>
    {
        public TEntity Instance { get; set; }
    }
}