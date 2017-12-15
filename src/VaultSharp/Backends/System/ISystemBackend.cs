using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.Core;

namespace VaultSharp.Backends.System
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISystemBackend
    {
        /// <summary>
        /// Gets all the mounted audit backends (it does not list all available audit backends).
        /// </summary>
        /// <returns>
        /// The mounted audit backends.
        /// </returns>
        Task<Secret<IEnumerable<AbstractAuditBackend>>> GetAuditBackendsAsync();

        /// <summary>
        /// Gets the initialization status of Vault.
        /// This is an unauthenticated call and does not use the credentials.
        /// </summary>
        /// <returns>
        /// The initialization status of Vault.
        /// </returns>
        Task<bool> GetInitStatusAsync();

        /// <summary>
        /// Initializes a new Vault. The Vault must not have been previously initialized. 
        /// The recovery options, as well as the stored shares option, are only available when using Vault HSM.
        /// </summary>
        /// <param name="initOptions"><para>[required]</para>
        /// The initialization options.
        /// </param>
        /// <returns>
        /// An object including the (possibly encrypted, if pgp_keys was provided) master keys and initial root token.
        /// </returns>
        Task<MasterCredentials> InitAsync(InitOptions initOptions);


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
        /// Seals the Vault. In HA mode, only an active node can be sealed. 
        /// Standby nodes should be restarted to get the same effect. 
        /// Requires a token with root policy or sudo capability on the path.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        Task SealAsync();

        /// <summary>
        /// Gets the seal status of the Vault.
        /// This is an unauthenticated call and does not need credentials.
        /// </summary>
        /// <returns>
        /// The seal status of the Vault.
        /// </returns>
        Task<SealStatus> GetSealStatusAsync();

        /// <summary>
        /// Progresses the unsealing of the Vault.
        /// Enter a single master key share to progress the unsealing of the Vault.
        /// If the threshold number of master key shares is reached, Vault will attempt to unseal the Vault.
        /// Otherwise, this API must be called multiple times until that threshold is met.
        /// <para>
        /// Either the <see cref="masterShareKey" /> or <see cref="resetCompletely" /> parameter must be provided; 
        /// if both are provided, <see cref="resetCompletely" /> takes precedence.
        /// </para>
        /// This is an unauthenticated call and does not use the credentials.
        /// </summary>
        /// <param name="masterShareKey">A single master share key.</param>
        /// <param name="resetCompletely">When <value>true</value>, the previously-provided unseal keys are discarded from memory 
        /// and the unseal process is completely reset.
        /// Default value is <value>false</value>.
        /// If you make a call with the value as <value>true</value>, it doesn't matter if this call has a valid unused <see cref="masterShareKey" />. 
        /// It'll be ignored.</param>
        /// <returns>
        /// The seal status of the Vault.
        /// </returns>
        Task<SealStatus> UnsealAsync(string masterShareKey = null, bool resetCompletely = false);

        /// <summary>
        /// Unseals the Vault in a single call.
        /// Provide all the master keys together.
        /// </summary>
        /// <param name="allMasterShareKeys">All the master share keys.</param>
        /// <returns>The final Seal Status after all the share keys are applied.</returns>
        Task<SealStatus> QuickUnsealAsync(string[] allMasterShareKeys);
    }
}
