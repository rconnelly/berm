namespace Quad.Berm.Web.Areas.Main.Controllers
{
    using System.IdentityModel.Services;

    public class HomeManager
    {
        public void SignOut()
        {
            FederatedAuthentication.WSFederationAuthenticationModule.SignOut(false);
        }
    }
}