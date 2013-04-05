namespace Quad.Berm.Web.Areas.Manage.Controllers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.Practices.Unity;

    using global::Mvc.JQuery.Datatables;

    using Quad.Berm.Business;
    using Quad.Berm.Web.Areas.Manage.Models;

    public class MembershipManager
    {
        [Dependency]
        public IUserManager Manager { get; set; }

        public JsonResult GetUsersGridModel(DataTablesParam dataTableParam)
        {
            Contract.Assert(dataTableParam != null);

            // without AsQueryable "Count()" will be executed on application level instead of db level
            var list = this.Manager.Enumerate().AsQueryable();

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
    }
}