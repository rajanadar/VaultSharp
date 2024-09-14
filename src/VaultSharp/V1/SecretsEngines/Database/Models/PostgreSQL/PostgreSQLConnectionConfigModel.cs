using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Database.Models.PostgreSQL
{
    /// <summary>
    /// PostgreSQL specific Connection Config Model.
    /// </summary>
    public class PostgreSQLConnectionConfigModel : ConnectionConfigModel
    {
        [JsonIgnore]
        public DatabaseProviderType DatabaseProviderType { get; } = DatabaseProviderType.PostgreSQL;
        
        /// <summary>
        /// Specifies the maximum number of open connections to the database.
        /// </summary>
        [JsonPropertyName("max_open_connections")]
        public int MaximumOpenConnections { get; set; } = 4;
        
        /// <summary>
        /// Specifies the maximum number of idle connections to the database.
        /// A zero uses the value of <see cref="MaximumOpenConnections"/> and a negative value
        /// disables idle connections.
        /// If larger than <see cref="MaximumOpenConnections"/> it will be reduced to be equal.
        /// </summary>
        [JsonPropertyName("max_idle_connections")]
        public int MaximumIdleConnections { get; set; }
        
        /// <summary>
        /// Specifies the maximum amount of time a connection may be reused.
        /// If <= 0s, connections are reused forever.
        /// </summary>
        [JsonPropertyName("max_connection_lifetime")]
        public string MaximumConnectionLifetime { get; set; }
        
        /// <summary>
        /// If set to gcp_iam, will enable IAM authentication to a Google CloudSQL instance.
        /// For more information on authenticating to CloudSQL via IAM,
        /// please refer to Google's official documentation here
        /// https://cloud.google.com/sql/docs/postgres/iam-authentication
        /// </summary>
        [JsonPropertyName("auth_type")]
        public string AuthType { get; set; }
        
        /// <summary>
        /// JSON encoded credentials for a GCP Service Account to use for IAM authentication.
        /// Requires <see cref="AuthType" /> to be gcp_iam.
        /// </summary>
        [JsonPropertyName("service_account_json")]
        public string ServiceAccountJson { get; set; }
        
        /// <summary>
        /// Template describing how dynamic usernames are generated.
        /// https://developer.hashicorp.com/vault/docs/concepts/username-templating
        /// </summary>
        [JsonPropertyName("username_template")]
        public string UsernameTemplate { get; set; }

        /// <summary>
        /// When set to "scram-sha-256", passwords will be hashed by Vault and stored as-is by PostgreSQL.
        /// Using "scram-sha-256" requires a minimum version of PostgreSQL 10.
        /// Available options are "scram-sha-256" and "password".
        /// The default is "password".
        /// When set to "password", passwords will be sent to PostgreSQL in plaintext format
        /// and may appear in PostgreSQL logs as-is.
        /// For more information, please refer to the PostgreSQL documentation.
        /// https://www.postgresql.org/docs/current/sql-createrole.html#password
        /// </summary>
        [JsonPropertyName("password_authentication")]
        public string PasswordAuthentication { get; set; } = "password";

        public PostgreSQLConnectionConfigModel()
        {
            this.PluginName = "postgresql-database-plugin";
        }
    }
}