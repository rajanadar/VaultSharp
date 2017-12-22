using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;

namespace VaultSharp.Backends.System
{
    /// <summary>
    /// Enterprise System backend APIs
    /// </summary>
    internal class EnterpriseProvider : IEnterprise
    {
        private readonly Polymath _polymath;

        public EnterpriseProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<ControlGroup>> GetControlGroupConfigAsync()
        {
            return await _polymath.MakeVaultApiRequest<Secret<ControlGroup>>("v1/sys/config/control-group", HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ConfigureControlGroupAsync(string maxTimeToLive)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/config/control-group", HttpMethod.Put, new { max_ttl = maxTimeToLive }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteControlGroupConfigAsync()
        {
            await _polymath.MakeVaultApiRequest("v1/sys/config/control-group", HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ControlGroupRequestStatus>> AuthorizeControlGroupAsync(string accessor)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ControlGroupRequestStatus>>("v1/sys/control-group/authorize", HttpMethod.Post, new { accessor = accessor }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ControlGroupRequestStatus>> CheckControlGroupStatusAsync(string accessor)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ControlGroupRequestStatus>>("v1/sys/control-group/request", HttpMethod.Post, new { accessor = accessor }).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}