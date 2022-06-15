using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.UserPass.Models;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.UserPass;

internal class UserPassAuthMethodProvider : IUserPassAuthMethod
{
    private readonly Polymath _polymath;

    public UserPassAuthMethodProvider(Polymath polymath)
    {
        Checker.NotNull(polymath, "polymath");
        _polymath = polymath;
    }

    public async Task CreateOrUpdateUser(UserPassUser user, string mountPoint = AuthMethodDefaultPaths.UserPass)
    {
        var configWithoutNullProperties = JsonConvert
            .DeserializeObject(JsonConvert.SerializeObject(
                user,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}
            ))!;

        await _polymath
            .MakeVaultApiRequest($"v1/auth/{mountPoint}/users/{user.Username.ToLower()}", HttpMethod.Post,
                configWithoutNullProperties)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task DeleteUser(string username,
        string mountPoint = null)
    {
        await _polymath.MakeVaultApiRequest(
                $"v1/auth/{mountPoint}/users/{username.ToLower()}",
                HttpMethod.Delete)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task UpdatePasswordOnUser(string username, string password,
        string mountPoint = AuthMethodDefaultPaths.UserPass)
    {
        await _polymath
            .MakeVaultApiRequest($"v1/auth/{mountPoint}/users/{username.ToLower()}/password", HttpMethod.Post,
                new {password})
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task UpdatePoliciesOnUser(string username, List<string> policies,
        string mountPoint = AuthMethodDefaultPaths.UserPass)
    {
        await _polymath
            .MakeVaultApiRequest($"v1/auth/{mountPoint}/users/{username.ToLower()}/policies", HttpMethod.Post,
                new {policies})
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task<Secret<ListInfo>> ListUsers(string mountPoint = AuthMethodDefaultPaths.UserPass,
        string wrapTimeToLive = null)
    {
        return await _polymath
                   .MakeVaultApiRequest<Secret<ListInfo>>(
                       $"v1/auth/{mountPoint}/users?list=true",
                       HttpMethod.Get, wrapTimeToLive: wrapTimeToLive)
                   .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext) ??
               new Secret<ListInfo> {Data = new ListInfo {Keys = new List<string>()}};
    }

    public async Task<Secret<ReadUserResponse>> ReadUser(string username,
        string mountPoint = AuthMethodDefaultPaths.UserPass)
    {
        return await _polymath
            .MakeVaultApiRequest<Secret<ReadUserResponse>>(
                $"v1/auth/{mountPoint}/users/{username.ToLower()}", HttpMethod.Get)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }
}