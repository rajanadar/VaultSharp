using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend.Plugin
{
    /// <summary>
    /// The plugin config.
    /// </summary>
    public class PluginConfig
    {
        /// <summary>
        /// Gets or sets the plugin arguments.
        /// </summary>
        [JsonPropertyName("args")]
        public string[] Args { get; set; }

        /// <summary>
        /// Gets or sets a flag denoting if this plugin is built in or not.
        /// </summary>
        [JsonPropertyName("builtin")]
        public bool Builtin { get; set; }

        /// <summary>
        /// Gets or sets the command used to execute the plugin. 
        /// This is relative to the plugin directory. e.g. "myplugin --my_flag=1"
        /// </summary>
        [JsonPropertyName("command")]
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the name for this plugin. 
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the SHA256 sum of the plugin's binary. 
        /// Before a plugin is run it's SHA will be checked against this value, if they do not match the plugin can not be run.
        /// </summary>
        [JsonPropertyName("sha256")]
        public string Sha256 { get; set; }
    }
}