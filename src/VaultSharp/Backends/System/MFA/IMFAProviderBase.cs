using System.Threading.Tasks;
using VaultSharp.Core;

namespace VaultSharp.Backends.System.MFA
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