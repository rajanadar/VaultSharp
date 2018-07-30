namespace VaultSharp.V1.SecretEngines
{
    public class SecretsEngineDefaultPaths
    {
        public const string System = "system";

        public const string AWS = "aws";
        public const string Consul = "consul";
        public const string Cubbyhole = "cubbyhole";
        public const string Databases = "database";
        public const string KeyValue = "secret"; // this is the generic backend.
        public const string Identity = "identity";
        public const string Nomad = "nomad";
        public const string PKI = "pki";
        public const string RabbitMQ = "rabbitmq";
        public const string SSH = "ssh";
        public const string TOTP = "totp";
        public const string Transit = "transit";

        public const string Cassandra = "cassandra";
        public const string HanaDB = "hana";
        public const string MongoDB = "mongodb";
        public const string MSSQL = "mssql";
        public const string MySqlMariaDB = "mysql";
        public const string PostgreSQL = "postgresql";
        public const string Oracle = "oracle";
    }
}