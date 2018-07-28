using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SystemBackend.MFA
{
    /// <summary>
    /// The MFA interface for operations.
    /// </summary>
    public interface IMFAProviderBase<TMFAConfig> where TMFAConfig : AbstractMFAConfig
    {
        Task ConfigureAsync(TMFAConfig mfaConfig);

        Task<Secret<TMFAConfig>> GetConfigAsync(string methodName);

        Task DeleteConfigAsync(string methodName);
    }
}