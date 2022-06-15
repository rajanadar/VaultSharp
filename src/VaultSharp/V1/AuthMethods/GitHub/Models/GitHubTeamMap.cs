using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.GitHub.Models;

public class GitHubTeamMap
{
    /// <summary>
    ///     GitHub team name in "slugified" format
    /// </summary>
    [JsonProperty("team_name")] public string team_name = "";

    /// <summary>
    ///     Comma separated list of policies to assign
    /// </summary>
    [JsonProperty("value")] public string value = "";
}