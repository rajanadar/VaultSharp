﻿using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.Enterprise.Transform
{
    /// <summary>
    /// Represents a single Encoding item.
    /// </summary>
    public class EncodingItem
    {
        /// <summary>
        /// Specifies the value to be encoded.
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Specifies the transformation within the role that should be used for this encode operation. 
        /// If a single transformation exists for role, this parameter may be skipped and will be inferred. 
        /// If multiple transformations exist, one must be specified.
        /// </summary>
        [JsonProperty("transformation")]
        public string Transformation { get; set; }

        /// <summary>
        /// Specifies the base64 encoded tweak to use. 
        /// Only applicable for FPE transformations with supplied as the tweak source.
        /// </summary>
        [JsonProperty("tweak")]
        public string Tweak { get; set; }
    }
}