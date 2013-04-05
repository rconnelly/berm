  namespace Quad.Berm.Data
  {
  public enum AccessRight
  {
          ManageSecurity,
        ManageSuperAdmin,
        ManageLocalAdmin,
        ManageUser,
    }

    public static class AccessRightRegistry
    {
        public const string ManageSecurity = "ManageSecurity";

        public const string ManageSuperAdmin = "ManageSuperAdmin";

        public const string ManageLocalAdmin = "ManageLocalAdmin";

        public const string ManageUser = "ManageUser";
    }
}