﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend.Enterprise
{
    /// <summary>
    /// Represents a Vault EGP Policy entity.
    /// </summary>
    public class EGPPolicy : AbstractGPPolicyBase
    {
        /// <summary>
        /// Gets or sets the paths.
        /// </summary>
        /// <value>
        /// The paths.
        /// </value>
        [JsonPropertyName("paths")]
        public IEnumerable<string> Paths { get; set; }
    }
}