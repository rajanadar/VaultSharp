using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.TOTP
{
    /// <summary>
    /// Represents a queried key
    /// </summary>
    public class TOTPKey
    {
        /// <summary>
        /// Gets or sets the name of the account associated with the key.
        /// </summary>
        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets the used hashing algorithm.
        /// </summary>
        [JsonProperty("algorithm")]
        public string Algorithm { get; set; }

        /// <summary>
        /// Gets or sets the name of the issuing organization.
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the length of time in seconds used to
        /// create a counter for the TOTP code calculation.
        /// </summary>
        [JsonProperty("period")]
        public string Period { get; set; }
    }
}
