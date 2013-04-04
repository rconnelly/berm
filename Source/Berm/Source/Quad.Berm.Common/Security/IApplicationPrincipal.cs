namespace Quad.Berm.Common.Security
{
    using System.Security.Claims;
    using System.Security.Principal;

    public interface IApplicationPrincipal : IPrincipal
    {
        new ClaimsIdentity Identity { get; }

        bool HasPermission(string permission);
    }
}