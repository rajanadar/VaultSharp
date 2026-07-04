using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.PKI.Roles
{
    internal class PKISecretRolesProvider : IPKIRoles
    {
        private readonly Polymath _polymath;

        public PKISecretRolesProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<RolesData>> ListRoles(string pkiBackendMountPoint = null, string wrapTimeToLive = null)
        {
            var result = await _polymath.MakeVaultApiRequest<Secret<RolesData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI,
                    "/roles/?list=true", HttpMethod.Get,
                    wrapTimeToLive: wrapTimeToLive)
                .ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<RoleData>> AddRole(RoleData data, string pkiBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(data, "data");
            var result = await _polymath.MakeVaultApiRequest<Secret<RoleData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/roles/" + data.Name, HttpMethod.Post, data, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return result;
        }

        public async Task<Secret<RoleData>> GetRole(string roleName, string pkiBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(roleName, "roleName");
            var result = await _polymath.MakeVaultApiRequest<Secret<RoleData>>(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/roles/" + roleName, HttpMethod.Get, null, wrapTimeToLive:wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            result.Data.Name = roleName;
            return result;
        }

        public async Task DeleteRole(string roleName, string pkiBackendMountPoint = null)
        {
            Checker.NotNull(roleName, "roleName");
            await _polymath.MakeVaultApiRequest(pkiBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.PKI, "/roles/" + roleName, HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}
