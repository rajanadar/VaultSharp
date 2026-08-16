using System.Text.Json.Serialization;


namespace VaultSharp.Core
{
    /// <summary>
    /// Represents the BackendListingVisibility
    /// </summary>
#if NET8_0_OR_GREATER
    [JsonConverter(typeof(JsonStringEnumConverter<BackendListingVisibility>))]
#else
    [JsonConverter(typeof(JsonStringEnumConverter))]
#endif
    public enum BackendListingVisibility
    {
        hidden,
        unauth
    }
}