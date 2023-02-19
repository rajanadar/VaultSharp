using System.Text.Json.Serialization;


namespace VaultSharp.Core
{
    /// <summary>
    /// Represents the BackendListingVisibility
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BackendListingVisibility
    {
        hidden,
        unauth
    }
}