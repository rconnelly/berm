namespace Quad.Berm.Data.Specifications.User
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class ActiveUserByCredentials : SpecificationBase<UserEntity>
    {
        private readonly string provider;

        private readonly string identifier;

        public ActiveUserByCredentials(string provider, string identifier)
        {
            this.provider = provider;
            this.identifier = identifier;
        }

        public override Expression<Func<UserEntity, bool>> IsSatisfiedBy()
        {
            return m => !m.Disabled && !m.Deleted && m.StsCredentials.Any(c => c.Identifier == this.identifier && c.Provider == this.provider);
        }
    }
}