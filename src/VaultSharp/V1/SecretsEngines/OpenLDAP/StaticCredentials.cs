using System.Text.Json.Serialization;
using VaultSharp.Core;

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
        [JsonPropertyName("dn")]
        public string DistinguishedName { get; set; }

        /// <summary>
        /// Specifies the last vault rotation for this cred.
        /// </summary>
        [JsonPropertyName("last_vault_rotation")]
        public string LastVaultRotation { get; set; }

        /// <summary>
        /// Specifies the rotation period for this cred.
        /// </summary>
        [JsonPropertyName("rotation_period")]
        [JsonConverter(typeof(IntegerToStringJsonConverter))]
        public int RotationPeriod { get; set; }

        /// <summary>
        /// Specifies the remaining time for the creds.
        /// </summary>
        [JsonPropertyName("ttl")]
        public int TimeToLive { get; set; }
    }
}