using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.Core
{
    /// <summary>
    /// Represents the configuration values for a backend.
    /// </summary>
    public class NewBackendConfig : BackendConfig
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("audit_non_hmac_request_keys")]
        public List<string> AuditNonHMACRequestKeys { get; set; }

        [JsonProperty("audit_non_hmac_response_keys")]
        public List<string> AuditNonHMACResponseKeys { get; set; }

        [JsonProperty("listing_visibility")]
        public BackendListingVisibility ListingVisibility { get; set; } = BackendListingVisibility.hidden;

        [JsonProperty("passthrough_request_headers")]
        public List<string> PassthroughRequestHeaders { get; set; }

        [JsonProperty("allowed_response_headers")]
        public List<string> AllowedResponseHeaders { get; set; }

        [JsonProperty("allowed_managed_keys")]
        public List<string> AllowedManagedKeys { get; set; }

        [JsonProperty("plugin_version", NullValueHandling = NullValueHandling.Ignore)]
        public string PluginVersion { get; set; }
    }
}