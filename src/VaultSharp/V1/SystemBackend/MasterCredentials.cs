using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Represents the root credentials (master keys and root token) for Vault.
    /// </summary>
    public class MasterCredentials
    {
        /// <summary>
        /// Gets or sets the master keys (possibly encrypted with PGP Keys, if provided during Vault initialization)
        /// </summary>
        /// <value>
        /// The master keys.
        /// </value>
        [JsonPropertyName("keys")]
        public string[] MasterKeys { get; set; }

        /// <summary>
        /// Gets or sets the new base 64 master keys. (possibly pgp encrypted)
        /// </summary>
        /// <value>
        /// The master keys.
        /// </value>
        [JsonPropertyName("keys_base64")]
        public string[] Base64MasterKeys { get; set; }

        /// <summary>
        /// Gets or sets the root token which has superhero permissions on Vault.
        /// </summary>
        /// <value>
        /// The root token.
        /// </value>
        [JsonPropertyName("root_token")]
        public string RootToken { get; set; }
    }
}