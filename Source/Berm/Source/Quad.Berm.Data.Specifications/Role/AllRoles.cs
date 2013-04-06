namespace Quad.Berm.Data.Specifications.Role
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class AllRoles : SpecificationBase<RoleEntity>
    {
        public override Expression<Func<RoleEntity, bool>> IsSatisfiedBy()
        {
            return m => true;
        }

        public override IQueryable<RoleEntity> Order(System.Linq.IQueryable<RoleEntity> query)
        {
            return query.OrderBy(m => m.Name);
        }
    }
}