using Newtonsoft.Json;

namespace VaultSharp.Samples
{
    public class FooData
    {
        [JsonProperty("foo")]
        public string Foo { get; set; }
        [JsonProperty("foo2")]
        public int Foo2 { get; set; }
    }
}