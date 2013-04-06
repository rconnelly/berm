namespace Quad.Berm.Business.Impl
{
    using System.Collections.Generic;
    using System.Security.Principal;

    using Microsoft.Practices.Unity;

    using Quad.Berm.Data;
    using Quad.Berm.Data.Specifications;
    using Quad.Berm.Data.Specifications.Role;

    internal class RoleManager : ManagerBase<RoleEntity>, IRoleManager
    {
        #region Public Properties

        [Dependency]
        public IPrincipal Principal { get; set; }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<RoleEntity> Enumerate(RoleSet option)
        {
            var queryData = this.GetRoleListQueryData(option);
            var result = this.Repository.Enumerable(queryData);
            return result;
        }

        public RoleEntity Get(long roleId)
        {
            var role = this.Repository.Get<RoleEntity>(roleId);
            return role;
        }

        private IQueryData<RoleEntity> GetRoleListQueryData(RoleSet option)
        {
            if (option == RoleSet.Available)
            {
                if (this.Principal.HasPermission(AccessRight.ManageSuperAdmin))
                {
                    option = RoleSet.Full;
                }
                else if (this.Principal.HasPermission(AccessRight.ManageLocalAdmin))
                {
                    option = RoleSet.BindedLocalAdminsAndUsers;
                }
                else
                {
                    option = RoleSet.BindedLocalUsers;
                }
            }

            switch (option)
            {
                case RoleSet.Full:
                case RoleSet.AllSuperAdmins:
                case RoleSet.AllLocalAdmins:
                case RoleSet.AllLocalUsers:
                    this.Principal.Demand(AccessRight.ManageSuperAdmin);
                    break;
                case RoleSet.BindedLocalAdmins:
                case RoleSet.BindedLocalAdminsAndUsers:
                    this.Principal.DemandAny(AccessRight.ManageSuperAdmin, AccessRight.ManageLocalAdmin);
                    break;
                case RoleSet.BindedLocalUsers:
                    this.Principal.DemandAny(AccessRight.ManageSuperAdmin, AccessRight.ManageLocalAdmin, AccessRight.ManageUser);
                    break;
                default:
                    this.Principal.Demand(AccessRight.ManageSuperAdmin);
                    break;
            }

            var data = Roles.ByType(option, this.Principal.Identity.GetClientId());
            
            return data;
        }

        #endregion
    }
}