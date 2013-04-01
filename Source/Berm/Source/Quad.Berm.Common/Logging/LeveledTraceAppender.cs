namespace Quad.Berm.Common.Logging
{
    using System.Diagnostics;

    using log4net.Appender;
    using log4net.Core;

    public sealed class LeveledTraceAppender : TraceAppender
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            var level = loggingEvent.Level;
            var message = this.RenderLoggingEvent(loggingEvent);
            if (level >= Level.Error)
            {
                Trace.TraceError(message);
            }
            else if (level >= Level.Warn)
            {
                Trace.TraceWarning(message);
            }
            else if (level >= Level.Info)
            {
                Trace.TraceInformation(message);
            }
            else
            {
                Trace.Write(message);
            }

            Trace.Flush();
        }
    }
}
