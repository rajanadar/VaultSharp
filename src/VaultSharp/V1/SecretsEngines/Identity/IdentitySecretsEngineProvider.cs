using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Identity
{
    internal class IdentitySecretsEngineProvider : IIdentitySecretsEngine
    {
        private readonly Polymath _polymath;

        public IdentitySecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<IdentityToken>> GetTokenAsync(string roleName, string mountPoint = SecretsEngineDefaultPaths.Identity, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<IdentityToken>>("v1/" + mountPoint.Trim('/') + "/oidc/token/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<bool>> IntrospectTokenAsync(string token, string clientId = null, string mountPoint = SecretsEngineDefaultPaths.Identity, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(token, "token");

            return await _polymath.MakeVaultApiRequest<Secret<bool>>("v1/" + mountPoint.Trim('/') + "/oidc/introspect", HttpMethod.Post, new { token, client_id = clientId }, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}