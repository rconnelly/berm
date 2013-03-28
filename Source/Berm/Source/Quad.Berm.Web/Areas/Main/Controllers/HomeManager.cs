namespace Quad.Berm.Web.Areas.Main.Controllers
{
    using System.Web.Security;

    using Quad.Berm.Web.Areas.Main.Models;

    public class HomeManager
    {
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
            FormsAuthentication.SignOut();
        }
    }
}