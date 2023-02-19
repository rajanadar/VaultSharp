using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.Core
{
    /// <summary>
    /// Represents the configuration values for a backend.
    /// </summary>
    public class NewBackendConfig : BackendConfig
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("audit_non_hmac_request_keys")]
        public List<string> AuditNonHMACRequestKeys { get; set; }

        [JsonPropertyName("audit_non_hmac_response_keys")]
        public List<string> AuditNonHMACResponseKeys { get; set; }

        [JsonPropertyName("listing_visibility")]
        public BackendListingVisibility ListingVisibility { get; set; } = BackendListingVisibility.hidden;

        [JsonPropertyName("passthrough_request_headers")]
        public List<string> PassthroughRequestHeaders { get; set; }

        [JsonPropertyName("allowed_response_headers")]
        public List<string> AllowedResponseHeaders { get; set; }

        [JsonPropertyName("allowed_managed_keys")]
        public List<string> AllowedManagedKeys { get; set; }

        [JsonPropertyName("plugin_version", NullValueHandling = NullValueHandling.Ignore)]
        public string PluginVersion { get; set; }
    }
}