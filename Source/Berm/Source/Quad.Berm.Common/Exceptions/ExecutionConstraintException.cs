namespace Quad.Berm.Common.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.Serialization;

    [Serializable]
    public class ExecutionConstraintException : RootException
    {
        #region Constructors and Destructors

        public ExecutionConstraintException()
        {
        }

        public ExecutionConstraintException(string message)
            : base(message)
        {
        }

        public ExecutionConstraintException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ExecutionConstraintException(Exception inner)
            : base("Constraint violation while operation execution", inner)
        {
        }

        protected ExecutionConstraintException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Contract.Assert(info != null);

            this.ConstraintName = info.GetString("ConstraintName");
        }

        #endregion

        #region Properties

        public string ConstraintName { get; set; }

        #endregion

        #region Methods

        [SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison", MessageId = "System.String.Equals(System.String,System.String,System.StringComparison)", Justification = "As designed")]
        public bool Match(string constraintName)
        {
            Contract.Assert(constraintName != null);
            return string.Equals(this.ConstraintName, constraintName, StringComparison.InvariantCulture);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ConstraintName", this.ConstraintName);
        }

        #endregion
    }
}