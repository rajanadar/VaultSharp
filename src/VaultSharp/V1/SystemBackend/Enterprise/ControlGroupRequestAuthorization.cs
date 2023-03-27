﻿using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend.Enterprise
{
    /// <summary>
    /// Authorization.
    /// </summary>
    public class ControlGroupRequestAuthorization
    {
        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        [JsonPropertyName("entity_id")]
        public string EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity name.
        /// </summary>
        [JsonPropertyName("entity_name")]
        public string EntityName { get; set; }
    }
}