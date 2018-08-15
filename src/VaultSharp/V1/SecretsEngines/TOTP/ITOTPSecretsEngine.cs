using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.TOTP
{
    /// <summary>
    /// The TOTP Secrets Engine.
    /// </summary>
    public interface ITOTPSecretsEngine
    {
        /// <summary>
        /// Generates a new time-based one-time use password based on the named key.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the key to create credentials against.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Nomad backend. Defaults to <see cref="SecretsEngineDefaultPaths.TOTP" />
        /// Provide a value only if you have customized the Nomad mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="TOTPCode" /> as the data.
        /// </returns>
        Task<Secret<TOTPCode>> GetCodeAsync(string keyName, string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null);

        /// <summary>
        /// Generates a new time-based one-time use password based on the named key.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the key to create credentials against.
        /// </param>
        /// <param name="code"><para>[required]</para>
        /// Specifies the the password you want to validate.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Nomad backend. Defaults to <see cref="SecretsEngineDefaultPaths.TOTP" />
        /// Provide a value only if you have customized the Nomad mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="TOTPCode" /> as the data.
        /// </returns>
        Task<Secret<TOTPCodeValidity>> ValidateCodeAsync(string keyName, string code, string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null);
    }
}