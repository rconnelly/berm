namespace Quad.Berm.Business.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security;

    using Ach.Fulfillment.Data;

    using Quad.Berm.Common.Security;
    using Quad.Berm.Data;

    internal static class ClaimExtension
    {
        public static bool HasPermission(this IApplicationPrincipal principal, AccessRight accessRight)
        {
            Contract.Assert(principal != null);
            var permission = MetadataInfo.ClaimTypes.ToClaim(accessRight);
            var hasPermission = principal.HasPermission(permission);
            return hasPermission;
        }

        public static void Demand(this IApplicationPrincipal principal, AccessRight action, bool enabled)
        {
            principal.Demand(action, () => enabled);
        }

        public static void Demand(this IApplicationPrincipal principal, AccessRight action, Func<bool> condition = null)
        {
            principal.Demand(condition, Enumerable.All, action);
        }

        public static void DemandAny(this IApplicationPrincipal principal, params AccessRight[] actions)
        {
            principal.Demand(selector: Enumerable.Any, actions: actions);
        }

        public static void DemandAll(this IApplicationPrincipal principal, params AccessRight[] actions)
        {
            principal.Demand(selector: Enumerable.All, actions: actions);
        }

        public static void Demand(
            this IApplicationPrincipal principal, 
            Func<bool> condition = null,
            Func<IEnumerable<AccessRight>, Func<AccessRight, bool>, bool> selector = null,
            params AccessRight[] actions)
        {
            Contract.Assert(principal != null);
            Contract.Assert(actions != null);
            
            selector = selector ?? Enumerable.All;
            
            if (condition == null || condition())
            {
                if (!selector(actions, principal.HasPermission))
                {
                    throw new SecurityException();
                }
            }
        }
    }
}