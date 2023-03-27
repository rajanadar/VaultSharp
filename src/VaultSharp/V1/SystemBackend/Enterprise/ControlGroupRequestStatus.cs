using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend.Enterprise
{
    /// <summary>
    /// Control group status
    /// </summary>
    public class ControlGroupRequestStatus
    {
        /// <summary>
        /// Gets or sets the approval status.
        /// </summary>
        [JsonPropertyName("approved")]
        public bool Approved { get; set; }

        /// <summary>
        /// Gets or sets the request path.
        /// </summary>
        [JsonPropertyName("request_path")]
        public string RequestPath { get; set; }

        /// <summary>
        /// Gets or sets the requesting entity.
        /// </summary>
        [JsonPropertyName("request_entity")]
        public ControlGroupRequestEntity RequestEntity { get; set; }

        /// <summary>
        /// Gets or sets the authorizations.
        /// </summary>
        [JsonPropertyName("authorizations")]
        public IEnumerable<ControlGroupRequestAuthorization> Authorizations { get; set; }
    }
}