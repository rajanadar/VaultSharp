using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory
{
    internal class ActiveDirectorySecretsEngineProvider : IActiveDirectorySecretsEngine
    {
        private readonly Polymath _polymath;

        public ActiveDirectorySecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<ActiveDirectoryCredentials>> GetCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<ActiveDirectoryCredentials>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/creds/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}