using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Models
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
        [JsonProperty("keys")]
        public string[] MasterKeys { get; set; }

        /// <summary>
        /// Gets or sets the new base 64 master keys. (possibly pgp encrypted)
        /// </summary>
        /// <value>
        /// The master keys.
        /// </value>
        [JsonProperty("keys_base64")]
        public string[] Base64MasterKeys { get; set; }

        /// <summary>
        /// Gets or sets the root token which has superhero permissions on Vault.
        /// </summary>
        /// <value>
        /// The root token.
        /// </value>
        [JsonProperty("root_token")]
        public string RootToken { get; set; }
    }
}