namespace VaultSharp.V1.SecretsEngines
{
    public class SecretsEngineMountPoints 
    {
        public class Defaults 
        {
            public const string System = "system";

            public const string ActiveDirectory = "ad";
            public const string AliCloud = "alicloud";
            public const string AWS = "aws";
            public const string Azure = "azure";
            public const string Consul = "consul";
            public const string Cubbyhole = "cubbyhole";
            public const string Database = "database";
            public const string GoogleCloud = "gcp";
            public const string GoogleCloudKMS = "gcpkms";
            public const string KeyManagement = "keymgmt";
            public const string KMIP = "kmip";
            public const string KeyValueV1 = "kv";
            public const string MongoDBAtlas = "mongodbatlas";
            public const string KeyValueV2 = "kv-v2";
            public const string Identity = "identity";
            public const string Nomad = "nomad";
            public const string OpenLDAP = "openldap";
            public const string PKI = "pki";
            public const string RabbitMQ = "rabbitmq";
            public const string SSH = "ssh";
            public const string TOTP = "totp";
            public const string Transform = "transform";
            public const string Transit = "transit";

            public const string Cassandra = "cassandra";
            public const string HanaDB = "hana";
            public const string MongoDB = "mongodb";
            public const string MSSQL = "mssql";
            public const string MySqlMariaDB = "mysql";
            public const string PostgreSQL = "postgresql";
            public const string Oracle = "oracle";
        }

        public string System { get; set; } = Defaults.System;

        public string ActiveDirectory { get; set; } = Defaults.ActiveDirectory;
        public string AliCloud { get; set; } = Defaults.AliCloud;
        public string AWS { get; set; } = Defaults.AWS;
        public string Azure { get; set; } = Defaults.Azure;
        public string Consul { get; set; } = Defaults.Consul;
        public string Cubbyhole { get; set; } = Defaults.Cubbyhole;
        public string Database { get; set; } = Defaults.Database;
        public string GoogleCloud { get; set; } = Defaults.GoogleCloud;
        public string GoogleCloudKMS { get; set; } = Defaults.GoogleCloudKMS;
        public string KeyManagement { get; set; } = Defaults.KeyManagement;
        public string KMIP { get; set; } = Defaults.KMIP;
        public string KeyValueV1 { get; set; } = Defaults.KeyValueV1;
        public string KeyValueV2 { get; set; } = Defaults.KeyValueV2;
        public string MongoDBAtlas { get; set; } = Defaults.MongoDBAtlas;
        public string Identity { get; set; } = Defaults.Identity;
        public string Nomad { get; set; } = Defaults.Nomad;
        public string OpenLDAP { get; set; } = Defaults.OpenLDAP;
        public string PKI { get; set; } = Defaults.PKI;
        public string RabbitMQ { get; set; } = Defaults.RabbitMQ;
        public string SSH { get; set; } = Defaults.SSH;
        public string TOTP { get; set; } = Defaults.TOTP;
        public string Transform { get; set; } = Defaults.Transform;
        public string Transit { get; set; } = Defaults.Transit;

        public string Cassandra { get; set; } = Defaults.Cassandra;
        public string HanaDB { get; set; } = Defaults.HanaDB;
        public string MongoDB { get; set; } = Defaults.MongoDB;
        public string MSSQL { get; set; } = Defaults.MSSQL;
        public string MySqlMariaDB { get; set; } = Defaults.MySqlMariaDB;
        public string PostgreSQL { get; set; } = Defaults.PostgreSQL;
        public string Oracle { get; set; } = Defaults.Oracle;
    }
}
