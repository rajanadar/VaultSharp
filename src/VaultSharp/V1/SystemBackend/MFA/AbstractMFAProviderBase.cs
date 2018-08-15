using System.Net.Http;
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

        public async Task ConfigureAsync(TMFAConfig mfaConfig)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/mfa/method/" + Type + "/" + mfaConfig.Name, HttpMethod.Post, mfaConfig).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task<Secret<TMFAConfig>> GetConfigAsync(string methodName)
        {
            return await _polymath.MakeVaultApiRequest<Secret<TMFAConfig>>("v1/sys/mfa/method/" + Type + "/" + methodName, HttpMethod.Get).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }

        public async Task DeleteConfigAsync(string methodName)
        {
            await _polymath.MakeVaultApiRequest("v1/sys/mfa/method/" + Type + "/" + methodName, HttpMethod.Delete).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}