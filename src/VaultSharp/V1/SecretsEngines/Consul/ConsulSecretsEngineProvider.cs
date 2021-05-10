using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Consul
{
    internal class ConsulSecretsEngineProvider : IConsulSecretsEngine
    {
        private readonly Polymath _polymath;

        public ConsulSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<ConsulCredentials>> GetCredentialsAsync(string consulRoleName, string consulBackendMountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(consulRoleName, "consulRoleName");

            return await _polymath.MakeVaultApiRequest<Secret<ConsulCredentials>>(consulBackendMountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Consul, "/creds/" + consulRoleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}