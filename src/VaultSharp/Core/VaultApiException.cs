using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;

namespace VaultSharp.Core
{
    /// <summary>
    /// The vault client exception
    /// </summary>
    public class VaultApiException : Exception
    {
        /// <summary>
        /// The status code returned by Api.
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// The http status code returned by Api.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; }

        /// <summary>
        /// The list of api errors.
        /// </summary>
        public IEnumerable<string> ApiErrors { get; }

        /// <summary>
        /// The list of api warnings.
        /// </summary>
        public IEnumerable<string> ApiWarnings { get; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public VaultApiException()
        {
        }

        /// <summary>
        /// Message constructor.
        /// </summary>
        /// <param name="message">Exception message.</param>
        public VaultApiException(string message) : base(message)
        {
        }

        /// <summary>
        /// Message constructor.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="innerException">Inner exception</param>
        public VaultApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Status code based exception.
        /// </summary>
        /// <param name="httpStatusCode">Http status code.</param>
        /// <param name="message">Exception message.</param>
        public VaultApiException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
            StatusCode = (int) HttpStatusCode;

            try
            {
                using (var document = JsonDocument.Parse(message))
                {
                var root = document.RootElement;

                if (root.TryGetProperty("errors", out var errorsElement) && errorsElement.ValueKind == JsonValueKind.Array)
                {
                    var errors = new List<string>();
                    foreach (var item in errorsElement.EnumerateArray())
                    {
                        errors.Add(item.GetString());
                    }
                    ApiErrors = errors;
                }

                if (root.TryGetProperty("warnings", out var warningsElement) && warningsElement.ValueKind == JsonValueKind.Array)
                {
                    var warnings = new List<string>();
                    foreach (var item in warningsElement.EnumerateArray())
                    {
                        warnings.Add(item.GetString());
                    }
                    ApiWarnings = warnings;
                }
                }
            }
            catch
            {
                // nothing to do.
            }
        }
    }
}
