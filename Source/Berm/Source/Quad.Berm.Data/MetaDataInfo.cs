namespace Quad.Berm.Data
{
    using System.Diagnostics.CodeAnalysis;

    public static class MetadataInfo
    {
        public const string UnitOfWorkCache = "UnitOfWork";

        #region Enums

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Justification = "As designed")]
        public enum InvalidState
        {
            AlreadyExist,

            Invalid,

            NotExist
        }

        #endregion

    }
}