﻿using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Enterprise.Transform
{
    /// <summary>
    /// Represents a single Decoded item.
    /// </summary>
    public class DecodedItem
    {
        /// <summary>
        /// Specifies the decoded value.
        /// </summary>
        [JsonPropertyName("decoded_value")]
        public string DecodedValue { get; set; }

        /// <summary>
        /// Specifies the base64 encoded tweak that was provided during encoding.
        /// </summary>
        [JsonPropertyName("tweak")]
        public string Tweak { get; set; }
    }
}