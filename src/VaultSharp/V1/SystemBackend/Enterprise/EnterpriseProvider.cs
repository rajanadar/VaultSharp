using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SystemBackend.Enterprise
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

        public async Task<Secret<ControlGroup>> GetControlGroupConfigAsync(CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ControlGroup>>("v1/sys/config/control-group", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task ConfigureControlGroupAsync(string maxTimeToLive, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/config/control-group", HttpMethod.Put, new MaxTtlRequest { MaxTtl = maxTimeToLive }, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteControlGroupConfigAsync(CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/config/control-group", HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ControlGroupRequestStatus>> AuthorizeControlGroupAsync(string accessor, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ControlGroupRequestStatus>>("v1/sys/control-group/authorize", HttpMethod.Post, new AccessorRequest { Accessor = accessor }, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ControlGroupRequestStatus>> CheckControlGroupStatusAsync(string accessor, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ControlGroupRequestStatus>>("v1/sys/control-group/request", HttpMethod.Post, new AccessorRequest { Accessor = accessor }, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<License>> GetLicenseAsync(CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<License>>("v1/sys/license", HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task InstallLicenseAsync(string licenseText, CancellationToken cancellationToken = default)
        {
            var requestData = new LicenseTextRequest
            {
                Text = licenseText
            };

            await _polymath.MakeVaultApiRequest<Secret<License>>("v1/sys/license", HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> GetRGPPoliciesAsync(CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/sys/policies/rgp", _polymath.ListHttpMethod, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<RGPPolicy>> GetRGPPolicyAsync(string policyName, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<RGPPolicy>>("v1/sys/policies/rgp/" + policyName, HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteRGPPolicyAsync(RGPPolicy policy, CancellationToken cancellationToken = default)
        {
            var requestData = new PolicyTextRequest
            {
                Policy = policy.Policy
            };

            await _polymath.MakeVaultApiRequest("v1/sys/policies/rgp/" + policy.Name, HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteRGPPolicyAsync(string policyName, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/policies/rgp/" + policyName, HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<ListInfo>> GetEGPPoliciesAsync(CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<ListInfo>>("v1/sys/policies/egp", _polymath.ListHttpMethod, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<EGPPolicy>> GetEGPPolicyAsync(string policyName, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<EGPPolicy>>("v1/sys/policies/egp/" + policyName, HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task WriteEGPPolicyAsync(EGPPolicy policy, CancellationToken cancellationToken = default)
        {
            var requestData = new PolicyTextRequest
            {
                Policy = policy.Policy
            };

            await _polymath.MakeVaultApiRequest("v1/sys/policies/egp/" + policy.Name, HttpMethod.Put, requestData, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteEGPPolicyAsync(string policyName, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/policies/egp/" + policyName, HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}