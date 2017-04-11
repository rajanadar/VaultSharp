using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Models
{
    /// <summary>
    /// Represents a Vault Secret with lease information and generic data.
    /// </summary>
    /// <typeparam name="TData">The type of the data contained in the secret.</typeparam>
    public class Secret<TData>
    {
        /// <summary>
        /// Gets or sets the request identifier.
        /// </summary>
        /// <value>
        /// The request identifier.
        /// </value>
        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the lease identifier.
        /// </summary>
        /// <value>
        /// The lease identifier.
        /// </value>
        [JsonProperty("lease_id")]
        public string LeaseId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Secret{TData}"/> is renewable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if renewable; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("renewable")]
        public bool Renewable { get; set; }

        /// <summary>
        /// Gets or sets the lease duration seconds.
        /// </summary>
        /// <value>
        /// The lease duration seconds.
        /// </value>
        [JsonProperty("lease_duration")]
        public int LeaseDurationSeconds { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonProperty("data")]
        public TData Data { get; set; }

        /// <summary>
        /// Gets or sets the wrapped information.
        /// </summary>
        /// <value>
        /// The wrapped information.
        /// </value>
        [JsonProperty("wrap_info")]
        public WrapInfo WrappedInformation { get; set; }

        /// <summary>
        /// Gets or sets the warnings.
        /// </summary>
        /// <value>
        /// The warnings.
        /// </value>
        [JsonProperty("warnings")]
        public List<string> Warnings { get; set; }

        /// <summary>
        /// Gets or sets the authorization information.
        /// </summary>
        /// <value>
        /// The authorization information.
        /// </value>
        [JsonProperty("auth")]
        public AuthorizationInfo AuthorizationInfo { get; set; }
    }
}