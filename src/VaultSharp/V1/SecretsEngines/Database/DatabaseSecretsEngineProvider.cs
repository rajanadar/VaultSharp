using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Database
{
    internal class DatabaseSecretsEngineProvider : IDatabaseSecretsEngine
    {
        private readonly Polymath _polymath;

        public DatabaseSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task CreateRoleAsync(string roleName, Role role, string mountPoint = SecretsEngineDefaultPaths.Database)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(role, "role");

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/roles/" + roleName.Trim('/'), HttpMethod.Post, role).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Role>> ReadRoleAsync(string roleName, string mountPoint = SecretsEngineDefaultPaths.Database, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<Role>>("v1/" + mountPoint.Trim('/') + "/roles/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllRolesAsync(string mountPoint = SecretsEngineDefaultPaths.Database, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/" + mountPoint.Trim('/') + "/roles?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleAsync(string roleName, string mountPoint = SecretsEngineDefaultPaths.Database)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/roles/" + roleName.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<UsernamePasswordCredentials>> GetCredentialsAsync(string roleName, string mountPoint = SecretsEngineDefaultPaths.Database, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<UsernamePasswordCredentials>>("v1/" + mountPoint.Trim('/') + "/creds/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task CreateStaticRoleAsync(string roleName, StaticRole staticRole, string mountPoint = SecretsEngineDefaultPaths.Database)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(staticRole, "staticRole");

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/static-roles/" + roleName.Trim('/'), HttpMethod.Post, staticRole).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<StaticRole>> ReadStaticRoleAsync(string roleName, string mountPoint = SecretsEngineDefaultPaths.Database, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<StaticRole>>("v1/" + mountPoint.Trim('/') + "/static-roles/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllStaticRolesAsync(string mountPoint = SecretsEngineDefaultPaths.Database, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/" + mountPoint.Trim('/') + "/static-roles?list=true", HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteStaticRoleAsync(string roleName, string mountPoint = SecretsEngineDefaultPaths.Database)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/static-roles/" + roleName.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<StaticCredentials>> GetStaticCredentialsAsync(string roleName, string mountPoint = SecretsEngineDefaultPaths.Database, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<StaticCredentials>>("v1/" + mountPoint.Trim('/') + "/static-creds/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task RotateStaticCredentialsAsync(string roleName, string mountPoint = SecretsEngineDefaultPaths.Database)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/" + mountPoint.Trim('/') + "/rotate-role/" + roleName.Trim('/'), HttpMethod.Post).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}