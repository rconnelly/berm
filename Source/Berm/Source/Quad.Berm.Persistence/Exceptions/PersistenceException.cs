namespace Quad.Berm.Persistence.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    using Quad.Berm.Common.Exceptions;

    [Serializable]
    public class PersistenceException : RootException
    {
        #region Constructors

        public PersistenceException()
        {
        }

        public PersistenceException(string message)
            : base(message)
        {
        }

        public PersistenceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PersistenceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}