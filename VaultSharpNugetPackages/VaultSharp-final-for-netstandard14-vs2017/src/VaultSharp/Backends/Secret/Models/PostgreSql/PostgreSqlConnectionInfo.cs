using System;
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
        [JsonProperty("connection_url")]
        public string ConnectionUrl { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the PostgreSQL connection URL or PG style string. e.g. "user=foo host=bar"
        /// Deprecated, please use <see cref="ConnectionUrl"/>
        /// </summary>
        /// <value>
        /// The value of the data source.
        /// </value>
        [JsonProperty("value")]
        [Obsolete("Please use ConnectionUrl.")]
        public string Value { get; set; }

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
        /// <para>[optional]</para>
        /// Gets or sets the Maximum number of idle connections to the database. 
        /// A zero uses the value of <see cref="MaximumOpenConnections"/> and a negative value disables idle connections. 
        /// If larger than <see cref="MaximumOpenConnections"/> it will be reduced to be equal.
        /// </summary>
        /// <value>
        /// The maximum idle connections.
        /// </value>
        [JsonProperty("max_idle_connections")]
        public int MaximumIdleConnections { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a flag which when set verifies the <see cref="ConnectionUrl"/> by 
        /// actually connecting to the database. 
        /// Defaults to true.
        /// </summary>
        /// <value>
        /// A flag to verify the connection.
        /// </value>
        [JsonProperty("verify_connection")]
        public bool VerifyConnection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlConnectionInfo"/> class.
        /// </summary>
        public PostgreSqlConnectionInfo()
        {
            MaximumOpenConnections = 2;
            VerifyConnection = true;
        }
    }
}