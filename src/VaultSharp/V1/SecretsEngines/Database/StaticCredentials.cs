using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Database
{
    /// <summary>
    /// Static credential definition.
    /// </summary>
    public class StaticCredentials : UsernamePasswordCredentials
    {
        /// <summary>
        /// Specifies the last time Vault rotated this cred.
        /// </summary>
        [JsonProperty("last_vault_rotation")]
        public string LastVaultRotation { get; set; }

        /// <summary>
        /// Specifies the rotation period for this cred.
        /// </summary>
        [JsonProperty("rotation_period")]
        public string RotationPeriod { get; set; }

        /// <summary>
        /// Specifies the remaining time for the creds.
        /// </summary>
        [JsonProperty("ttl")]
        public string TimeToLive { get; set; }
    }
}