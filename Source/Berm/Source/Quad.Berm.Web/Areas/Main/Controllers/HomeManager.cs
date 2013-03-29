namespace Quad.Berm.Web.Areas.Main.Controllers
{
    using System;
    using System.Configuration;
    using System.IdentityModel.Services;
    using System.Linq;

    public class HomeManager
    {
        public void SignOut()
        {
            // Load Identity Configuration
            var config = FederatedAuthentication.FederationConfiguration;

            // Get wtrealm from WsFederationConfiguation Section
            var wtrealm = config.WsFederationConfiguration.Realm;

            // Construct wreply value from wtrealm
            var wreply = wtrealm.Last().Equals('/') ? wtrealm + "Logout" : wtrealm + "/Logout";

            // Read the ACS Ws-Federation endpoint from web.Config
            var endpoint = ConfigurationManager.AppSettings["ida:Issuer"];

            var signoutRequestMessage = new SignOutRequestMessage(new Uri(endpoint));
            signoutRequestMessage.Parameters.Add("wreply", wreply);
            signoutRequestMessage.Parameters.Add("wtrealm", wtrealm);

            FederatedAuthentication.SessionAuthenticationModule.SignOut();
        }
    }
}