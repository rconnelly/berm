namespace Quad.Berm.Web.Areas.Main.Controllers
{
    using System.Web.Mvc;

    using Microsoft.Practices.Unity;

    using Quad.Berm.Mvc;
    using Quad.Berm.Web.Areas.Main.Models;

    public class HomeController : Controller
    {
        [Dependency]
        public HomeManager Manager { get; set; }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;

            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [WebValidationFilter]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                if (this.Manager.Login(model))
                {
                    if (this.Url.IsLocalUrl(returnUrl))
                    {
                        return this.Redirect(returnUrl);
                    }

                    return this.RedirectToAction("Index", "Home");
                }

                this.ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
            }

            return this.View(model);
        }

        public ActionResult LogOff()
        {
            this.Manager.Logout();

            return this.RedirectToAction("Index", "Home");
        }

        public ActionResult Index()
        {
            return this.View();
        }
    }
}
