namespace Quad.Berm.Web.Areas.Manage.Models
{
    public class UserGridModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Identifier { get; set; }

        public string Role { get; set; }

        public string Client { get; set; }

        public bool Disabled { get; set; }
    }
}