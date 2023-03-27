using System.Text.Json.Serialization;

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
        [JsonPropertyName("backup")]
        public string BackupData { get; set; }
    }
}