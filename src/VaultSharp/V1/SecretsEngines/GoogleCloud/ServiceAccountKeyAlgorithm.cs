using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.V1.SecretsEngines.GoogleCloud
{
    /// <summary>
    /// Represents the ServiceAccountKeyAlgorithm.
    /// https://cloud.google.com/iam/reference/rest/v1/projects.serviceAccounts.keys#ServiceAccountKeyAlgorithm
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ServiceAccountKeyAlgorithm
    {
        /// <summary>
        /// An unspecified key algorithm.
        /// </summary>
        KEY_ALG_UNSPECIFIED = 0,

        /// <summary>
        /// 1k RSA Key.
        /// </summary>
        KEY_ALG_RSA_1024 = 1,

        /// <summary>
        /// 2k RSA Key.
        /// </summary>
        KEY_ALG_RSA_2048 = 2
    }
}