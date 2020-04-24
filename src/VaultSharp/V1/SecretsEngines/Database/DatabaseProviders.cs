
namespace VaultSharp.V1.SecretsEngines.Database
{
    /// <summary>
    /// Names of supported database providers
    /// </summary>
    public static class DatabaseProviders
    {
        public const string MySQL = "mysql";
        public const string PostgreSQL = "postgresql";
        public const string MongoDB = "mongodb";
        public const string Oracle = "oracle";
        public const string RedShift = "redshift";
    }
}