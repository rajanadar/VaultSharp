using Newtonsoft.Json;

namespace VaultSharp.Backends.Secret.Models.Cassandra
{
    /// <summary>
    /// Represents the Cassandra connection information.
    /// </summary>
    public class CassandraConnectionInfo
    {
        private const int DefaultCqlProtocolVersion = 2;

        private const int DefaultConnectionTimeoutSeconds = 5;

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets a set of comma-delimited Cassandra hosts to connect to.
        /// </summary>
        /// <value>
        /// The hosts.
        /// </value>
        [JsonProperty("hosts")]
        public string Hosts { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the username to use for superuser access.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// <para>[required]</para>
        /// Gets or sets the password corresponding to the given username.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether to use TLS when connecting to Cassandra.
        ///  If set to true, the connection will use TLS; 
        /// this happens automatically if <see cref="PemBundle"/>, <see cref="PemJson"/>, or <see cref="InsecureTLS"/> is set.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use TLS]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("tls")]
        public bool UseTLS { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets a value indicating whether to skip verification of the server certificate when using TLS.
        /// The connection will not perform verification of the server certificate; 
        /// If this needs to be true, also set <see cref="UseTLS"/> to true.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use insecure TLS]; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty("insecure_tls")]
        public bool InsecureTLS { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the concatenated PEM blocks containing a certificate and private key; 
        /// a certificate, private key, and issuing CA certificate; or just a CA certificate.
        /// Should be a PEM-concatenated bundle of a private key + client certificate, an issuing CA certificate, or both. 
        /// <para>
        /// If the only certificate in <see cref="PemBundle"/> is a CA certificate, 
        /// the given CA certificate will be used for server certificate verification; otherwise the system CA certificates will be used.
        /// If certificate and private_key are set in <see cref="PemBundle"/>, client auth will be turned on for the connection.
        /// </para>
        /// </summary>
        /// <value>
        /// The pem bundle.
        /// </value>
        [JsonProperty("pem_bundle")]
        public string PemBundle { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the pem JSON containing a certificate and private key; 
        /// a certificate, private key, and issuing CA certificate; or just a CA certificate.
        /// Should be a PEM-concatenated bundle of a private key + client certificate, an issuing CA certificate, or both. 
        /// <para>
        /// If only issuing_ca is set in <see cref="PemJson"/> the given CA certificate will be used for server certificate verification; 
        /// otherwise the system CA certificates will be used.
        /// If certificate and private_key are set in <see cref="PemJson"/>, client auth will be turned on for the connection.
        /// </para>
        /// </summary>
        /// <value>
        /// The pem json.
        /// </value>
        [JsonProperty("pem_json")]
        public string PemJson { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets the CQL protocol version.
        /// Defaults to <value>true</value>.
        /// </summary>
        /// <value>
        /// The CQL protocol version.
        /// </value>
        [JsonProperty("protocol_version")]
        public int CqlProtocolVersion { get; set; }

        /// <summary>
        /// <para>[optional]</para>
        /// Gets or sets connection timeout to use. 
        /// Defaults to 5 seconds.
        /// </summary>
        /// <value>
        /// The connection timeout.
        /// </value>
        [JsonProperty("connect_timeout")]
        public int ConnectionTimeoutSeconds { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CassandraConnectionInfo"/> class.
        /// </summary>
        public CassandraConnectionInfo()
        {
            CqlProtocolVersion = DefaultCqlProtocolVersion;
            ConnectionTimeoutSeconds = DefaultConnectionTimeoutSeconds;
        }
    }
}