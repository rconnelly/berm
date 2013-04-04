namespace Quad.Berm.Data.Specifications.User
{
    using System;
    using System.Linq.Expressions;

    public class UndeletedUser : SpecificationBase<UserEntity>
    {
        public override Expression<Func<UserEntity, bool>> IsSatisfiedBy()
        {
            return m => !m.Deleted;
        }
    }
}
