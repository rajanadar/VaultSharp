using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PostgreSql
{
    /// <summary>
    /// Represents the PostgreSql role definition
    /// </summary>
    public class PostgreSqlRoleDefinition
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the SQL statements executed to create and configure the role. 
        /// Must be semi-colon separated string, a base64-encoded semicolon-separated string, 
        /// a serialized JSON string array, or a base64-encoded serialized JSON string array.
        /// The '{{name}}', '{{password}}' and '{{expiration}}' values will be substituted.
        /// </summary>
        /// <value>
        /// The SQL.
        /// </value>
        [JsonProperty("sql")]
        public string Sql { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the SQL statements to be executed to revoke a user. 
        /// Must be a semicolon-separated string, a base64-encoded semicolon-separated string, 
        /// a serialized JSON string array, or a base64-encoded serialized JSON string array. 
        /// The '{{name}}' value will be substituted.
        /// </summary>
        /// <value>
        /// The Revocation SQL.
        /// </value>
        [JsonProperty("revocation_sql")]
        public string RevocationSql { get; set; }
    }
}