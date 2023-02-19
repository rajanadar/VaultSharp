using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend.Enterprise
{
    /// <summary>
    /// The requesting entity.
    /// </summary>
    public class ControlGroupRequestEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}