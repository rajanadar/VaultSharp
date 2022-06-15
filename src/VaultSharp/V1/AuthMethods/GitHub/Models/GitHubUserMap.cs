using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.GitHub.Models;

public class GitHubUserMap
{
    /// <summary>
    ///     GitHub user name in "slugified" format
    /// </summary>
    [JsonProperty("user_name")] public string user_name = "";

    /// <summary>
    ///     Comma separated list of policies to assign
    /// </summary>
    [JsonProperty("value")] public string value = "";
}