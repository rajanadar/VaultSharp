using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Database
{
    /// <summary>
    /// Static role definition.
    /// </summary>
    public class StaticRole
    {
        /// <summary>
        /// Specifies the database username that this Vault role corresponds to.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// The name of the database connection to use for this role.
        /// </summary>
        [JsonPropertyName("db_name")]
        public DatabaseProviderType DatabaseProviderType { get; set; }

        /// <summary>
        /// Specifies the database statements to be executed to rotate the password for the configured database user. 
        /// Not every plugin type will support this functionality. 
        /// See the plugin's API page for more information on support and formatting for this parameter.
        /// </summary>
        [JsonPropertyName("rotation_statements")]
        public List<string> RotationStatements { get; set; }

        /// <summary>
        /// Specifies the amount of time Vault should wait before rotating the password. 
        /// The minimum is 5 seconds.
        /// </summary>
        [JsonPropertyName("rotation_period")]
        public string RotationPeriod { get; set; }
    }
}