using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    /// <summary>
    /// Request object to create a named key which is used to sign tokens.
    /// </summary>
    public class CreateNamedKeyRequest
    {
        /// <summary>
        /// How often to generate a new signing key. Uses duration format strings.
        /// </summary>
        /// <example>"24h"</example>
        [JsonProperty("rotation_period")]
        public string RotationPeriod { get; set; }

        /// <summary>
        /// Controls how long the public portion of a signing key will be 
        /// available for verification after being rotated. Uses duration format strings.
        /// </summary>
        /// <example>"24h"</example>
        [JsonProperty("verification_ttl")]
        public string VerificationTimeToLive { get; set; }

        /// <summary>
        /// Array of role client ids allowed to use this key for signing. If 
        /// empty, no roles are allowed. If "*", all roles are allowed.
        /// </summary>
        [JsonProperty("allowed_client_ids")]
        public List<string> AllowedClientIDs { get; set; }

        /// <summary>
        /// Signing algorithm to use. Allowed values are: RS256 (default), 
        /// RS384, RS512, ES256, ES384, ES512, EdDSA.
        /// </summary>
        [JsonProperty("algorithm")]
        public string Algorithm { get; set; }
    }
}
