namespace Quad.Berm.Data.Common
{
    public interface IInstanceActionData<TEntity> : IActionData<TEntity>
    {
        TEntity Instance { get; set; }
    }
}