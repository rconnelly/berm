namespace Quad.Berm.Mvc.Data
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    using Quad.Berm.Business.Exceptions;

    public static class OperationErrorExtender
    {
        public static void Fill(this OperationError operationError, BusinessValidationException ex)
        {
            Contract.Assert(operationError != null);
            var r = from e in ex.Errors
                    select
                        new OperationErrorEntry
                            {
                                ErrorCode = e.ErrorCode,
                                FieldName = e.PropertyName,
                                Message = e.ErrorMessage
                            };

            var errors = r.ToArray();
            operationError.Status = "fail";
            operationError.ErrorCode = ex.GetType().Name;
            operationError.Message = operationError.Data.Select(e => e.Message).FirstOrDefault() ?? ex.Message;
            operationError.Data = errors;
        }
    }
}