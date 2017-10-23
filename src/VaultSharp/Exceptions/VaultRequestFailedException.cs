using System;
using System.Net;

namespace VaultSharp.Exceptions
{
    /// <summary>
    ///     Error making request to Vault server.
    /// </summary>
    public class VaultRequestFailedException : VaultException
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="VaultRequestFailedException" /> class.
        /// </summary>
        /// <param name="message">User message.</param>
        /// <param name="statusCode">Http code.</param>
        /// <param name="response">Server response.</param>
        public VaultRequestFailedException(string message, HttpStatusCode statusCode, string response) : base(message)
        {
            StatusCode = statusCode;
            Response = response ?? string.Empty;
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="VaultRequestFailedException" /> class.
        /// </summary>
        /// <param name="message">User message.</param>
        /// <param name="statusCode">Http code.</param>
        /// <param name="response">Server response.</param>
        /// <param name="innerException">Inner exception.</param>
        public VaultRequestFailedException(string message, HttpStatusCode statusCode, string response,
            Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
            Response = response;
        }

        /// <summary>
        ///     Http code returned by Vault server.
        /// </summary>
        public HttpStatusCode StatusCode { get; protected set; }

        /// <summary>
        ///     Vault server response.
        /// </summary>
        public string Response { get; protected set; }
    }
}