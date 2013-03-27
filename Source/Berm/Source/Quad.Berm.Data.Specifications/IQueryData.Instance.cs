namespace Quad.Berm.Data.Specifications
{
    public interface IInstanceQueryData<TEntity> : IQueryData<TEntity>
    {
        TEntity Instance { get; set; }
    }
}