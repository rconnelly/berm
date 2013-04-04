namespace Quad.Berm.Business.Impl.Configuration
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Practices.Unity;

    using Quad.Berm.Common.Security;
    using Quad.Berm.Common.Unity;

    using Unity.AutoRegistration;

    public class BusinessContainerExtension : UnityContainerExtension
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "As designed")]
        protected override void Initialize()
        {
            this.WellKnownITypeNameAutoRegistration<ContainerControlledLifetimeManager>(WellKnownAppParts.Manager);
            this.Container.RegisterType<IApplicationPrincipal, ThreadApplicationPrincipal>(new ContainerControlledLifetimeManager());
        }
    }
}