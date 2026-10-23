using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Terraform
{
    internal class TerraformSecretsEngineProvider : ITerraformSecretsEngine
    {
        private readonly Polymath _polymath;

        public TerraformSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<TerraformCredentials>> GetCredentialsAsync(string roleName, string mountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<TerraformCredentials>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.Terraform, "/creds/" + roleName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}