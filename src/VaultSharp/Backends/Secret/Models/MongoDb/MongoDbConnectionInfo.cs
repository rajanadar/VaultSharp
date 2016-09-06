using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.MongoDb
{
    /// <summary>
    /// Represents the MongoDb connection information.
    /// </summary>
    public class MongoDbConnectionInfo
    {
        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the standard connection string (URI) used to communicate with MongoDB.
        /// </summary>
        /// <value>
        /// The Connection String Uri
        /// </value>
        [JsonProperty("uri")]
        public string ConnectionStringUri { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a flag which when set verifies the <see cref="ConnectionStringUri"/> by 
        /// actually connecting to the database. 
        /// Defaults to true.
        /// </summary>
        /// <value>
        /// A flag to verify the connection.
        /// </value>
        [JsonProperty("verify_connection")]
        public bool VerifyConnection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbConnectionInfo" /> class.
        /// </summary>
        public MongoDbConnectionInfo()
        {
            VerifyConnection = true;
        }
    }
}