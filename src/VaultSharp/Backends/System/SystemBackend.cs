using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Backends;
using VaultSharp.Backends.System;

namespace VaultSharp
{
    internal class SystemBackend : BackendConnector, ISystemBackend
    {
        public async Task<SealStatus> GetSealStatusAsync()
        {
            var response = await MakeVaultApiRequest<SealStatus>("sys/seal-status", HttpMethod.Get).ConfigureAwait(continueOnCapturedContext: _continueAsyncTasksOnCapturedContext);
            return response;
        }

        public Task<string> HashWithAuditBackendAsync(string mountPoint, string inputToHash)
        {
            throw new System.NotImplementedException();
        }
    }
}
