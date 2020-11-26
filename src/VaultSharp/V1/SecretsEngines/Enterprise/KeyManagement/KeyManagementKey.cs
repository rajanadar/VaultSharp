using System.Collections.Generic;
using Newtonsoft.Json;

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
        [JsonProperty("deletion_allowed")]
        public bool DeletionAllowed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("keys")]
        public Dictionary<string, Dictionary<string, object>> Keys;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("latest_version")]
        public int LatestVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("min_enabled_version")]
        public int MinimumEnabledVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}