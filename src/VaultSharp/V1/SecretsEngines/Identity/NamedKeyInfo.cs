using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    public class NamedKeyInfo
    {
        /// <summary>
        /// How often to generate a new signing key. Uses duration format strings.
        /// </summary>
        /// <example>"24h"</example>
        [JsonPropertyName("rotation_period")]
        public string RotationPeriod { get; set; }

        /// <summary>
        /// Controls how long the public portion of a signing key will be 
        /// available for verification after being rotated. Uses duration format strings.
        /// </summary>
        /// <example>"24h"</example>
        [JsonPropertyName("verification_ttl")]
        public string VerificationTimeToLive { get; set; }

        /// <summary>
        /// Signing algorithm to use. Allowed values are: RS256 (default), 
        /// RS384, RS512, ES256, ES384, ES512, EdDSA.
        /// </summary>
        [JsonPropertyName("algorithm")]
        public string Algorithm { get; set; }
    }
}
