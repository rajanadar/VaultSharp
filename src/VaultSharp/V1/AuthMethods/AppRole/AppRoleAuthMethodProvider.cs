using System.Collections.Generic;
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
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/auth/" + mountPoint.Trim('/') + "/role", new HttpMethod("LIST")).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteRoleAsync(string roleName, AppRoleRoleModel appRoleRoleModel, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/'), HttpMethod.Post, requestData: appRoleRoleModel).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AppRoleRoleModel>> ReadRoleAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");
            
            return await _polymath.MakeVaultApiRequest<Secret<AppRoleRoleModel>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/'), HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
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

        public async Task<Secret<List<string>>> ReadRolePoliciesAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            var secret = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/policies", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            return _polymath.GetMappedSecret(secret, secret.Data["policies"] as List<string>);
        }

        public async Task WriteRolePoliciesAsync(string roleName, List<string> policies, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/policies", HttpMethod.Post, new { policies = policies }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRolePoliciesAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/policies", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<int>> ReadRoleSecretIdNumberOfUsesAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            var secret = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/secret-id-num-uses", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            return _polymath.GetMappedSecret(secret, (int)secret.Data["secret_id_num_uses"]);
        }

        public async Task WriteRoleSecretIdNumberOfUsesAsync(string roleName, int secretIdNumberOfUses, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/secret-id-num-uses", HttpMethod.Post, new { secret_id_num_uses = secretIdNumberOfUses }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleSecretIdNumberOfUsesAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/secret-id-num-uses", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<int>> ReadRoleSecretIdTimeToLiveAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            var secret = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/secret-id-ttl", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            return _polymath.GetMappedSecret(secret, (int)secret.Data["secret_id_ttl"]);
        }

        public async Task WriteRoleSecretIdTimeToLiveAsync(string roleName, int secretIdTimeToLive, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/secret-id-ttl", HttpMethod.Post, new { secret_id_ttl = secretIdTimeToLive }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleSecretIdTimeToLiveAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/secret-id-ttl", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<int>> ReadRoleTokenTimeToLiveAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            var secret = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/token-ttl", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            return _polymath.GetMappedSecret(secret, (int)secret.Data["token_ttl"]);
        }

        public async Task WriteRoleTokenTimeToLiveAsync(string roleName, int tokenTimeToLive, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/token-ttl", HttpMethod.Post, new { token_ttl = tokenTimeToLive }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleTokenTimeToLiveAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/token-ttl", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<int>> ReadRoleTokenMaximumTimeToLiveAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            var secret = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/token-max-ttl", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            return _polymath.GetMappedSecret(secret, (int)secret.Data["token_max_ttl"]);
        }

        public async Task WriteRoleTokenMaximumTimeToLiveAsync(string roleName, int tokenMaximumTimeToLive, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/token-max-ttl", HttpMethod.Post, new { token_max_ttl = tokenMaximumTimeToLive }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleTokenMaximumTimeToLiveAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/token-max-ttl", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<bool>> ReadRoleBindSecretIdAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            var secret = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/bind-secret-id", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            return _polymath.GetMappedSecret(secret, (bool)secret.Data["bind_secret_id"]);
        }

        public async Task WriteRoleBindSecretIdAsync(string roleName, bool bindSecretId, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/bind-secret-id", HttpMethod.Post, new { bind_secret_id = bindSecretId }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleBindSecretIdAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/bind-secret-id", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<List<string>>> ReadRoleSecretIdBoundCIDRsAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            var secret = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/secret-id-bound-cidrs", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            return _polymath.GetMappedSecret(secret, secret.Data["secret_id_bound_cidrs"] as List<string>);
        }

        public async Task WriteRoleSecretIdBoundCIDRsAsync(string roleName, List<string> secretIdBoundCIDRs, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/secret-id-bound-cidrs", HttpMethod.Post, new { secret_id_bound_cidrs = secretIdBoundCIDRs }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleSecretIdBoundCIDRsAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/secret-id-bound-cidrs", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<List<string>>> ReadRoleTokenBoundCIDRsAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            var secret = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/token-bound-cidrs", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            return _polymath.GetMappedSecret(secret, secret.Data["token_bound_cidrs"] as List<string>);
        }

        public async Task WriteRoleTokenBoundCIDRsAsync(string roleName, List<string> tokenBoundCIDRs, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/token-bound-cidrs", HttpMethod.Post, new { token_bound_cidrs = tokenBoundCIDRs }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRoleTokenBoundCIDRsAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/token-bound-cidrs", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<int>> ReadRolePeriodAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            var secret = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/period", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            return _polymath.GetMappedSecret(secret, (int)secret.Data["period"]);
        }

        public async Task WriteRolePeriodAsync(string roleName, int period, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/period", HttpMethod.Post, new { period = period }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRolePeriodAsync(string roleName, string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            await _polymath.MakeVaultApiRequest("v1/auth/" + mountPoint.Trim('/') + "/role/" + roleName.Trim('/') + "/period", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<Dictionary<string, object>>> TidyTokensAsync(string mountPoint = AuthMethodDefaultPaths.AppRole)
        {
            Checker.NotNull(mountPoint, "mountPoint");

            return await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>("v1/auth/" + mountPoint.Trim('/') + "/tidy/secret-id", HttpMethod.Post).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}
