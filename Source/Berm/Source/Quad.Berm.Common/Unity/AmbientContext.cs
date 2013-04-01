namespace Quad.Berm.Common.Unity
{
    using System;

    public sealed class AmbientContext : IDisposable
    {
        #region Constructors

        public AmbientContext()
        {
            AmbientContextLifetimeManager.Store.Enable();
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            AmbientContextLifetimeManager.Store.Clear();
            AmbientContextLifetimeManager.Store.Disable();
        }

        #endregion
    }
}