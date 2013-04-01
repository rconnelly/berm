namespace Quad.Berm.Data.Specifications
{
    public abstract class SpecificationInstanceBase<TEntity> : SpecificationBase<TEntity>, IInstanceQueryData<TEntity>
    {
        public TEntity Instance { get; set; }
    }
}