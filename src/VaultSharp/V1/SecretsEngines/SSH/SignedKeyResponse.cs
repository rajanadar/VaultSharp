using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.SSH
{
    /// <summary>
    /// Response from signing a key.
    /// </summary>
    public class SignedKeyResponse
    {
        /// <summary>
        /// The serial number of the key.
        /// </summary>
        [JsonPropertyName("serial_number")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// The signed key
        /// </summary>
        [JsonPropertyName("signed_key")]
        public string SignedKey { get; set; }
    }
}