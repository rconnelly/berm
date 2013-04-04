namespace Quad.Berm.Data
{
    public class StsCredentialEntity : BaseEntity
    {
        public virtual UserEntity User { get; set; }

        public virtual string Provider { get; set; }

        public virtual string Identifier { get; set; }
    }
}