using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.MongoDb
{
    /// <summary>
    /// Represents MongoDb credentials with a username and password.
    /// </summary>
    public class MongoDbUsernamePasswordCredentials : UsernamePasswordCredentials
    {
        /// <summary>
        /// Gets or sets name of the database the credentials apply to.
        /// </summary>
        /// <value>
        /// The database name.
        /// </value>
        [JsonProperty("db")]
        public string Database { get; set; }
    }
}