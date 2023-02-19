using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.KeyValue.V2
{
    /// <summary>
    /// Request Object to create/update/patch a metadata of secret.
    /// </summary>
    public class CustomMetadataRequest
    {
        /// <summary>
        /// The number of versions to keep per key. If not set, the backend’s configured max version is used.
        /// Once a key has more than the configured allowed versions, the oldest version will be permanently deleted.
        /// </summary>
        [JsonPropertyName("max_versions")]
        public int? MaxVersion { get; set; }

        /// <summary>
        /// If true, the key will require the cas parameter to be set on all write requests.
        /// If false, the backend’s configuration will be used.
        /// </summary>
        [JsonPropertyName("cas_required")]
        public bool? CASRequired { get; set; }

        /// <summary>
        /// Set the delete_version_after value to a duration to specify the deletion_time for all new versions written to this key.
        /// If not set, the backend's delete_version_after will be used.
        /// If the value is greater than the backend's delete_version_after, the backend's delete_version_after will be used.
        /// Accepts [Go duration format string][duration-godoc].
        /// </summary>
        /// <example>"0s"</example>
        /// <example>"3h25m19s"</example>
        [JsonPropertyName("delete_version_after")]
        public string DeleteVersionAfter { get; set; }

        /// <summary>
        /// An object that holds option settings.
        /// <para>
        /// cas - This flag is required if cas_required is set to true on
        /// either the secret or the engine's config. In order for a write
        /// to be successful, cas must be set to the current version of
        /// the secret. A patch operation must be attempted on an existing
        /// key, thus the provided cas value must be greater than 0.
        /// </para>
        /// </summary>
        [JsonPropertyName("options")]
        public Dictionary<string, object> Options { get; set; }

        /// <summary>
        /// A map of arbitrary string to string valued user-provided metadata meant to describe the secret.
        /// </summary>
        [JsonPropertyName("custom_metadata")]
        public Dictionary<string, string> CustomMetadata { get; set; }
    }
}
