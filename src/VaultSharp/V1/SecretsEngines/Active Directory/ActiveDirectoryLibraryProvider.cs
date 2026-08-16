using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.ActiveDirectory.Models;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory
{
    internal class ActiveDirectoryLibraryProvider : IActiveDirectoryLibrary
    {
        private readonly Polymath _polymath;

        public ActiveDirectoryLibraryProvider(Polymath polymath)
        {
            _polymath = polymath;
        }
        public async Task WriteServiceAccountSetAsync(string setName, CreateServiceAccountSetModel createServiceAccountSetModel, string mountPoint = null)
        {
            Checker.NotNull(setName, "setName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/library/" + setName, HttpMethod.Post, requestData: createServiceAccountSetModel).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ServiceAccountSetModel>> ReadServiceAccountSetAsync(string setName, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(setName, "setName");

            return await _polymath.MakeVaultApiRequest<Secret<ServiceAccountSetModel>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/library/" + setName, HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllServiceAccountSetsAsync(string mountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/library", _polymath.ListHttpMethod, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteServiceAccountSetAsync(string setName, string mountPoint = null)
        {
            Checker.NotNull(setName, "setName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/library/" + setName, HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CheckedOutCredentials>> CheckoutCredentialsAsync(string setName, long? timeToLive = null, string mountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<CheckedOutCredentials>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/library/" + setName + "/check-out", HttpMethod.Post, requestData: new TtlRequest { Ttl = timeToLive }, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CheckedInAccounts>> CheckinCredentialsAsync(string setName, List<string> serviceAccountNames = null, string mountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<CheckedInAccounts>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/library/" + setName + "/check-in", HttpMethod.Post, requestData: new ServiceAccountNamesRequest { ServiceAccountNames = serviceAccountNames }, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<CheckedInAccounts>> ForceCheckinCredentialsAsync(string setName, List<string> serviceAccountNames = null, string mountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<CheckedInAccounts>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/library/manage/" + setName + "/check-in", HttpMethod.Post, requestData: new ServiceAccountNamesRequest { ServiceAccountNames = serviceAccountNames }, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}