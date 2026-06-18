using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        /// The correlation id included in the request.
        /// </summary>
        public string CorrelationId { get; }
        
        /// <summary>
        /// The request id returned by the Api.
        /// </summary>
        public string RequestId { get; }

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
        /// <param name="requestHeaders">Headers from the http request</param>
        /// <param name="message">Exception message.</param>
        public VaultApiException(HttpStatusCode httpStatusCode, HttpRequestHeaders requestHeaders, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
            StatusCode = (int) httpStatusCode;

            if (requestHeaders != null && (
                requestHeaders.TryGetValues("x-correlation-id", out var correlationIdValues) ||
                requestHeaders.TryGetValues("correlation-id", out correlationIdValues)))
            {
                CorrelationId = string.Join(",", correlationIdValues);
            }

            if (!string.IsNullOrWhiteSpace(message))
            {
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(message);
                    RequestId = errorResponse?.RequestId;
                    ApiErrors = errorResponse?.Errors;
                    ApiWarnings = errorResponse?.Warnings;
                }
                catch
                {
                    // nothing to do.
                }
            }
        }

        private class ErrorResponse
        {
            [JsonPropertyName("request_id")]
            public string RequestId { get; set; }
            [JsonPropertyName("errors")]
            public string[] Errors { get; set; }
            [JsonPropertyName("warnings")]
            public string[] Warnings { get; set; }
        }
    }
}
