using Newtonsoft.Json;

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
        [JsonProperty("lease_duration")]
        public string LeaseDuration { get; set; }

        /// <summary>
        /// Gets or sets the lease renewable flag.
        /// </summary>
        [JsonProperty("lease_renewable")]
        public string LeaseRenewable { get; set; }

        /// <summary>
        /// Gets or sets the description of the creds.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the private key.
        /// </summary>
        [JsonProperty("private_key")]
        public string PrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }
    }
}