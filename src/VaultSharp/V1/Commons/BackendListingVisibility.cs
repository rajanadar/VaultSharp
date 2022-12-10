using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VaultSharp.Core
{
    /// <summary>
    /// Represents the BackendListingVisibility
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BackendListingVisibility
    {
        hidden,
        unauth
    }
}