using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.OpenLDAP
{
    internal class OpenLDAPSecretsEngineProvider : IOpenLDAPSecretsEngine
    {
        private readonly Polymath _polymath;

        public OpenLDAPSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task CreateDynamicRoleAsync(string roleName, DynamicRole role, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(role, "role");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.OpenLDAP, "/role/" + roleName.Trim('/'), HttpMethod.Post, role, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<DynamicRole>> ReadDynamicRoleAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<DynamicRole>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.OpenLDAP, "/role/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllDynamicRolesAsync(string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.OpenLDAP, "/role/", _polymath.ListHttpMethod, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteDynamicRoleAsync(string roleName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.OpenLDAP, "/role/" + roleName.Trim('/'), HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<LDAPCredentials>> GetDynamicCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<LDAPCredentials>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.OpenLDAP, "/creds/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task CreateStaticRoleAsync(string roleName, StaticRole staticRole, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.OpenLDAP, "/static-role/" + roleName.Trim('/'), HttpMethod.Post, staticRole, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<StaticRole>> ReadStaticRoleAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<StaticRole>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.OpenLDAP, "/static-role/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllStaticRolesAsync(string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.OpenLDAP, "/static-role/", _polymath.ListHttpMethod, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteStaticRoleAsync(string roleName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.OpenLDAP, "/static-role/" + roleName.Trim('/'), HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<StaticCredentials>> GetStaticCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<StaticCredentials>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.OpenLDAP, "/static-cred/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RotateStaticCredentialsAsync(string roleName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.OpenLDAP, "/rotate-role/" + roleName.Trim('/'), HttpMethod.Post, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}