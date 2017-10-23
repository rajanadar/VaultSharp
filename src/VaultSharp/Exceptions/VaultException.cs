using System;
using System.Runtime.Serialization;

namespace VaultSharp.Exceptions
{
    /// <summary>
    ///     Base exception class for Vault client errors.
    /// </summary>
    public class VaultException : Exception
    {
        /// <inheritdoc />
        protected VaultException()
        {
        }

        /// <inheritdoc />
        protected VaultException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public VaultException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <inheritdoc />
        protected VaultException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}