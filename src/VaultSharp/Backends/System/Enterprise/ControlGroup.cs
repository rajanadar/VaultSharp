using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Enterprise
{
    public class ControlGroup
    {
        /// <summary>
        /// Gets or sets the maximum ttl for a control group wrapping token. 
        /// This can be in seconds or duration 
        /// </summary>
        /// <value>
        /// The max ttl.
        /// </value>
        [JsonProperty("max_ttl")]
        public string MaxTimeToLive { get; set; }
    }
}