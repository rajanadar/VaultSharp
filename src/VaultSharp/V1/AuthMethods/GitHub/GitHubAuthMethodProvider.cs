using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.GitHub;
using VaultSharp.V1.AuthMethods.GitHub.Models;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.AppRole;

internal class GitHubAuthMethodProvider : IGitHubAuthMethod
{
    private readonly Polymath _polymath;

    public GitHubAuthMethodProvider(Polymath polymath)
    {
        Checker.NotNull(polymath, "polymath");
        _polymath = polymath;
    }

    public async Task<Secret<GitHubConfig>> ReadGitHubConfig(string organization, string mountPoint = "github")
    {
        return await _polymath
            .MakeVaultApiRequest<Secret<GitHubConfig>>(
                $"v1/auth/{mountPoint}/config", HttpMethod.Get)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task WriteGitHubConfig(GitHubConfig config, string mountPoint = "github")
    {
        var configWithoutNullProperties = JsonConvert
            .DeserializeObject(JsonConvert.SerializeObject(
                config,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}
            ))!;

        await _polymath
            .MakeVaultApiRequest($"v1/auth/{mountPoint}/config", HttpMethod.Post, configWithoutNullProperties)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task<Secret<GitHubTeamMap>> ReadGitHubTeamMap(string teamName, string mountPoint = "github")
    {
        return await _polymath
            .MakeVaultApiRequest<Secret<GitHubTeamMap>>(
                $"v1/auth/{mountPoint}/map/teams/{teamName}", HttpMethod.Get)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task WriteGitHubTeamMap(GitHubTeamMap teamMap, string mountPoint = "github")
    {
        await _polymath
            .MakeVaultApiRequest($"v1/auth/{mountPoint}/map/teams/{teamMap.team_name}", HttpMethod.Post,
                new {teamMap.value})
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task<Secret<GitHubUserMap>> ReadGitHubUserMap(string userName, string mountPoint = "github")
    {
        return await _polymath
            .MakeVaultApiRequest<Secret<GitHubUserMap>>(
                $"v1/auth/{mountPoint}/map/users/{userName}", HttpMethod.Get)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task WriteGitHubUserMap(GitHubUserMap userMap, string mountPoint = "github")
    {
        //var configWithoutNullProperties = JsonConvert
        //    .DeserializeObject(JsonConvert.SerializeObject(
        //        userMap,
        //        new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}
        //    ))!;
        await _polymath
            .MakeVaultApiRequest($"v1/auth/{mountPoint}/map/users/{userMap.user_name}", HttpMethod.Post,
                new {userMap.value})
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }
}