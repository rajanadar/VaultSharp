using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.MicrosoftSql
{
    /// <summary>
    /// Represents the Microsoft Sql connection information.
    /// </summary>
    public class MicrosoftSqlConnectionInfo
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the connection DSN used to communicate with Sql Server.
        /// </summary>
        /// <value>
        /// The MSSQL DSN
        /// </value>
        [JsonProperty("connection_string ")]
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
        /// <para>[optional]</para>
        /// Gets or sets a flag which when set verifies the <see cref="ConnectionString"/> by 
        /// actually connecting to the database. 
        /// Defaults to true.
        /// </summary>
        /// <value>
        /// A flag to verify the connection.
        /// </value>
        [JsonProperty("verify_connection")]
        public bool VerifyConnection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftSqlConnectionInfo" /> class.
        /// </summary>
        public MicrosoftSqlConnectionInfo()
        {
            MaximumOpenConnections = 2;
            VerifyConnection = true;
        }
    }
}