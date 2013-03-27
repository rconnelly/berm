namespace Quad.Berm.Common.Exceptions
{
    using System;
    using System.IO;

    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

    // THIS CODE IS NEVER TESTED - WANT TO KEEP FOR A WHILE
    public class AggregatedExceptionTextFormatter : TextExceptionFormatter
    {
        #region Constructors and Destructors

        public AggregatedExceptionTextFormatter(TextWriter writer, Exception exception, Guid handlingInstanceId)
            : base(writer, exception, handlingInstanceId)
        {
        }

        #endregion

        #region Methods

        protected override void WriteException(Exception exceptionToFormat, Exception outerException)
        {
            // Trace.WriteLine("!!! writing base exception !!!");
            base.WriteException(exceptionToFormat, outerException);

            var aggregateException = exceptionToFormat as AggregateException;
            if (aggregateException != null)
            {
                foreach (Exception innerException in aggregateException.InnerExceptions)
                {
                    // Trace.WriteLine("!!! writing inner exception !!!");
                    base.WriteException(innerException, aggregateException);
                }
            }
        }

        #endregion
    }
}