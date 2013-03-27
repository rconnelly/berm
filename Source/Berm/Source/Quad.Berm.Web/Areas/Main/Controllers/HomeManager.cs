namespace Quad.Berm.Web.Areas.Main.Controllers
{
    using System;
    using System.Runtime.Caching;
    using System.Web.Security;

    using Microsoft.Practices.Unity;

    using Quad.Berm.Common.Security;
    using Quad.Berm.Web.Areas.Main.Models;

    public class HomeManager
    {
        //[Dependency]
        //public IApplicationPrincipal Principal { get; set; }

        [Dependency]
        public ObjectCache Cache { get; set; }

        public bool Login(LoginModel model)
        {
            var authenticated = string.Equals(model.UserName, model.Password);
            if (authenticated)
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
            }

            return authenticated;
        }

        public void Logout()
        {
            // this.Cache.Remove(this.Principal.Identity.Name);

            FormsAuthentication.SignOut();
        }
    }
}