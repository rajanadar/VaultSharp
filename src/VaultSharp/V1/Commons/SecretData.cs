using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.Commons
{
    /// <summary>
    /// Represents a Vault Secret Data.
    /// </summary>
    public class SecretData : SecretData<IDictionary<string, object>>
    {
    }

    public class SecretData<T>
    {
        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        [JsonPropertyName("data")]
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        [JsonPropertyName("metadata")]
        public CurrentSecretMetadata Metadata { get; set; }
    }
}
