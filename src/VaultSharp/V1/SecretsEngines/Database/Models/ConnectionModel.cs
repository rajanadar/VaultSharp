using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Database.Models
{
    /// <summary>
    /// Connection Config Model.
    /// </summary>
    public class ConnectionModel
    {
        /// <summary>
        /// Specifies the name of the plugin to use for this connection.
        /// </summary>
        [JsonPropertyName("plugin_name")]
        public string PluginName { get; set; }
        
        /// <summary>
        /// Specifies the semantic version of the plugin to use for this connection.
        /// </summary>
        [JsonPropertyName("plugin_version")]
        public string PluginVersion { get; set; }
        
        /// <summary>
        /// List of the roles allowed to use this connection.
        /// Defaults to empty (no roles), if contains a * any role can use this connection.
        /// </summary>
        [JsonPropertyName("allowed_roles")]
        public List<string> AllowedRoles { get; set; }
        
        [JsonPropertyName("connection_details")]
        public ConnectionDetailsModel ConnectionDetails { get; set; }
        
        /// <summary>
        /// The name of the password policy to use when generating passwords for this database.
        /// If not specified, this will use a default policy defined as:
        /// 20 characters with at least 1 uppercase, 1 lowercase, 1 number, and 1 dash character.
        /// </summary>
        [JsonPropertyName("password_policy")]
        public string PasswordPolicyName { get; set; }
        
        [JsonPropertyName("root_credentials_rotate_statements")]
        public List<string> RootCredentialsRotateStatements { get; set; }
        
        public class ConnectionDetailsModel
        {
            [JsonPropertyName("connection_url")]
            public string ConnectionUrl { get; set; }
            
            [JsonPropertyName("username")]
            public string Username { get; set; }
        }
    }
}