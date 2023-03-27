using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.Commons
{
    /// <summary>
    /// Represents the authorization information in Vault.
    /// </summary>
    public class AuthInfo
    {
        /// <summary>
        /// Gets or sets the client token accessor.
        /// </summary>
        /// <value>
        /// The client token accessor.
        /// </value>
        [JsonPropertyName("accessor")]
        public string ClientTokenAccessor { get; set; }

        /// <summary>
        /// Gets or sets the client token.
        /// </summary>
        /// <value>
        /// The client token.
        /// </value>
        [JsonPropertyName("client_token")]
        public string ClientToken { get; set; }

        /// <summary>
        /// Gets or sets the policies.
        /// </summary>
        /// <value>
        /// The policies.
        /// </value>
        [JsonPropertyName("policies")]
        public List<string> Policies { get; set; }

        /// <summary>
        /// Gets or sets any metadata associated with this authorization info.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        [JsonPropertyName("metadata")]
        public Dictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Gets or sets the lease duration seconds.
        /// </summary>
        /// <value>
        /// The lease duration seconds.
        /// </value>
        [JsonPropertyName("lease_duration")]
        public int LeaseDurationSeconds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AuthInfo"/> is renewable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if renewable; otherwise, <c>false</c>.
        /// </value>
        [JsonPropertyName("renewable")]
        public bool Renewable { get; set; }

        // raja todo: Add more stuff to this.
        /*
         * https://www.vaultproject.io/api-docs/auth/token
         *   "auth": {
                        "client_token": "s.wOrq9dO9kzOcuvB06CMviJhZ",
                        "accessor": "B6oixijqmeR4bsLOJH88Ska9",
                        "policies": ["default", "stage", "web"],
                        "token_policies": ["default", "stage", "web"],
                        "metadata": {
                          "user": "armon"
                        },
                        "lease_duration": 3600,
                        "renewable": true,
                        "entity_id": "",
                        "token_type": "service",
                        "orphan": false
                      }
         * 
         */

    }
}