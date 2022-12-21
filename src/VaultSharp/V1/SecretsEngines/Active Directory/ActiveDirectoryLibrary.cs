using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines.ActiveDirectory.Models;

namespace VaultSharp.V1.SecretsEngines.ActiveDirectory
{
    internal class ActiveDirectoryLibrary : IActiveDirectoryLibrary
    {
        private readonly Polymath _polymath;

        public ActiveDirectoryLibrary(Polymath polymath)
        {
            _polymath = polymath;
        }
        public async Task WriteServiceAccountSetAsync(string setName, CreateServiceAccountSetModel createServiceAccountSetModel, string mountPoint = null)
        {
            Checker.NotNull(setName, "setName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/library/" + setName.Trim('/'), HttpMethod.Post, requestData: createServiceAccountSetModel).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ServiceAccountSetModel>> ReadServiceAccountSetAsync(string setName, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(setName, "setName");

            return await _polymath.MakeVaultApiRequest<Secret<ServiceAccountSetModel>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/library/" + setName.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> ReadAllServiceAccountSetsAsync(string mountPoint = null, string wrapTimeToLive = null)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/library", _polymath.ListHttpMethod, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteServiceAccountSetAsync(string setName, string mountPoint = null)
        {
            Checker.NotNull(setName, "setName");

            await _polymath.MakeVaultApiRequest(mountPoint ?? _polymath.VaultClientSettings.SecretsEngineMountPoints.ActiveDirectory, "/library/" + setName.Trim('/'), HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}