using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.AliCloud.Models;

namespace VaultSharp.V1.SecretsEngines.AliCloud
{
    internal class AliCloudSecretsEngineProvider : IAliCloudSecretsEngine
    {
        private readonly Polymath _polymath;

        public AliCloudSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task ConfigureRootCredentialsAsync(CreateRootCredentialsConfigModel createRootCredentialsConfigModel = null, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AliCloud, "/config", HttpMethod.Post, createRootCredentialsConfigModel, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RootCredentialsConfigModel>> ReadRootCredentialsConfigAsync(string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<RootCredentialsConfigModel>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AliCloud, "/config", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteRoleAsync(string roleName, CreateAliCloudRoleModel createAliCloudRoleModel, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AliCloud, "/role/" + roleName.Trim('/'), HttpMethod.Post, requestData: createAliCloudRoleModel, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AliCloudRoleModel>> ReadRoleAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<AliCloudRoleModel>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AliCloud, "/role/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllRolesAsync(string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AliCloud, "/role", _polymath.ListHttpMethod, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleAsync(string roleName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AliCloud, "/role/" + roleName.Trim('/'), HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AliCloudCredentials>> GetCredentialsAsync(string aliCloudRoleName, string aliCloudMountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(aliCloudRoleName, "aliCloudRoleName");

            return await _polymath.MakeVaultApiRequest<Secret<AliCloudCredentials>>(aliCloudMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.AliCloud, "/creds/" + aliCloudRoleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}