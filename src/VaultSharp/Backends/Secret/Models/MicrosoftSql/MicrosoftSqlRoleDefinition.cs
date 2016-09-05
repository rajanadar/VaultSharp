using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.MicrosoftSql
{
    /// <summary>
    /// Represents the Microsoft Sql role definition
    /// </summary>
    public class MicrosoftSqlRoleDefinition
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the SQL statements executed to create and configure the role. 
        /// Must be semi-colon separated string, a base64-encoded semicolon-separated string, 
        /// a serialized JSON string array, or a base64-encoded serialized JSON string array.
        /// The '{{name}}' and '{{password}}' values will be substituted.
        /// </summary>
        /// <value>
        /// The SQL.
        /// </value>
        [JsonProperty("sql")]
        public string Sql { get; set; }
    }
}