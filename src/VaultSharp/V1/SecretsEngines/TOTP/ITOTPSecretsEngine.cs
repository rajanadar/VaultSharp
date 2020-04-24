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
        /// The mount point for the TOTP backend. Defaults to <see cref="SecretsEngineDefaultPaths.TOTP" />
        /// Provide a value only if you have customized the TOTP mount point.</param>
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
        /// The mount point for the TOTP backend. Defaults to <see cref="SecretsEngineDefaultPaths.TOTP" />
        /// Provide a value only if you have customized the TOTP mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="TOTPCode" /> as the data.
        /// </returns>
        Task<Secret<TOTPCodeValidity>> ValidateCodeAsync(string keyName, string code, string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint creates or updates a key definition.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// The name of the key.
        /// </param>
        /// <param name="createKeyRequest"><para>[required]</para>
        /// The create key options.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the TOTP backend. Defaults to <see cref="SecretsEngineDefaultPaths.TOTP" />
        /// Provide a value only if you have customized the TOTP mount point.</param>
        /// <returns>The barcode and url of the key.</returns>
        Task<Secret<TOTPCreateKeyResponse>> CreateKeyAsync(string keyName, TOTPCreateKeyRequest createKeyRequest, string mountPoint = SecretsEngineDefaultPaths.TOTP);

        /// <summary>
        /// Retrieves a TOTP key.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// The name of the key to retrieve.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the TOTP backend. Defaults to <see cref="SecretsEngineDefaultPaths.TOTP" />
        /// Provide a value only if you have customized the TOTP mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>Key Info.</returns>
        Task<Secret<TOTPKey>> ReadKeyAsync(string keyName, string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null);

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
        /// <returns>List of available keys</returns>
        Task<Secret<ListInfo>> ReadAllKeysAsync(string mountPoint = SecretsEngineDefaultPaths.TOTP, string wrapTimeToLive = null);

        /// <summary>
        /// Deletes a TOTP key.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// The name of the key to delete.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the TOTP backend. Defaults to <see cref="SecretsEngineDefaultPaths.TOTP" />
        /// Provide a value only if you have customized the TOTP mount point.</param>
        Task DeleteKeyAsync(string keyName, string mountPoint = SecretsEngineDefaultPaths.TOTP);
    }
}