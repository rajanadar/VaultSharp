using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.V1.Commons
{
    /// <summary>
    /// Represents backend.
    /// </summary>
    public abstract class AbstractBackend
    {
        /// <summary>
        /// Gets or sets the path. If not set, the value will default to the type value.
        /// Presence or absence of leading or trailing slashes don't matter.
        /// </summary>
        /// <value>
        /// The mount point.
        /// </value>
        [JsonIgnore]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the accessor.
        /// </summary>
        /// <value>
        /// The accessor.
        /// </value>
        [JsonProperty("accessor")]
        public string Accessor { get; set; }

        // raja todo: see if these Config options are strong typable or predicatble.
        // "config":{"default_lease_ttl":0,"force_no_cache":false,"max_lease_ttl":0,"token_type":"default-service"}
        // BackendConfig + some specific things like token_type

        /// <summary>
        /// Gets or sets the config options.
        /// </summary>
        /// <value>
        /// The config options.
        /// </value>
        [JsonProperty("config")]
        public Dictionary<string, object> Config { get; set; }

        /// <summary>
        /// Gets or sets a human-friendly description of the backend.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a flag indicating if this is a local mount.
        /// </summary>
        /// <remarks>
        /// The option is allowed in Vault open-source, but relevant functionality is only supported in Vault Enterprise:
        /// </remarks>
        /// <value>
        /// The flag.
        /// </value>
        [JsonProperty("local")]
        public bool Local { get; set; }

        /// <summary>
        /// Gets or sets a seal wrap flag for the backend.
        /// </summary>
        /// <value>
        /// The flag.
        /// </value>
        [JsonProperty("seal_wrap")]
        public bool SealWrap { get; set; }

        /// <summary>
        /// Gets or sets the plugin name.
        /// </summary>
        /// <value>
        /// The plugin name.
        /// </value>
        [JsonProperty("plugin_name")]
        public string PluginName { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        [JsonProperty("options")]
        public Dictionary<string, object> Options { get; set; }
    }
}