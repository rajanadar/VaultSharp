using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SystemBackend.MFA
{
    /// <summary>
    /// The base MFA provider for all types.
    /// </summary>
    internal abstract class AbstractMFAProviderBase<TMFAConfig> : IMFAProviderBase<TMFAConfig> where TMFAConfig : AbstractMFAConfig
    {
        private readonly Polymath _polymath;

        protected AbstractMFAProviderBase(Polymath polymath) => _polymath = polymath;

        /// <summary>
        /// Gets the type of MFA method.
        /// </summary>
        public abstract string Type { get; }

        public async Task ConfigureAsync(TMFAConfig mfaConfig, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/mfa/method/" + Type + "/" + mfaConfig.Name, HttpMethod.Post, mfaConfig, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TMFAConfig>> GetConfigAsync(string methodName, CancellationToken cancellationToken = default)
        {
            return await _polymath.MakeVaultApiRequest<Secret<TMFAConfig>>("v1/sys/mfa/method/" + Type + "/" + methodName, HttpMethod.Get, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteConfigAsync(string methodName, CancellationToken cancellationToken = default)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/mfa/method/" + Type + "/" + methodName, HttpMethod.Delete, cancellationToken: cancellationToken).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}