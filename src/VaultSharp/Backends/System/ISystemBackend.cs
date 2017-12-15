using System.Threading.Tasks;

namespace VaultSharp.Backends.System
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISystemBackend
    {
        /// <summary>
        /// Lists only the mounted audit backends (it does not list all available audit backends).
        /// </summary>
        /// <returns>
        /// The mounted audit backends.
        /// </returns>
        // Task<Secret<IEnumerable<AuditBackend>>> GetMountedAuditBackendsAsync();

        /// <summary>
        /// This endpoint hashes the given input data with the specified audit backend's hash function and salt. 
        /// This endpoint can be used to discover whether a given plaintext string (the input parameter) appears in the audit log in obfuscated form.
        /// The audit log records requests and responses.
        /// Since the Vault API is JSON-based, any binary data returned from an API call(such as a DER-format certificate) is base64-encoded by the Vault 
        /// server in the response.
        /// As a result such information should also be base64-encoded to supply into the input parameter.
        /// </summary>
        /// <param name="mountPoint"><para>[required]</para>
        /// The mount point for the audit backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <param name="inputToHash"><para>[required]</para>
        /// The input value to hash</param>
        /// <remarks>https://www.vaultproject.io/api/system/audit-hash.html</remarks>
        /// <returns>
        /// The hashed value.
        /// </returns>
        Task<string> HashWithAuditBackendAsync(string mountPoint, string inputToHash);

        /// <summary>
        /// Gets the seal status of the Vault.
        /// This is an unauthenticated call and does not need credentials.
        /// </summary>
        /// <returns>
        /// The seal status of the Vault.
        /// </returns>
        Task<SealStatus> GetSealStatusAsync();
    }
}
