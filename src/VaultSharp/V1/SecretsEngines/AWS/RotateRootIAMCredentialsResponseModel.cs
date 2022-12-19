using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.AWS
{
    public class RotateRootIAMCredentialsResponseModel
    {
        [JsonProperty("access_key")]
        public string NewAccessKey { get; set; }
    }
}