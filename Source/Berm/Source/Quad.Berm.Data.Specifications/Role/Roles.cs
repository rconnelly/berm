namespace Quad.Berm.Data.Specifications.Role
{
    using System;
    using System.Diagnostics.Contracts;

    public static class Roles
    {
        public static IQueryData<RoleEntity> ByType(RoleSet option, long? clientId)
        {
            IQueryData<RoleEntity> data;
            switch (option)
            {
                case RoleSet.Available:
                case RoleSet.Full:
                    data = All();
                    break;
                case RoleSet.AllSuperAdmins:
                    data = SuperAdmin();
                    break;
                case RoleSet.AllLocalAdmins:
                    data = LocalAdmin();
                    break;
                case RoleSet.AllLocalUsers:
                    data = LocalUser();
                    break;
                case RoleSet.BindedLocalAdmins:
                    {
                        Contract.Assert(clientId != null && clientId.Value > 0);
                        data = LocalAdmin(clientId.Value);
                        break;
                    }

                case RoleSet.BindedLocalAdminsAndUsers:
                    {
                        Contract.Assert(clientId != null && clientId.Value > 0);
                        data = LocalAdminAndUser(clientId.Value);
                        break;
                    }

                case RoleSet.BindedLocalUsers:
                    {
                        Contract.Assert(clientId != null && clientId.Value > 0);
                        data = LocalUser(clientId.Value);
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException("option");
            }

            return data;
        }

        public static IQueryData<RoleEntity> All()
        {
            return new AllRoles();
        }

        public static IQueryData<RoleEntity> SuperAdmin()
        {
            return new SuperAdminRoles();
        }

        public static IQueryData<RoleEntity> LocalAdmin(long clientId = 0)
        {
            return new LocalAdminRoles { ClientId = clientId };
        }

        public static IQueryData<RoleEntity> LocalUser(long clientId = 0)
        {
            return new LocalUserRoles { ClientId = clientId };
        }

        public static IQueryData<RoleEntity> LocalAdminAndUser(long clientId)
        {
            return new LocalAdminAndUserRoles { ClientId = clientId };
        }
    }
}