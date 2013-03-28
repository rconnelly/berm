namespace Quad.Berm.Mvc.Data
{
    using System.Diagnostics.CodeAnalysis;

    public class PrincipalSession
    {
        #region Public Properties

        public string Email { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "As Designed")]
        public string[] Permissions { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "As Designed")]
        public string[] Role { get; set; }

        public long UserId { get; set; }

        #endregion
    }
}