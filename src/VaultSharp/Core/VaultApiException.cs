using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VaultSharp.Core
{
    public class VaultApiException : Exception
    {
        public int StatusCode { get; }

        public HttpStatusCode HttpStatusCode { get; }

        public IEnumerable<string> ApiErrors { get; }

        public IEnumerable<string> ApiWarnings { get; }

        public VaultApiException()
        {
        }

        public VaultApiException(string message) : base(message)
        {
        }

        public VaultApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public VaultApiException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
            StatusCode = (int) HttpStatusCode;

            try
            {
                var structured = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<string>>>(message);

                if (structured.ContainsKey("errors"))
                {
                    ApiErrors = structured["errors"];
                }

                if (structured.ContainsKey("warnings"))
                {
                    ApiWarnings = structured["warnings"];
                }
            }
            catch
            {
                // nothing to do.
            }
        }

        protected VaultApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
