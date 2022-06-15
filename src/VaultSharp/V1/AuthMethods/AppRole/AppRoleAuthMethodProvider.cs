using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.AppRole.Models;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.AppRole;

internal class AppRoleAuthMethodProvider : IAppRoleAuthMethod
{
    private readonly Polymath _polymath;

    public AppRoleAuthMethodProvider(Polymath polymath)
    {
        Checker.NotNull(polymath, "polymath");
        _polymath = polymath;
    }

    public async Task<Secret<AppRoleInfo>> ReadRoleAsync(string roleName,
        string mountPoint = AuthMethodDefaultPaths.AppRole)
    {
        Checker.NotNull(mountPoint, "mountPoint");
        Checker.NotNull(roleName, "roleName");
        return await _polymath
            .MakeVaultApiRequest<Secret<AppRoleInfo>>(
                "v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/'), HttpMethod.Get)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task WriteAppRoleRoleAsync(AppRoleRole role, string mountPoint = AuthMethodDefaultPaths.AppRole)
    {
        var configWithoutNullProperties = JsonConvert
            .DeserializeObject(JsonConvert.SerializeObject(
                role,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}
            ))!;
        //var requestData = new {role.role_name, role.token_policies};
        await _polymath
            .MakeVaultApiRequest($"v1/auth/{mountPoint}/role/{role.role_name}", HttpMethod.Post,
                configWithoutNullProperties)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task WriteCustomAppRoleId(string roleName, string customRoleId,
        string mountPoint = AuthMethodDefaultPaths.AppRole)
    {
        var requestData = new {role_id = customRoleId};
        //var requestData = new {role.role_name, role.token_policies};
        await _polymath
            .MakeVaultApiRequest($"v1/auth/{mountPoint}/role/{roleName}", HttpMethod.Post,
                requestData)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task<Secret<RoleId>> ReadRoleIdAsync(string roleName,
        string mountPoint = AuthMethodDefaultPaths.AppRole)
    {
        var requestData = new {roleName};
        return await _polymath
            .MakeVaultApiRequest<Secret<RoleId>>($"v1/auth/{mountPoint}/role/{roleName}/role-id", HttpMethod.Get)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task<Secret<ListInfo>> ReadAllAppRoles(string mountPoint = AuthMethodDefaultPaths.AppRole)
    {
        return await _polymath
            .MakeVaultApiRequest<Secret<ListInfo>>($"v1/auth/{mountPoint}/role" + "?list=true",
                HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task<Secret<SecretId>> CreateSecretId(string roleName,
        string mountPoint = AuthMethodDefaultPaths.AppRole)
    {
        var requestData = new {roleName};
        return await _polymath
            .MakeVaultApiRequest<Secret<SecretId>>($"v1/auth/{mountPoint}/role/{roleName}/secret-id",
                HttpMethod.Post)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task<Secret<WrapInfo>> CreateResponseWrappedSecretId(string wrapTimeToLive, string roleName,
        string mountPoint = AuthMethodDefaultPaths.AppRole)
    {
        var requestData = new {roleName};
        return await _polymath
            .MakeVaultApiRequest<Secret<WrapInfo>>($"v1/auth/{mountPoint}/role/{roleName}/secret-id",
                HttpMethod.Post, wrapTimeToLive: wrapTimeToLive)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }
}