namespace Quad.Berm.Data.Specifications.User
{
    using System;
    using System.Linq.Expressions;

    public class UniqueStsCredentialIdentifier : SpecificationBase<StsCredentialEntity>, IInstanceQueryData<StsCredentialEntity>
    {
        public StsCredentialEntity Instance { get; set; }

        public override Expression<Func<StsCredentialEntity, bool>> IsSatisfiedBy()
        {
            var id = this.Instance.Id;
            var identifier = this.Instance.Identifier;
            return m => m.Id != id && m.Identifier == identifier;
        }
    }
}