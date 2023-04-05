using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    /// <summary>
    /// Request object to create a named key which is used to sign tokens.
    /// </summary>
    public class CreateNamedKeyRequest : NamedKeyInfo
    {
        /// <summary>
        /// Array of role client ids allowed to use this key for signing. If 
        /// empty, no roles are allowed. If "*", all roles are allowed.
        /// </summary>
        [JsonPropertyName("allowed_client_ids")]
        public List<string> AllowedClientIDs { get; set; }
    }
}
