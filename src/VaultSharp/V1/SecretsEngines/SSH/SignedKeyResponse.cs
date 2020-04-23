using Newtonsoft.Json;

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
        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// The signed key
        /// </summary>
        [JsonProperty("signed_key")]
        public string SignedKey { get; set; }
    }
}