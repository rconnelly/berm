namespace Quad.Berm.Web.Areas.Main.Controllers
{
    using System;
    using System.IdentityModel.Services;
    using System.Net;
    using System.Security.Permissions;
    using System.Web.Mvc;

    using Microsoft.Practices.Unity;

    public class HomeController : Controller
    {
        [Dependency]
        public HomeManager Manager { get; set; }

        public ActionResult LogOff()
        {
            this.Manager.SignOut();

            return this.RedirectToAction("Index", "Home");
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult UnhandledError()
        {
            throw new Exception("bla-la");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Unauthorized()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Admin")]
        public ActionResult Principal()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [ClaimsPrincipalPermission(SecurityAction.Demand, Resource = "resource1", Operation = "action1")]
        public ActionResult Claims()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
