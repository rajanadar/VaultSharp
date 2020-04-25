using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.OpenLDAP
{
    /// <summary>
    /// Static credential definition.
    /// </summary>
    public class StaticCredentials : UsernamePasswordCredentials
    {
        /// <summary>
        /// Specifies the Distinguished Name
        /// </summary>
        [JsonProperty("dn")]
        public string DistinguishedName { get; set; }

        /// <summary>
        /// Specifies the last vault rotation for this cred.
        /// </summary>
        [JsonProperty("last_vault_rotation")]
        public string LastVaultRotation { get; set; }

        /// <summary>
        /// Specifies the rotation period for this cred.
        /// </summary>
        [JsonProperty("rotation_period")]
        public int RotationPeriod { get; set; }

        /// <summary>
        /// Specifies the remaining time for the creds.
        /// </summary>
        [JsonProperty("ttl")]
        public int TimeToLive { get; set; }
    }
}