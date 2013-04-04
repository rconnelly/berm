namespace Quad.Berm.Data
{
    using Ach.Fulfillment.Data;

    public class PermissionEntity : BaseEntity
    {
        public virtual AccessRight Name { get; set; }
    }
}