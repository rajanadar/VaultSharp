using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.SystemBackend.Enterprise
{
    /// <summary>
    /// Control group status
    /// </summary>
    public class ControlGroupRequestStatus
    {
        /// <summary>
        /// Gets or sets the approval status.
        /// </summary>
        [JsonProperty("approved")]
        public bool Approved { get; set; }

        /// <summary>
        /// Gets or sets the request path.
        /// </summary>
        [JsonProperty("request_path")]
        public string RequestPath { get; set; }

        /// <summary>
        /// Gets or sets the requesting entity.
        /// </summary>
        [JsonProperty("request_entity")]
        public ControlGroupRequestEntity RequestEntity { get; set; }

        /// <summary>
        /// Gets or sets the authorizations.
        /// </summary>
        [JsonProperty("authorizations")]
        public IEnumerable<ControlGroupRequestAuthorization> Authorizations { get; set; }
    }
}