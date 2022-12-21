using System;
using Newtonsoft.Json;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory.Models
{
    public class CreateConnectionConfigModel : ConnectionConfigModel
    {
        [JsonProperty("bindpassword")]
        public string BindingPassword { get; set; }

        public CreateConnectionConfigModel()
        {
            ConnectionRequestTimeout = 90;
        }
    }
}