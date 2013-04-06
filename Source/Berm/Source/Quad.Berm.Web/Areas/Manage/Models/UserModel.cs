namespace Quad.Berm.Web.Areas.Manage.Models
{
    using System.ComponentModel.DataAnnotations;

    using Quad.Berm.Data;

    public class UserModel
    {
        public UserModel()
        {
            this.Option = UserModelOption.None;
        }

        public UserModelOption Option { get; set; }

        public long Id { get; set; }

        [Required]
        [StringLength(MetadataInfo.StringNormal)]
        public string Name { get; set; }

        [Required]
        [StringLength(MetadataInfo.StringNormal)]
        [DataType(DataType.EmailAddress)]
        public string Identifier { get; set; }

        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "The Role Field is required")]
        public long Role { get; set; }

        public long Client { get; set; }

        public bool Disabled { get; set; }
    }
}