using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;

namespace VaultSharp.Backends.System
{
    internal class SystemBackend : ISystemBackend
    {
        private readonly Polymath _polymath;

        public SystemBackend(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<Dictionary<string, AbstractAuditBackend>>> GetAuditBackendsAsync()
        {
            var response = await _polymath.MakeVaultApiRequest<Secret<Dictionary<string, AbstractAuditBackend>>>("v1/sys/audit", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);

            foreach (var kv in response.Data)
            {
                kv.Value.MountPoint = kv.Key;
            }

            return response;
        }

        public async Task MountAuditBackendAsync(AbstractAuditBackend abstractAuditBackend)
        {
            if (string.IsNullOrWhiteSpace(abstractAuditBackend.MountPoint))
            {
                abstractAuditBackend.MountPoint = abstractAuditBackend.Type.Value;
            }

            await _polymath.MakeVaultApiRequest("v1/sys/audit/" + abstractAuditBackend.MountPoint.Trim('/'), HttpMethod.Put, abstractAuditBackend).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task UnmountAuditBackendAsync(string mountPoint)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/audit/" + mountPoint.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<AuditHash>> AuditHashAsync(string mountPoint, string inputToHash)
        {
            var requestData = new { input = inputToHash };
            return await _polymath.MakeVaultApiRequest<Secret<AuditHash>>("v1/sys/audit-hash/" + mountPoint.Trim('/'), HttpMethod.Post, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<bool> GetInitStatusAsync()
        {
            var response = await _polymath.MakeVaultApiRequest<dynamic>("v1/sys/init", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response.initialized;
        }

        public async Task<MasterCredentials> InitAsync(InitOptions initOptions)
        {
            var response = await _polymath.MakeVaultApiRequest<MasterCredentials>("v1/sys/init", HttpMethod.Put, initOptions).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task SealAsync()
        {
            await _polymath.MakeVaultApiRequest("v1/sys/seal", HttpMethod.Put).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<SealStatus> GetSealStatusAsync()
        {
            var response = await _polymath.MakeVaultApiRequest<SealStatus>("v1/sys/seal-status", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task<SealStatus> UnsealAsync(string masterShareKey = null, bool resetCompletely = false)
        {
            var requestData = new
            {
                key = masterShareKey,
                reset = resetCompletely
            };

            var response = await _polymath.MakeVaultApiRequest<SealStatus>("v1/sys/unseal", HttpMethod.Put, requestData).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
            return response;
        }

        public async Task<SealStatus> QuickUnsealAsync(string[] allMasterShareKeys)
        {
            SealStatus finalStatus = null;

            foreach (var masterShareKey in allMasterShareKeys)
            {
                finalStatus = await UnsealAsync(masterShareKey);
            }

            return finalStatus;
        }

        public Task<string> HashWithAuditBackendAsync(string mountPoint, string inputToHash)
        {
            throw new NotImplementedException();
        }
    }
}
