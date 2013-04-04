namespace Quad.Berm.Data.Specifications.User
{
    using System;
    using System.Linq.Expressions;

    public class UndeletedUserByClientId : SpecificationBase<UserEntity>
    {
        private readonly long clientId;

        public UndeletedUserByClientId(long clientId)
        {
            this.clientId = clientId;
        }

        public override Expression<Func<UserEntity, bool>> IsSatisfiedBy()
        {
            return m => !m.Deleted && m.Role.Client.Id == this.clientId;
        }
    }
}