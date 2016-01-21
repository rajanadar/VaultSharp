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
        /// Must be semi-colon separated. 
        /// The '{{name}}', '{{password}}' and '{{expiration}}' values will be substituted.
        /// </summary>
        /// <value>
        /// The SQL.
        /// </value>
        [JsonProperty("sql")]
        public string Sql { get; set; }
    }
}