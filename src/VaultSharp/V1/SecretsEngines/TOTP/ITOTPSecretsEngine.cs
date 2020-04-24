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

        /// <summary>
        /// Creates a new key definition where Vault acts as TOTP provider
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the key to create credentials against.
        /// </param>
        /// <param name="issuer"><para>[required]</para>
        /// Specifies the name of the key’s issuing organization.
        /// </param>
        /// <param name="accountName"><para>[required]</para>
        /// Specifies the name of the account associated with the key.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Nomad backend. Defaults to <see cref="SecretsEngineDefaultPaths.TOTP" />
        /// Provide a value only if you have customized the Nomad mount point.</param>
        /// <returns>
        /// The secret with the <see cref="TOTPProvider" /> as the data.
        /// </returns>
        Task<Secret<TOTPProvider>> CreateTOTPProviderKeyAsync(string keyName, string issuer, string accountName, string mountPoint = SecretsEngineDefaultPaths.TOTP);

        /// <summary>
        /// Deletes the key definition
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the key to create credentials against.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Nomad backend. Defaults to <see cref="SecretsEngineDefaultPaths.TOTP" />
        /// Provide a value only if you have customized the Nomad mount point.</param>
        /// <returns>A completed task when succesful</returns>
        Task DeleteKeyAsync(string keyName, string mountPoint = SecretsEngineDefaultPaths.TOTP);

        /// <summary>
        /// Returns a list of available keys. Only the key names are returned, not any values.
        /// </summary>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Generic backend. Defaults to <see cref="SecretsEngineDefaultPaths.TOTP" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>List of availble keys</returns>
        Task<Secret<ListInfo>> ListAllKeysAsync(string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null);

        /// <summary>
        /// Queries the key definition
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
        /// <returns>A secret with the key definition</returns>
        Task<Secret<TOTPKey>> ReadKeyAsync(string keyName, string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null);


    }
}