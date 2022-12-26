using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.Consul.Models;

namespace VaultSharp.V1.SecretsEngines.Consul
{
    internal class ConsulSecretsEngineProvider : IConsulSecretsEngine
    {
        private readonly Polymath _polymath;

        public ConsulSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task ConfigureAccesssync(AccessConfigModel accessConfigModel, string mountPoint = null)
        {
            Checker.NotNull(accessConfigModel, "accessConfigModel");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Consul, "/config/access", HttpMethod.Post, accessConfigModel).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteRoleAsync(string roleName, CreateConsulRoleModel createConsulRoleModel, string mountPoint = null)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Consul, "/roles/" + roleName.Trim('/'), HttpMethod.Post, requestData: createConsulRoleModel).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ConsulRoleModel>> ReadRoleAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<ConsulRoleModel>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Consul, "/roles/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllRolesAsync(string mountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Consul, "/roles", _polymath.ListHttpMethod, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleAsync(string roleName, string mountPoint = null)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Consul, "/roles/" + roleName.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ConsulCredentials>> GetCredentialsAsync(string consulRoleName, string consulBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(consulRoleName, "consulRoleName");

            return await _polymath.MakeVaultApiRequest<Secret<ConsulCredentials>>(consulBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Consul, "/creds/" + consulRoleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}