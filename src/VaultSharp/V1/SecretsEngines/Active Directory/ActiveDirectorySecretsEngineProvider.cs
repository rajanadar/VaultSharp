using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.ActiveDirectory.Models;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory
{
    internal class ActiveDirectorySecretsEngineProvider : IActiveDirectorySecretsEngine
    {
        private readonly Polymath _polymath;

        public IActiveDirectoryLibrary Library { get; }

        public ActiveDirectorySecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
            Library = new ActiveDirectoryLibraryProvider(_polymath);
        }

        public async Task ConfigureConnectionAsync(CreateConnectionConfigModel createConnectionConfigModel, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/config", HttpMethod.Post, requestData: createConnectionConfigModel, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ConnectionConfigModel>> ReadConnectionAsync(string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ConnectionConfigModel>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/config", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteConnectionAsync(string mountPoint = null, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/config", HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteRoleAsync(string roleName, CreateActiveDirectoryRoleModel createActiveDirectoryRoleModel, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/roles/" + roleName.Trim('/'), HttpMethod.Post, requestData: createActiveDirectoryRoleModel, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ActiveDirectoryRoleModel>> ReadRoleAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<ActiveDirectoryRoleModel>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/roles/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllRolesAsync(string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/roles", _polymath.ListHttpMethod, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleAsync(string roleName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/roles/" + roleName.Trim('/'), HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ActiveDirectoryCredentials>> GetCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<ActiveDirectoryCredentials>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/creds/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> RotateRootCredentialsAsync(string mountPoint = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/rotate-root", HttpMethod.Post, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> ReadRotateRootCredentialsStatusAsync(string mountPoint = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/rotate-root", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> RotateRoleCredentialsAsync(string roleName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/rotate-role/" + roleName.Trim('/'), HttpMethod.Post, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}