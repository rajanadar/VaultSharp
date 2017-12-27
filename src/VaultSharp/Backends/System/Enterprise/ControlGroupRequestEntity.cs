using Newtonsoft.Json;

namespace VaultSharp.Backends.System.Enterprise
{
    /// <summary>
    /// The requesting entity.
    /// </summary>
    public class ControlGroupRequestEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}