namespace Quad.Berm.Mvc.Data
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public class OperationError
    {
        public OperationError()
        {
            this.Errors = new OperationErrorEntry[0];
        }

        public OperationError(string code, string message) : this()
        {
            Contract.Assert(code != null);
            Contract.Assert(message != null);
            this.Message = message;
            this.ErrorCode = code;
        }

        public string ErrorCode { get; set; }

        public string Message { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "As Designed")]
        public OperationErrorEntry[] Errors { get; set; }
    }
}