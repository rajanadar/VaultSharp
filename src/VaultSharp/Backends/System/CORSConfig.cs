using System.Collections.Generic;
using Newtonsoft.Json;

namespace VaultSharp.Backends.System
{
    public class CORSConfig
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("allowed_origins")]
        public IEnumerable<string> AllowedOrigins { get; set; }

        [JsonProperty("allowed_headers")]
        public IEnumerable<string> AllowedHeaders { get; set; }
    }
}