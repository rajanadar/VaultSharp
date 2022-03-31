using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Data to be sent on restoring an encryption key from backup.
    /// </summary>
    public class RestoreKeyRequestOptions
    {
        /// <summary>
        /// The backup data for the key.
        /// </summary>
        [JsonProperty("backup")]
        public string BackupData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the restore operation should proceed and overwrite if a key by the provided name already exists.
        /// </summary>
        [JsonProperty("force")]
        public bool Force { get; set; }
    }
}