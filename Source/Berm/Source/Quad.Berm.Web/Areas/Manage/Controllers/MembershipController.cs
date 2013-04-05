namespace Quad.Berm.Web.Areas.Manage.Controllers
{
    using System.Web.Mvc;

    using Microsoft.Practices.Unity;

    using global::Mvc.JQuery.Datatables;

    public class MembershipController : Controller
    {
        [Dependency]
        public MembershipManager Manager { get; set; }

        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public JsonResult Index(DataTablesParam dataTableParam)
        {
            var model = this.Manager.GetUsersGridModel(dataTableParam);
            return model;
        }
    }
}
