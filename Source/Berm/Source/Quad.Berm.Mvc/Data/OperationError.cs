namespace Quad.Berm.Mvc.Data
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    // http://labs.omniti.com/labs/jsend
    public class OperationError
    {
        public OperationError()
        {
            this.Status = "error";
        }

        public OperationError(string code, string message) : this()
        {
            Contract.Assert(code != null);
            Contract.Assert(message != null);
            this.ErrorCode = code;
            this.Message = message;
        }

        public string Status { get; set; }

        public string ErrorCode { get; set; }

        public string Message { get; set; }

        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "As Designed")]
        public OperationErrorEntry[] Data { get; set; }
    }
}