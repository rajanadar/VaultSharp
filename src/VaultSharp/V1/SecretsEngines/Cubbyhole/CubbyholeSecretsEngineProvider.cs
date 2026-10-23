using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Cubbyhole
{
    internal class CubbyholeSecretsEngineProvider : ICubbyholeSecretsEngine
    {
        private readonly Polymath _polymath;

        public CubbyholeSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<Dictionary<string, object>>> ReadSecretAsync(string secretPath, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(secretPath, "secretPath");
            return await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, object>>>(_polymath.VaultClientSettings.SecretsEngineMountPoints.Cubbyhole, "/" + secretPath.TrimStart('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadSecretPathsAsync(string folderPath, string wrapTimeToLive = null, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(folderPath, "folderPath");
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(_polymath.VaultClientSettings.SecretsEngineMountPoints.Cubbyhole, "/" + folderPath.TrimStart('/'), _polymath.ListHttpMethod, wrapTimeToLive: wrapTimeToLive, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteSecretAsync(string secretPath, IDictionary<string, object> values, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(secretPath, "secretPath");
            await _polymath.MakeVaultApiRequest(_polymath.VaultClientSettings.SecretsEngineMountPoints.Cubbyhole, "/" + secretPath.Trim('/'), HttpMethod.Post, values, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteSecretAsync(string secretPath, CancellationToken cancellationToken = default)
        {
            Checker.NotNull(secretPath, "secretPath");
            await _polymath.MakeVaultApiRequest(_polymath.VaultClientSettings.SecretsEngineMountPoints.Cubbyhole, "/" + secretPath.Trim('/'), HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}