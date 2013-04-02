  namespace Ach.Fulfillment.Data
  {
  public enum AccessRight
  {
          SuperAdmin,
        LocalAdmin,
        User,
    }

    public static class AccessRightRegistry
    {
        public const string SuperAdmin = "SuperAdmin";

        public const string LocalAdmin = "LocalAdmin";

        public const string User = "User";
    }
}