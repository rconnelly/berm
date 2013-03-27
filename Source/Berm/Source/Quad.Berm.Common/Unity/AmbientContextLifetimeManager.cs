namespace Quad.Berm.Common.Unity
{
    using System;

    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;

    public class AmbientContextLifetimeManager : LifetimeManager
    {
        #region Constants and Fields

        private static Lazy<AmbientContextLifetimeStore> storeBuilder = new Lazy<AmbientContextLifetimeStore>(() => ServiceLocator.Current.GetInstance<AmbientContextLifetimeStore>());

        private readonly Guid key = Guid.NewGuid();

        #endregion

        #region Properties

        public static AmbientContextLifetimeStore Store
        {
            get
            {
                return storeBuilder.Value;
            }

            set
            {
                var newBuilder = value != null 
                    ? new Lazy<AmbientContextLifetimeStore>(() => value)
                    : new Lazy<AmbientContextLifetimeStore>(() => ServiceLocator.Current.GetInstance<AmbientContextLifetimeStore>());

                if (storeBuilder.IsValueCreated)
                {
                    storeBuilder.Value.Disable();
                    storeBuilder.Value.Clear();
                }

                storeBuilder = newBuilder;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override object GetValue()
        {
            var result = Store.GetValue(this.key);
            return result;
        }

        public override void RemoveValue()
        {
            Store.RemoveValue(this.key);
        }

        public override void SetValue(object newValue)
        {
            Store.SetValue(this.key, newValue);
        }

        #endregion
    }
}