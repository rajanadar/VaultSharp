using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents the Seal status of the Vault.
    /// </summary>
    public class SealStatus
    {
        /// <summary>
        /// Gets or sets a value indicating the type.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating thif initialized.
        /// </summary>
        [JsonPropertyName("initialized")]
        public bool Initialized { get; set; }

        /// <summary>
        /// Gets or sets a value indicating about the <see cref="SealStatus"/>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if sealed; otherwise, <c>false</c>.
        /// </value>
        [JsonPropertyName("sealed")]
        public bool Sealed { get; set; }

        /// <summary>
        /// Gets or sets the number of shares required to reconstruct the master key.
        /// </summary>
        /// <value>
        /// The secret threshold.
        /// </value>
        [JsonPropertyName("t")]
        public int SecretThreshold { get; set; }

        /// <summary>
        /// Gets or sets the number of shares to split the master key into.
        /// </summary>
        /// <value>
        /// The secret shares.
        /// </value>
        [JsonPropertyName("n")]
        public int SecretShares { get; set; }

        /// <summary>
        /// Gets or sets the number of shares that have been successfully applied to reconstruct the master key.
        /// When the value is about to reach <see cref="SecretThreshold"/>, the Vault will be unsealed and the value will become <value>0</value>.
        /// </summary>
        /// <value>
        /// The progress count.
        /// </value>
        [JsonPropertyName("progress")]
        public int Progress { get; set; }

        /// <summary>
        /// Gets or sets the nonce.
        /// </summary>
        /// <value>
        /// The nonce.
        /// </value>
        [JsonPropertyName("nonce")]
        public string Nonce { get; set; }

        /// <summary>
        /// Gets or sets the vault version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("build_date")]
        public DateTimeOffset BuildDate { get; set; }

        /// <summary>
        /// Gets or sets the migration.
        /// </summary>
        /// <value>
        /// The migration.
        /// </value>
        [JsonPropertyName("migration")]
        public bool Migration { get; set; }

        /// <summary>
        /// Gets or sets the recovery seal.
        /// </summary>
        /// <value>
        /// The recovery seal.
        /// </value>
        [JsonPropertyName("recovery_seal")]
        public bool RecoverySeal { get; set; }

        /// <summary>
        /// Gets or sets what type of storage the cluster is configured to use
        /// </summary>
        [JsonPropertyName("storage_type")]
        public string StorageType { get; set; }

        [JsonPropertyName("hcp_link_status")]
        public string HCPLinkStatus { get; set; }

        [JsonPropertyName("hcp_link_resource_ID")]
        public string HCPLinkResourceID { get; set; }

        [JsonPropertyName("warnings")]
        public List<string> Warnings { get; set; }

        [JsonPropertyName("ha_enabled")]
        public bool? HighAvailabilityEnabled { get; set; }

        [JsonPropertyName("active_time")]
        public DateTimeOffset? ActiveTime { get; set; }

        /// <summary>
        /// Gets or sets the name of the cluster.
        /// </summary>
        /// <value>
        /// The name of the cluster.
        /// </value>
        [JsonPropertyName("cluster_name")]
        public string ClusterName { get; set; }

        /// <summary>
        /// Gets or sets the cluster identifier.
        /// </summary>
        /// <value>
        /// The cluster identifier.
        /// </value>
        [JsonPropertyName("cluster_id")]
        public string ClusterId { get; set; }
    }
}