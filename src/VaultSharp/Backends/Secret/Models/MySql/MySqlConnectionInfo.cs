using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.MySql
{
    /// <summary>
    /// Represents the MySql connection information.
    /// </summary>
    public class MySqlConnectionInfo
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the name of the data source, the DSN.
        /// </summary>
        /// <value>
        /// The name of the data source.
        /// </value>
        [JsonProperty("value")]
        public string DataSourceName { get; set; }

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
        /// Initializes a new instance of the <see cref="MySqlConnectionInfo" /> class.
        /// </summary>
        public MySqlConnectionInfo()
        {
            MaximumOpenConnections = 2;
        }
    }
}