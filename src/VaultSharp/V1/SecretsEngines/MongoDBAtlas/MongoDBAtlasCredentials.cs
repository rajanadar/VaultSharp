using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.MongoDBAtlas
{
    /// <summary>
    /// Represents the MongoDBAtlas credentials.
    /// </summary>
    public class MongoDBAtlasCredentials
    {
        /// <summary>
        /// Gets or sets the lease duration seconds.
        /// </summary>
        [JsonPropertyName("lease_duration")]
        public string LeaseDuration { get; set; }

        /// <summary>
        /// Gets or sets the lease renewable flag.
        /// </summary>
        [JsonPropertyName("lease_renewable")]
        public string LeaseRenewable { get; set; }

        /// <summary>
        /// Gets or sets the description of the creds.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        [JsonPropertyName("private_key")]
        public string PrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        [JsonPropertyName("public_key")]
        public string PublicKey { get; set; }
    }
}