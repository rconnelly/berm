namespace Quad.Berm.Common.Security
{
    using System;
    using System.Security.Principal;

    [Obsolete("Use WIF")]
    public interface IApplicationIdentity : IIdentity
    {
        long UserId { get; }

        string Email { get; }

        string DisplayName { get; }
    }
}