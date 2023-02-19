using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.GoogleCloud
{
    /// <summary>
    /// Represents the GoogleCloud ServiceAccountKey.
    /// </summary>
    public class GoogleCloudServiceAccountKey
    {
        /// <summary>
        /// Gets or sets the private_key_data.
        /// </summary>
        /// <value>
        /// The private_key_data.
        /// </value>
        [JsonPropertyName("private_key_data")]
        public string Base64EncodedPrivateKeyData { get; set; }

        /// <summary>
        /// Gets or sets the Key algorithm used to generate key. 
        /// </summary>
        /// <value>
        /// The key_algorithm.
        /// </value>
        [JsonPropertyName("key_algorithm")]
        public ServiceAccountKeyAlgorithm KeyAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the key_type.
        /// </summary>
        /// <value>
        /// The key_type.
        /// </value>
        [JsonPropertyName("key_type")]
        public ServiceAccountPrivateKeyType PrivateKeyType { get; set; }
    }
}