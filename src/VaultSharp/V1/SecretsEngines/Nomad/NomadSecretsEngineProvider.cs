using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Nomad
{
    internal class NomadSecretsEngineProvider : INomadSecretsEngine
    {
        private readonly Polymath _polymath;

        public NomadSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<NomadCredentials>> GetCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<NomadCredentials>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Nomad, "/creds/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}