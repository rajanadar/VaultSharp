using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.KeyValue.V2
{
    /// <summary>
    /// Request Object to patch a secret.
    /// </summary>
    public class PatchSecretDataRequest
    {
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
        /// The contents of the data map will be applied as a partial update 
        /// to the existing entry via a JSON merge patch to the existing entry.
        /// </summary>
        [JsonPropertyName("data")]
        public Dictionary<string, object> Data { get; set; }
    }
}
