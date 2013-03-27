namespace Quad.Berm.Data.Common
{
    public interface IInstanceQueryData<TEntity> : IQueryData<TEntity>
    {
        TEntity Instance { get; set; }
    }
}