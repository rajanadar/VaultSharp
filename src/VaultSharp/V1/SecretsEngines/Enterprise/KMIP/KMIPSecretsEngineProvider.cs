using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Enterprise.KMIP
{
    internal class KMIPSecretsEngineProvider : IKMIPSecretsEngine
    {
        private readonly Polymath _polymath;

        public KMIPSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<KMIPCredentials>> GetCredentialsAsync(string scopeName, string roleName, CertificateFormat format = CertificateFormat.pem, string mountPoint = SecretsEngineDefaultPaths.KMIP, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(scopeName, "scopeName");
            Checker.NotNull(roleName, "roleName");

            return await _polymath.MakeVaultApiRequest<Secret<KMIPCredentials>>("v1/" + mountPoint.Trim('/') + "/scope/" + scopeName.Trim('/') + "/role/" + scopeName.Trim('/') + "/credential/generate", HttpMethod.Post, new { format }, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}