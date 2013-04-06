namespace Quad.Berm.Web.Areas.Manage.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.Practices.Unity;

    using global::Mvc.JQuery.Datatables;

    using Quad.Berm.Business;
    using Quad.Berm.Common.Transactions;
    using Quad.Berm.Data;
    using Quad.Berm.Web.Areas.Manage.Models;

    public class MembershipManager
    {
        #region Properties

        [Dependency]
        public IUserManager UserManager { get; set; }

        [Dependency]
        public IRoleManager RoleManager { get; set; }

        [Dependency]
        public IClientManager ClientManager { get; set; }

        #endregion

        #region Methods

        public JsonResult GetListModel(DataTablesParam dataTableParam)
        {
            Contract.Assert(dataTableParam != null);

            // without AsQueryable "Count()" will be executed on application level instead of db level
            var list = this.UserManager.Enumerate().AsQueryable();

            var query = (from entity in list
                         select new UserGridModel
                         {
                             Id = entity.Id,
                             Name = entity.Name,
                             Identifier = entity.StsCredentials.Min(c => c.Identifier),
                             Role = entity.Role.Name,
                             Client = entity.Role.Client.Name,
                             Disabled = entity.Disabled
                         }).AsQueryable();

            var result = DataTablesResult.Create(query, dataTableParam);

            return result;
        }

        public UserModel GetEditModel(long id)
        {
            var user = this.UserManager.Load(id);
            var model = new UserModel
                            {
                                Id = user.Id,
                                Name = user.Name,
                                Identifier = user.StsCredentials.Select(c => c.Identifier).FirstOrDefault(),
                                Role = user.Role.Id,
                                Client = user.Role.Client == null ? 0 : user.Role.Client.Id,
                                Disabled = user.Disabled
                            };

            return model;
        }

        public IEnumerable<ClientEntity> GetClients(UserModelOption option)
        {
            var result = this.ClientManager.Enumerate();
            return result;
        }

        public IQueryable<RoleEntity> GetRoles(UserModelOption option, long clientId)
        {
            var roleSet = ToRoleSet(option);
            var query = this.RoleManager.Enumerate(roleSet).AsQueryable();

            switch (option)
            {
                case UserModelOption.None:
                    {
                        if (clientId == 0)
                        {
                            query = from r in query where r.Client == null select r;
                        }
                        else
                        {
                            query = from r in query where r.Client.Id == clientId select r;
                        }

                        break;
                    }

                case UserModelOption.AllLocalAdmins:
                case UserModelOption.AllLocalUsers:
                    {
                        if (clientId == 0)
                        {
                            query = (new RoleEntity[0]).AsQueryable();
                        }
                        else
                        {
                            query = from r in query where r.Client.Id == clientId select r;
                        }

                        break;
                    }
            }

            return query;
        }

        public void CreateUser(UserModel model)
        {
            Contract.Assert(model.Id == 0);

            var role = this.RoleManager.Get(model.Role);
            var user = new UserEntity
                           {
                               Name = model.Name, 
                               Role = role, 
                               Disabled = model.Disabled
                           };
            user.AddCredential(new StsCredentialEntity { Provider = MetadataInfo.DefaultIdentityProvider, Identifier = model.Identifier });
            
            using (var tx = new Transaction())
            {
                this.UserManager.Create(user);

                tx.Complete();
            }

            model.Id = user.Id;
        }

        public void UpdateUser(UserModel model)
        {
            Contract.Assert(model.Id > 0);

            var user = this.UserManager.Load(model.Id);
            var role = this.RoleManager.Get(model.Role);

            user.Name = model.Name;
            user.Role = role;
            user.Disabled = model.Disabled;
            var credential = user.StsCredentials.FirstOrDefault();
            if (credential == null)
            {
                user.AddCredential(new StsCredentialEntity { Provider = MetadataInfo.DefaultIdentityProvider, Identifier = model.Identifier });
            }
            else
            {
                credential.Identifier = model.Identifier;
            }

            using (var tx = new Transaction())
            {
                this.UserManager.Update(user);

                tx.Complete();
            }
        }

        public void DeleteUser(long id)
        {
            var user = this.UserManager.Load(id);
            using (var tx = new Transaction())
            {
                this.UserManager.Delete(user);

                tx.Complete();
            }
        }

        private static RoleSet ToRoleSet(UserModelOption option)
        {
            RoleSet roleSet;
            switch (option)
            {
                case UserModelOption.None:
                    roleSet = RoleSet.Available;
                    break;
                case UserModelOption.AllSuperAdmins:
                    roleSet = RoleSet.AllSuperAdmins;
                    break;
                case UserModelOption.AllLocalAdmins:
                    roleSet = RoleSet.AllLocalAdmins;
                    break;
                case UserModelOption.AllLocalUsers:
                    roleSet = RoleSet.AllLocalUsers;
                    break;
                case UserModelOption.BindedLocalAdmins:
                    roleSet = RoleSet.BindedLocalAdmins;
                    break;
                case UserModelOption.BindedLocalUsers:
                    roleSet = RoleSet.BindedLocalUsers;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("option");
            }

            return roleSet;
        }

        #endregion
    }
}