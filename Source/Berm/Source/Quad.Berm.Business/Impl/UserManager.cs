namespace Quad.Berm.Business.Impl
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Security.Claims;

    using Ach.Fulfillment.Data;

    using Microsoft.Practices.Unity;

    using Quad.Berm.Business.Impl.Validation;
    using Quad.Berm.Common.Security;
    using Quad.Berm.Data;
    using Quad.Berm.Data.Specifications;
    using Quad.Berm.Data.Specifications.User;

    internal class UserManager : ManagerBase<UserEntity>, IUserManager
    {
        #region Public Properties

        [Dependency]
        public IApplicationPrincipal Principal { get; set; }

        #endregion

        #region Public Methods and Operators

        public int Count()
        {
            var data = this.GetUserListQueryData();
            var result = this.Repository.Count(data);
            return result;
        }

        public IEnumerable<UserEntity> Enumerate(int pageIndex, int pageSize)
        {
            var data = this.GetUserListQueryData(pageIndex, pageSize);
            var result = this.Repository.Enumerable(data);
            return result;
        }

        public UserEntity FindActive(string provider, string identifier)
        {
            var user = this.Repository.FindOne(Users.ByCredentials(provider, identifier));
            return user;
        }

        public override UserEntity Create(UserEntity instance)
        {
            this.Validate(instance);
            var result = base.Create(instance);
            return result;
        }

        public override void Update(UserEntity instance)
        {
            this.Validate(instance);
            base.Update(instance);
        }

        public override void Delete(UserEntity instance)
        {
            Contract.Assert(instance != null);
            this.Validate(instance);
            instance.Deleted = true;
            base.Update(instance);
        }

        #endregion

        #region Methods

        private void Validate(UserEntity instance)
        {
            Contract.Assert(instance != null);

            this.Principal.DemandAny(AccessRight.ManageSuperAdmin, AccessRight.ManageLocalAdmin, AccessRight.ManageUser);

            var requiresSuperAdmin = instance.Role != null
                                     && (instance.Role.HasAny(AccessRight.ManageSuperAdmin)
                                         || instance.Role.Client == null
                                         || instance.Role.Client.Id != this.Principal.Identity.GetClientId());
            var requiresLocalAdmin = instance.Role != null && !requiresSuperAdmin
                                     && instance.Role.HasAny(AccessRight.ManageLocalAdmin);
            
            this.Principal.Demand(AccessRight.ManageSuperAdmin, requiresSuperAdmin);
            this.Principal.Demand(AccessRight.ManageLocalAdmin, requiresLocalAdmin);

            this.DemandValid<UserValidator, UserEntity>(instance);
        }

        private IQueryData<UserEntity> GetUserListQueryData(int pageIndex = 0, int pageSize = 100)
        {
            this.Principal.DemandAny(AccessRight.ManageSuperAdmin, AccessRight.ManageLocalAdmin);

            IQueryData<UserEntity> data;
            if (this.Principal.HasPermission(AccessRight.ManageSuperAdmin))
            {
                data = Users.All(pageIndex, pageSize);
            }
            else
            {
                var clientId = this.Principal.Identity.GetClientId();
                Contract.Assert(clientId != null);
                data = Users.ByClientId(clientId.Value, pageIndex, pageSize);
            }

            return data;
        }

        #endregion
    }
}