namespace Quad.Berm.Data.Specifications.Client
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class ClientById : SpecificationBase<ClientEntity>
    {
        public override Expression<Func<ClientEntity, bool>> IsSatisfiedBy()
        {
            return m => true;
        }

        public override IQueryable<ClientEntity> Order(IQueryable<ClientEntity> query)
        {
            return query.OrderBy(m => m.Name);
        }
    }
}