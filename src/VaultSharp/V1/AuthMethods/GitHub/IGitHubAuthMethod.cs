using System.Threading.Tasks;
using VaultSharp.V1.AuthMethods.GitHub.Models;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.GitHub;

/// <summary>
/// </summary>
public interface IGitHubAuthMethod
{
    /// <summary>
    ///     Reads the configuration of an existing Github Auth method.
    /// </summary>
    /// <param name="organization"> Github organization where the Config should be read from</param>
    /// <param name="mountPoint">Mount point of the Github Auth method</param>
    /// <returns></returns>
    Task<Secret<GitHubConfig>> ReadGitHubConfig(string organization, string mountPoint = "github");

    /// <summary>
    ///     Writes the configuration of an existing Github Auth method.
    /// </summary>
    /// <param name="config">Github Configuration which should be applied</param>
    /// <param name="mountPoint">Mount point of the Github Auth method</param>
    /// <returns></returns>
    Task WriteGitHubConfig(GitHubConfig config, string mountPoint = "github");

    /// <summary>
    ///     Reads the GitHub team policy mapping.
    /// </summary>
    /// <param name="organization"> Github organization where the team Config should be read from</param>
    /// <param name="mountPoint">Mount point of the Github Auth method</param>
    /// <returns></returns>
    Task<Secret<GitHubTeamMap>> ReadGitHubTeamMap(string teamName, string mountPoint = "github");

    /// <summary>
    ///     Map a list of policies to a team that exists in the configured GitHub organization.
    /// </summary>
    /// <param name="organization"> Github organization where the team Config should be applied</param>
    /// <param name="teamMap"> Team policy Config which should be applied</param>
    /// <param name="mountPoint">Mount point of the Github Auth method</param>
    /// <returns></returns>
    Task WriteGitHubTeamMap(GitHubTeamMap teamMap, string mountPoint = "github");

    /// <summary>
    ///     Reads the GitHub team policy mapping.
    /// </summary>
    /// <param name="organization"> Github organization where the team Config should be read from</param>
    /// <param name="mountPoint">Mount point of the Github Auth method</param>
    /// <returns></returns>
    Task<Secret<GitHubUserMap>> ReadGitHubUserMap(string userName, string mountPoint = "github");

    /// <summary>
    ///     Map a list of policies to a team that exists in the configured GitHub organization.
    /// </summary>
    /// <param name="organization"> Github organization where the team Config should be applied</param>
    /// <param name="userMap"> Team policy Config which should be applied</param>
    /// <param name="mountPoint">Mount point of the Github Auth method</param>
    /// <returns></returns>
    Task WriteGitHubUserMap(GitHubUserMap userMap, string mountPoint = "github");
}