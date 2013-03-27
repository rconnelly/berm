namespace Quad.Berm.Web
{
    using System;

    using Microsoft.WindowsAzure.Diagnostics;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class WebRole : RoleEntryPoint
    {
        #region Constants and Fields

        private const double DefaultLogTransferInterval = 1.0;

        #endregion

        #region Public Methods and Operators

        public override bool OnStart()
        {
            ConfigureDiagnosticMonitor();

            return base.OnStart();
        }

        #endregion

        #region Methods

        private static void ConfigureDiagnosticMonitor()
        {
            var config = DiagnosticMonitor.GetDefaultInitialConfiguration();
            config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(DefaultLogTransferInterval);
            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", config);
        }

        #endregion
    }
}