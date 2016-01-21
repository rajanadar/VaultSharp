using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.PostgreSql
{
    /// <summary>
    /// Represents the PostgreSql connection information.
    /// </summary>
    public class PostgreSqlConnectionInfo
    {
        /// <summary>
        /// Gets or sets the PostgreSQL connection URL or PG style string. e.g. "user=foo host=bar"
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        [JsonProperty("value")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the maximum number of open connections to the database. 
        /// Defaults to 2.
        /// </summary>
        /// <value>
        /// The maximum open connections.
        /// </value>
        [JsonProperty("max_open_connections")]
        public int MaximumOpenConnections { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlConnectionInfo"/> class.
        /// </summary>
        public PostgreSqlConnectionInfo()
        {
            MaximumOpenConnections = 2;
        }
    }
}