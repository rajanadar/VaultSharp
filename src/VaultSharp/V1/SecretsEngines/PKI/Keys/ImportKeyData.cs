using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.PKI.Keys
{
    public class ImportKeyData
    {
        /// <summary>
        /// Optional name to be used for this key
        /// </summary>
        /// <value>Optional name to be used for this key</value>
        [JsonPropertyName("key_name")]
        public string KeyName { get; set; }

        [JsonPropertyName("pem_bundle")]
        public string PemBundle { get; set; }
    }
}