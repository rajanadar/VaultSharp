using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.V1.SecretsEngines.GoogleCloud
{
    /// <summary>
    /// Represents the ServiceAccountPrivateKeyType.
    /// https://cloud.google.com/iam/reference/rest/v1/projects.serviceAccounts.keys#ServiceAccountPrivateKeyType
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ServiceAccountPrivateKeyType
    {
        /// <summary>
        /// Unspecified. Equivalent to TYPE_GOOGLE_CREDENTIALS_FILE.
        /// </summary>
        TYPE_UNSPECIFIED = 0,

        /// <summary>
        /// PKCS12 format. 
        /// The password for the PKCS12 file is notasecret. 
        /// For more information, see https://tools.ietf.org/html/rfc7292.
        /// </summary>
        TYPE_PKCS12_FILE = 1,

        /// <summary>
        /// Google Credentials File format.
        /// </summary>
        TYPE_GOOGLE_CREDENTIALS_FILE = 2
    }
}