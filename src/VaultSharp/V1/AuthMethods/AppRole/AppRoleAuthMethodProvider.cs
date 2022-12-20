using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.AppRole.Models;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.AppRole
{
    internal class AppRoleAuthMethodProvider : IAppRoleAuthMethod
    {
        private readonly Polymath _polymath;

        public AppRoleAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }

        public async Task<Secret<ListInfo>> ReadAllRolesAsync(string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/auth/" + mountPoint.Trim('/') + "/roles?list=true", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteRoleAsync(string roleName, CreateAppRoleRoleModel createAppRoleRoleModel, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/'), HttpMethod.Post, requestData: createAppRoleRoleModel).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AppRoleInfo>> ReadRoleAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            
            return await _polymath.MakeVaultApiRequest<Secret<AppRoleInfo>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/'), HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RoleIdInfo>> GetRoleIdAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<RoleIdInfo>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/role-id", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RoleIdInfo>> WriteRoleIdAsync(string roleName, RoleIdInfo roleIdInfo, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleIdInfo, "roleIdInfo");
            Checker.NotNull(roleIdInfo.RoleId, "roleIdInfo.RoleId");

            return await _polymath.MakeVaultApiRequest<Secret<RoleIdInfo>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/role-id", HttpMethod.Post, requestData: roleIdInfo).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SecretIdInfo>> PullNewSecretIdAsync(string roleName, PullSecretIdRequestOptions secretIdRequestOptions = null, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<SecretIdInfo>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/secret-id", HttpMethod.Post, secretIdRequestOptions).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllSecretIdAccessorsAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "secret-id?list=true", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SecretIdInfo>> ReadSecretIdInfoAsync(string roleName, string secretId, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(secretId, "secretId");

            return await _polymath.MakeVaultApiRequest<Secret<SecretIdInfo>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/secret-id/lookup", HttpMethod.Post, requestData: new { secret_id = secretId }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DestroySecretIdAsync(string roleName, string secretId, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(secretId, "secretId");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/secret-id/destroy", HttpMethod.Post, requestData: new { secret_id = secretId }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SecretIdInfo>> ReadSecretIdInfoByAccessorAsync(string roleName, string secretIdAccessor, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(secretIdAccessor, "secretIdAccessor");

            return await _polymath.MakeVaultApiRequest<Secret<SecretIdInfo>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/secret-id-accessor/lookup", HttpMethod.Post, requestData: new { secret_id_accessor = secretIdAccessor }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DestroySecretIdByAccessorAsync(string roleName, string secretIdAccessor, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            Checker.NotNull(secretIdAccessor, "secretIdAccessor");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/secret-id-accessor/destroy", HttpMethod.Post, requestData: new { secret_id_accessor = secretIdAccessor }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<SecretIdInfo>> PushNewSecretIdAsync(string roleName, PushSecretIdRequestOptions secretIdRequestOptions = null, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<SecretIdInfo>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/custom-secret-id", HttpMethod.Post, secretIdRequestOptions).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}
