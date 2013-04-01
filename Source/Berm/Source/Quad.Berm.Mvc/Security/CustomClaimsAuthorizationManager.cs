namespace Quad.Berm.Mvc.Security
{
    using System.Security.Claims;

    public class CustomClaimsAuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            // todo: check role related permissions
            var granted = base.CheckAccess(context);
            return granted;
        }
    }
}