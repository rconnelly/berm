namespace Quad.Berm.Data.Specifications
{
    public interface IInstanceActionData<TEntity> : IActionData<TEntity>
    {
        TEntity Instance { get; set; }
    }
}