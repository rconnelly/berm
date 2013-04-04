namespace Quad.Berm.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;

    using Microsoft.Practices.Unity;

    using Quad.Berm.Common.Unity;

    internal class UnityDependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer container;

        public UnityDependencyResolver(IUnityContainer container)
        {
            Contract.Assert(container != null);
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            var service = this.container.TryResolve(serviceType);
            return service;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var service = this.container.TryResolve(serviceType);
            if (service != null)
            {
                yield return service;
            }

            foreach (var instance in this.container.ResolveAll(serviceType))
            {
                yield return instance;
            }
        }
    }
}