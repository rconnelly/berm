  namespace Ach.Fulfillment.Data
  {
  public enum AccessRight
  {
          ManageSuperAdmin,
        ManageLocalAdmin,
        ManageUser,
    }

    public static class AccessRightRegistry
    {
        public const string ManageSuperAdmin = "ManageSuperAdmin";

        public const string ManageLocalAdmin = "ManageLocalAdmin";

        public const string ManageUser = "ManageUser";
    }
}