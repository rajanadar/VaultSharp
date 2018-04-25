using Newtonsoft.Json;

namespace VaultSharp.SystemBackend.Plugin
{
    /// <summary>
    /// The plugin config.
    /// </summary>
    public class PluginConfig
    {
        /// <summary>
        /// Gets or sets the plugin arguments.
        /// </summary>
        [JsonProperty("args")]
        public string[] Args { get; set; }

        /// <summary>
        /// Gets or sets a flag denoting if this plugin is built in or not.
        /// </summary>
        [JsonProperty("builtin")]
        public bool Builtin { get; set; }

        /// <summary>
        /// Gets or sets the command used to execute the plugin. 
        /// This is relative to the plugin directory. e.g. "myplugin --my_flag=1"
        /// </summary>
        [JsonProperty("command")]
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the name for this plugin. 
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the SHA256 sum of the plugin's binary. 
        /// Before a plugin is run it's SHA will be checked against this value, if they do not match the plugin can not be run.
        /// </summary>
        [JsonProperty("sha256")]
        public string Sha256 { get; set; }
    }
}