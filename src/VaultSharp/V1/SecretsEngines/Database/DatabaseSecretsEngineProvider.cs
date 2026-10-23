using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.Database.Models;

namespace VaultSharp.V1.SecretsEngines.Database
{
    internal class DatabaseSecretsEngineProvider : IDatabaseSecretsEngine
    {
        private readonly Polymath _polymath;

        public DatabaseSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }
        
        public async Task ConfigureConnectionAsync(string connectionName, ConnectionConfigModel connectionConfigModel, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/config/" + connectionName, HttpMethod.Post, requestData: connectionConfigModel, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
        
        public async Task<Secret<ConnectionModel>> ReadConnectionConfigAsync(string connectionName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(connectionName, "connectionName");

            return await _polymath.MakeVaultApiRequest<Secret<ConnectionModel>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/config/" + connectionName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
        
        public async Task<Secret<ListInfo>> ReadAllConnectionsAsync(string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/config?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
        
        public async Task DeleteConnectionAsync(string connectionName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/config/" + connectionName, HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
        
        public async Task ResetConnectionAsync(string connectionName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/reset/" + connectionName, HttpMethod.Post, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
        
        public async Task ReloadPluginAsync(string pluginName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/reload/" + pluginName, HttpMethod.Post, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
        
        public async Task RotateRootCredentialsAsync(string connectionName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/rotate-root/" + connectionName, HttpMethod.Post, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task CreateRoleAsync(string roleName, Role role, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(role, "role");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/roles/" + roleName.Trim('/'), HttpMethod.Post, role, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Role>> ReadRoleAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<Role>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/roles/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllRolesAsync(string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/roles", _polymath.ListHttpMethod, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleAsync(string roleName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/roles/" + roleName.Trim('/'), HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UsernamePasswordCredentials>> GetCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<UsernamePasswordCredentials>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/creds/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task CreateStaticRoleAsync(string roleName, StaticRole staticRole, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(staticRole, "staticRole");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/static-roles/" + roleName.Trim('/'), HttpMethod.Post, staticRole, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<StaticRole>> ReadStaticRoleAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<StaticRole>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/static-roles/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllStaticRolesAsync(string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/static-roles", _polymath.ListHttpMethod, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteStaticRoleAsync(string roleName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/static-roles/" + roleName.Trim('/'), HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<StaticCredentials>> GetStaticCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<StaticCredentials>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/static-creds/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RotateStaticCredentialsAsync(string roleName, string mountPoint = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Database, "/rotate-role/" + roleName.Trim('/'), HttpMethod.Post, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}