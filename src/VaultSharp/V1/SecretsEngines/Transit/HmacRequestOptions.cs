﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the request to calculate the HMAC for a given data string.
    /// <seealso cref="HmacSingleInput" />
    public class HmacRequestOptions : HmacSingleInput
    { 
        /// <summary>
        /// Gets or sets a list of items for processing.  
        /// If set, then the <see cref="HmacSingleInput.Base64EncodedInput">input</see> parameter will be ignored.
        /// </summary>
        /// <value>The list of items for processing.</value>
        [JsonPropertyName("batch_input")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<HmacSingleInput> BatchInput { get; set; }
    }

    /// <summary>
    /// Represents a single entry to be sent for calculating the HMAC
    /// </summary>
    public class HmacSingleInput
    {
        [JsonIgnore]
        public TransitHashAlgorithm Algorithm { get; set; } = TransitHashAlgorithm.SHA2_256;

        /// <summary>
        /// Gets or sets the version of the key to use for the operation. Should only be set if an explicit version is required.
        /// </summary>
        /// <value>The key version to use for the operation.</value>
        [JsonPropertyName("key_version")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? KeyVersion { get; set; }

        /// <summary>
        /// Gets or sets the base64 encoded input data to generate an HMAC for.
        /// </summary>
        /// <value>The base64 encoded input to generate an HMAC for.</value>
        [JsonPropertyName("input")]
        public string Base64EncodedInput { get; set; }
    }
}