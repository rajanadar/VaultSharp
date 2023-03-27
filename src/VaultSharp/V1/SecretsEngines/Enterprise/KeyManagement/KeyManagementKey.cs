using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Enterprise.KeyManagement
{
    /// <summary>
    /// The KeyMgmt key
    /// </summary>
    public class KeyManagementKey
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("deletion_allowed")]
        public bool DeletionAllowed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("keys")]
        public Dictionary<string, Dictionary<string, object>> Keys;

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("latest_version")]
        public int LatestVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("min_enabled_version")]
        public int MinimumEnabledVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}