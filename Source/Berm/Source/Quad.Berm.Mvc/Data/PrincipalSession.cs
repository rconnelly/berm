namespace Quad.Berm.Mvc.Data
{
    public class PrincipalSession
    {
        #region Public Properties

        public string Email { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string[] Permissions { get; set; }

        public string[] Role { get; set; }

        public long UserId { get; set; }

        #endregion
    }
}