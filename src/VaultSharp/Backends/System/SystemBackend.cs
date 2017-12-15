using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace VaultSharp.Backends.System
{
    internal class SystemBackend : ISystemBackend
    {
        private readonly BackendConnector backendConnector;

        public SystemBackend(BackendConnector backendConnector)
        {
            this.backendConnector = backendConnector;
        }

        public async Task<bool> GetInitStatusAsync()
        {
            var response = await backendConnector.MakeVaultApiRequest<dynamic>("v1/sys/init", HttpMethod.Get).ConfigureAwait(backendConnector.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response.initialized;
        }

        public async Task<MasterCredentials> InitAsync(InitOptions initOptions)
        {
            var response = await backendConnector.MakeVaultApiRequest<MasterCredentials>("v1/sys/init", HttpMethod.Put, initOptions).ConfigureAwait(backendConnector.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task SealAsync()
        {
            await backendConnector.MakeVaultApiRequest("v1/sys/seal", HttpMethod.Put).ConfigureAwait(backendConnector.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<SealStatus> GetSealStatusAsync()
        {
            var response = await backendConnector.MakeVaultApiRequest<SealStatus>("v1/sys/seal-status", HttpMethod.Get).ConfigureAwait(backendConnector.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task<SealStatus> UnsealAsync(string masterShareKey = null, bool resetCompletely = false)
        {
            var requestData = new
            {
                key = masterShareKey,
                reset = resetCompletely
            };

            var response = await backendConnector.MakeVaultApiRequest<SealStatus>("v1/sys/unseal", HttpMethod.Put, requestData).ConfigureAwait(backendConnector.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response;
        }

        public Task<string> HashWithAuditBackendAsync(string mountPoint, string inputToHash)
        {
            throw new NotImplementedException();
        }
    }
}
