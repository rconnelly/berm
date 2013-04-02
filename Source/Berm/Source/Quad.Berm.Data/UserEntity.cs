namespace Quad.Berm.Data
{
    public class UserEntity : BaseEntity
    {
        public virtual RoleEntity Role { get; set; }

        public virtual string Name { get; set; }

        public virtual bool Disabled { get; set; }

        public virtual bool Deleted { get; set; }
    }
}