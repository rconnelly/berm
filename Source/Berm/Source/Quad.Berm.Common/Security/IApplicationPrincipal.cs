namespace Quad.Berm.Common.Security
{
    using System;
    using System.Security.Principal;

    [Obsolete("Use WIF")]
    public interface IApplicationPrincipal : IPrincipal
    {
        new IApplicationIdentity Identity { get; }

        bool HasPermission(string permission);
    }
}