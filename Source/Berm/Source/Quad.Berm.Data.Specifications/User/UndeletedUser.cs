namespace Quad.Berm.Data.Specifications.User
{
    using System;
    using System.Linq.Expressions;

    public class UndeletedUser : SpecificationBase<UserEntity>
    {
        public long UserId { get; set; }

        public override Expression<Func<UserEntity, bool>> IsSatisfiedBy()
        {
            return this.UserId == 0
                       ? (Expression<Func<UserEntity, bool>>)(m => !m.Deleted)
                       : (m => !m.Deleted && m.Id == this.UserId);
        }
    }
}
