using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.MySql
{
    /// <summary>
    /// Represents the MySql role definition
    /// </summary>
    public class MySqlRoleDefinition
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the SQL statements executed to create and configure the role. 
        /// Must be semi-colon separated. 
        /// The '{{name}}' and '{{password}}' values will be substituted.
        /// </summary>
        /// <value>
        /// The SQL.
        /// </value>
        [JsonProperty("sql")]
        public string Sql { get; set; }
    }
}