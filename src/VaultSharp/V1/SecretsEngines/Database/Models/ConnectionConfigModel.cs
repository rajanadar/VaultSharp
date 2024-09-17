using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Database.Models
{
    public class ConnectionConfigModel
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
        /// Specifies if the connection is verified during initial configuration.
        /// Defaults to true.
        /// </summary>
        [JsonPropertyName("verify_connection")]
        public bool VerifyConnection { get; set; } = true;
        
        /// <summary>
        /// List of the roles allowed to use this connection.
        /// Defaults to empty (no roles), if contains a * any role can use this connection.
        /// </summary>
        [JsonPropertyName("allowed_roles")]
        public List<string> AllowedRoles { get; set; }
        
        /// <summary>
        /// Specifies the database statements to be executed to rotate the root user's credentials.
        /// See the plugin's API page for more information on support and formatting for this parameter.
        /// </summary>
        [JsonPropertyName("root_rotation_statements")]
        public List<string> RootRotationStatements { get; set; }
        
        /// <summary>
        /// The name of the password policy to use when generating passwords for this database.
        /// If not specified, this will use a default policy defined as:
        /// 20 characters with at least 1 uppercase, 1 lowercase, 1 number, and 1 dash character.
        /// </summary>
        [JsonPropertyName("password_policy")]
        public string PasswordPolicyName { get; set; }
        
        /// <summary>
        /// Specifies the connection string used to connect to the database.
        /// Some plugins use url rather than connection_url.
        /// This allows for simple templating of the username and password of the root user.
        /// Typically, this is done by including a {{username}}, {{name}}, and/or {{password}}
        /// field within the string.
        /// These fields are typically be replaced with the values in the username and password fields.
        /// </summary>
        [JsonPropertyName("connection_url")]
        public string ConnectionUrl { get; set; }
        
        /// <summary>
        /// Specifies the name of the user to use as the "root" user when connecting to the database.
        /// This "root" user is used to create/update/delete users managed by these plugins,
        /// so you will need to ensure that this user has permissions to manipulate users appropriate
        /// to the database.
        /// This is typically used in the connection_url field via the templating
        /// directive {{username}} or {{name}}.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }
        
        /// <summary>
        /// Specifies the password to use when connecting with the username.
        /// This value will not be returned by Vault when performing a
        /// read upon the configuration.
        /// This is typically used in the connection_url field via the templating directive {{password}}.
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; }

        /// <summary>
        /// Determines whether special characters in the username and password fields will be escaped.
        /// Useful for alternate connection string formats like ADO.
        /// More information regarding this parameter can be found on the databases
        /// secrets engine docs.
        /// Defaults to false.
        /// </summary>
        [JsonPropertyName("disable_escaping")]
        public bool DisableEscapingSpecialCharactersInUsernameAndPassword { get; set; } = false;
    }
}