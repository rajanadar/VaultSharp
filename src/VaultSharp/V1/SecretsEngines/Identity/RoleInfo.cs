﻿using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    public class RoleInfo
    {
        /// <summary>
        /// A configured named key, the key must already exist.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// The template string to use for generating tokens. 
        /// This may be in string-ified JSON or base64 format.
        /// </summary>
        [JsonProperty("template")]
        public string Template { get; set; }

        /// <summary>
        /// Optional client ID. A random ID will be generated if left unset.
        /// </summary>
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        ///  TTL of the tokens generated against the role. Uses duration format
        ///  strings.
        /// </summary>
        [JsonProperty("ttl")]
        public string TimeToLive { get; set; }
    }
}
