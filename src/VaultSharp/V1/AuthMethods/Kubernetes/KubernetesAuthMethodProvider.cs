using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.Kubernetes.Models;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.Kubernetes;

internal class KubernetesAuthMethodProvider : IKubernetesAuthMethod
{
    private readonly Polymath _polymath;

    public KubernetesAuthMethodProvider(Polymath polymath)
    {
        Checker.NotNull(polymath, "polymath");
        _polymath = polymath;
    }

    public async Task<Secret<KubernetesConfig>> ReadKubernetesConfig(
        string mountPoint = AuthMethodDefaultPaths.Kubernetes)
    {
        return await _polymath
            .MakeVaultApiRequest<Secret<KubernetesConfig>>(
                $"v1/auth/{mountPoint}/config", HttpMethod.Get)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task WriteKubernetesConfig(KubernetesConfig config,
        string mountPoint = AuthMethodDefaultPaths.Kubernetes)
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

    public async Task<Secret<KubernetesRole>> ReadKubernetesRole(string roleName,
        string mountPoint = AuthMethodDefaultPaths.Kubernetes)
    {
        return await _polymath
            .MakeVaultApiRequest<Secret<KubernetesRole>>(
                $"v1/auth/{mountPoint}/role/{roleName}", HttpMethod.Get)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task CreateKubernetesRole(KubernetesRole role, string mountPoint = AuthMethodDefaultPaths.Kubernetes)
    {
        var configWithoutNullProperties = JsonConvert
            .DeserializeObject(JsonConvert.SerializeObject(
                role,
                new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore}
            ))!;

        await _polymath
            .MakeVaultApiRequest($"v1/auth/{mountPoint}/role/{role.name}", HttpMethod.Post, configWithoutNullProperties)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task<Secret<ListInfo>> ListKubernetesRoles(string mountPoint = AuthMethodDefaultPaths.Kubernetes)
    {
        return await _polymath
            .MakeVaultApiRequest<Secret<ListInfo>>($"v1/auth/{mountPoint}/role" + "?list=true",
                HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }

    public async Task DeleteKubernetesRole(string roleName, string mountPoint = AuthMethodDefaultPaths.Kubernetes)
    {
        await _polymath.MakeVaultApiRequest($"v1/auth/{mountPoint}/role/{roleName}", HttpMethod.Delete)
            .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
    }
}