namespace Quad.Berm.Web.Areas.Main.Controllers
{
    using System.Web.Mvc;

    using Microsoft.Practices.Unity;

    public class HomeController : Controller
    {
        #region Public Properties

        [Dependency]
        public HomeManager Manager { get; set; }

        #endregion

        #region Public Methods and Operators

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult LogOff()
        {
            this.Manager.SignOut();

            return this.RedirectToAction("Index", "Home");
        }

        #endregion
    }
}