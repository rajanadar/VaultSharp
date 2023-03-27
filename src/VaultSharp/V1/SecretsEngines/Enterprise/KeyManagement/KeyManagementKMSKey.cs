using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Enterprise.KeyManagement
{
    /// <summary>
    /// Key in KMS
    /// </summary>
    public class KeyManagementKMSKey
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("protection")]
        public string Protection { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("purpose")]
        public string Purpose { get; set; }
    }
}