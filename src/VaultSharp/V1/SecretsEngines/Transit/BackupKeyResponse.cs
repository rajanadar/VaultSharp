using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Data returned from an encryption key backup.
    /// </summary>
    public class BackupKeyResponse
    {
        /// <summary>
        /// The backup data for the key.
        /// </summary>
        [JsonProperty("backup")]
        public string BackupData { get; set; }
    }
}