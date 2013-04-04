namespace Quad.Berm.Data
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Security.Claims;

    public class UserEntity : BaseEntity
    {
        public virtual RoleEntity Role { get; set; }

        public virtual ICollection<StsCredentialEntity> StsCredentials { get; set; }

        public virtual string Name { get; set; }

        public virtual bool Disabled { get; set; }

        public virtual bool Deleted { get; set; }

        public virtual void AddCredential(StsCredentialEntity credential)
        {
            Contract.Assert(credential != null);

            if (this.StsCredentials == null)
            {
                this.StsCredentials = new Collection<StsCredentialEntity>();
            }

            credential.User = this;
            this.StsCredentials.Add(credential);
        }

        public virtual IEnumerable<Claim> ToClaims()
        {   
            yield return new Claim(MetadataInfo.ClaimTypes.UserId, this.Id.ToString(CultureInfo.InvariantCulture));

            if (this.Role != null)
            {
                yield return new Claim(ClaimTypes.Role, this.Role.Name);

                if (this.Role.Client != null)
                {
                    var value = this.Role.Client.Id.ToString(CultureInfo.InvariantCulture);
                    yield return new Claim(MetadataInfo.ClaimTypes.ClientId, value);
                }

                if (this.Role.Permissions != null)
                {
                    foreach (var permission in this.Role.Permissions)
                    {
                        var accessRight = permission.Name;
                        var type = MetadataInfo.ClaimTypes.ToClaim(accessRight);
                        yield return new Claim(type, "1");
                    }
                }
            }
        }
    }
}