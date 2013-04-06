namespace Quad.Berm.Web.Areas.Manage.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using Microsoft.Practices.Unity;

    using global::Mvc.JQuery.Datatables;

    using Quad.Berm.Business.Exceptions;
    using Quad.Berm.Data;
    using Quad.Berm.Mvc;
    using Quad.Berm.Mvc.Data;
    using Quad.Berm.Web.Areas.Manage.Models;
    using Quad.Berm.Web.Mvc.Helpers;

    public class MembershipController : Controller
    {
        #region Properties

        [Dependency]
        public MembershipManager Manager { get; set; }

        #endregion

        #region Methods

        #region Index

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public JsonResult Index(DataTablesParam dataTableParam)
        {
            var model = this.Manager.GetListModel(dataTableParam);
            return model;
        }

        #endregion

        #region Create

        [HttpGet]
        public ActionResult Create(UserModelOption option = UserModelOption.None)
        {
            var model = new UserModel { Option = option };

            this.PopulateDropDownLists(model);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Create(UserModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    this.Manager.CreateUser(model);

                    this.TempData["CompleteMessage"] = "User successfully created.";
                    return this.RedirectToAction("Index");
                }
                catch (BusinessValidationException exc)
                {
                    this.ModelState.FillFrom(exc);
                }
            }

            this.PopulateDropDownLists(model);

            return this.View(model);
        }

        #endregion

        #region Edit

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var model = this.Manager.GetEditModel(id);

            this.PopulateDropDownLists(model);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    this.Manager.UpdateUser(model);

                    this.TempData["CompleteMessage"] = "User successfully updated.";
                    return this.RedirectToAction("Index");
                }
                catch (BusinessValidationException exc)
                {
                    this.ModelState.FillFrom(exc);
                }
            }

            this.PopulateDropDownLists(model);

            return this.View(model);
        }

        #endregion

        #region Delete

        [HttpPost]
        [ApiValidationFilter]
        public ActionResult Delete(long id)
        {
            this.Manager.DeleteUser(id);
            return this.Json(new { message = "User successfully deleted" });
        }

        #endregion

        #region Helpers

        [HttpPost]
        [ApiValidationFilter]
        public ActionResult Roles(UserModelOption option, long clientId)
        {
            var roles = (from r in this.Manager.GetRoles(option, clientId)
                        select new { r.Id, r.Name })
                        .ToArray();
            return this.Json(roles);
        }

        private void PopulateDropDownLists(UserModel model)
        {
            var option = model.Option;
            
            var clients = this.Manager.GetClients(option);
            var clientId = model.Client;
            this.ViewBag.Client = clients.ToSelectList(
                clientId,
                (id, name) => new ClientEntity { Name = name, Id = id },
                (id, r) => r.Id == id);

            var roles = this.Manager.GetRoles(option, clientId).ToList();
            var roleAllowEmpty = roles.Count > 1;
            var roleId = model.Role;
            this.ViewBag.Role = roles.ToSelectList(
                roleId,
                (id, name) => new RoleEntity { Name = name, Id = id }, 
                (id, r) => r.Id == id,
                allowEmpty: roleAllowEmpty);
        }

        #endregion

        #endregion
    }
}
