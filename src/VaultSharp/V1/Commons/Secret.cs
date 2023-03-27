﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using VaultSharp.V1.SecretsEngines;

namespace VaultSharp.V1.Commons
{
    /// <summary>
    /// Represents a Vault SecretsEngine with lease information and generic data.
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
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets the lease identifier.
        /// </summary>
        /// <value>
        /// The lease identifier.
        /// </value>
        [JsonPropertyName("lease_id")]
        public string LeaseId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SecretsEngine"/> is renewable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if renewable; otherwise, <c>false</c>.
        /// </value>
        [JsonPropertyName("renewable")]
        public bool Renewable { get; set; }

        /// <summary>
        /// Gets or sets the lease duration seconds.
        /// </summary>
        /// <value>
        /// The lease duration seconds.
        /// </value>
        [JsonPropertyName("lease_duration")]
        public int LeaseDurationSeconds { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonPropertyName("data")]
        public TData Data { get; set; }

        /// <summary>
        /// Gets or sets the wrapped information.
        /// </summary>
        /// <value>
        /// The wrapped information.
        /// </value>
        [JsonPropertyName("wrap_info")]
        public WrapInfo WrapInfo { get; set; }

        /// <summary>
        /// Gets or sets the warnings.
        /// </summary>
        /// <value>
        /// The warnings.
        /// </value>
        [JsonPropertyName("warnings")]
        public List<string> Warnings { get; set; }

        /// <summary>
        /// Gets or sets the authorization information.
        /// </summary>
        /// <value>
        /// The authorization information.
        /// </value>
        [JsonPropertyName("auth")]
        public AuthInfo AuthInfo { get; set; }
    }
}