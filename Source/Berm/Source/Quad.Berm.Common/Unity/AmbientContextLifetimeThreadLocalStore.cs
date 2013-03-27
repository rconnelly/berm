namespace Quad.Berm.Common.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class AmbientContextLifetimeThreadLocalStore : AmbientContextLifetimeStore, IDisposable
    {
        #region Fields

        private ThreadLocal<bool> enabled = new ThreadLocal<bool>();

        private ThreadLocal<Dictionary<Guid, object>> values = new ThreadLocal<Dictionary<Guid, object>>();

        #endregion

        #region Constructors and Destructors

        ~AmbientContextLifetimeThreadLocalStore()
        {
            this.Dispose(false);
        }

        #endregion

        #region Properties

        protected override bool Enabled
        {
            get
            {
                return this.enabled.Value;
            }

            set
            {
                this.enabled.Value = value;
            }
        }

        protected override Dictionary<Guid, object> Values
        {
            get
            {
                return this.values.Value;
            }

            set
            {
                this.values.Value = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.enabled != null)
                {
                    this.enabled.Dispose();
                    this.enabled = null;
                }

                if (this.values != null)
                {
                    this.values.Dispose();
                    this.values = null;
                }
            }
        }

        #endregion
    }
}