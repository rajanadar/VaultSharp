using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines;
using VaultSharp.V1.SystemBackend.Enterprise;
using VaultSharp.V1.SystemBackend.MFA;
using VaultSharp.V1.SystemBackend.Plugin;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// The system backend.
    /// </summary>
    public interface ISystemBackend
    {
        /// <summary>
        /// Gets the enterprise functionality provider.
        /// </summary>
        IEnterprise Enterprise { get; }

        /// <summary>
        /// Gets the MFA provider.
        /// </summary>
        IMFA MFA { get; }

        /// <summary>
        /// Gets the plugin provider.
        /// </summary>
        IPlugin Plugins { get; }

        /// <summary>
        /// Gets all the enabled audit backends (it does not list all available audit backends).
        /// This endpoint requires sudo capability in addition to any path-specific capabilities.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The mounted audit backends.
        /// </returns>
        Task<Secret<Dictionary<string, AbstractAuditBackend>>> GetAuditBackendsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// This endpoint enables a new audit device at the supplied path. 
        /// The path can be a single word name or a more complex, nested path.
        /// </summary>
        /// <param name="abstractAuditBackend">The audit backend.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MountAuditBackendAsync(AbstractAuditBackend abstractAuditBackend, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unmounts the audit backend at the given mount point.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The mount point for the audit backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task UnmountAuditBackendAsync(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Hash the given input data with the specified audit backend's hash function and salt.
        /// This endpoint can be used to discover whether a given plaintext string (the input parameter) appears in
        /// the audit log in obfuscated form.
        /// Note that the audit log records requests and responses; since the Vault API is JSON-based,
        /// any binary data returned from an API call (such as a DER-format certificate) is base64-encoded by
        /// the Vault server in the response, and as a result such information should also be base64-encoded
        /// to supply into the <see paramref="inputToHash" /> parameter.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The mount point for the audit backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <param name="inputToHash"><para>[required]</para>
        /// The input value to hash</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The hashed value.
        /// </returns>
        Task<Secret<AuditHash>> AuditHashAsync(string path, string inputToHash, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all the enabled authentication backends.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The enabled authentication backends.
        /// </returns>
        Task<Secret<Dictionary<string, AuthMethod>>> GetAuthBackendsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Mounts a new authentication backend.
        /// The auth backend can be accessed and configured via the auth path specified in the URL. 
        /// </summary>
        /// <param name="authBackend">The authentication backend.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MountAuthBackendAsync(AuthMethod authBackend, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unmounts the authentication backend at the given mount point.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The authentication path for the authentication backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task UnmountAuthBackendAsync(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the mounted authentication backend's configuration values.
        /// The lease values for each TTL may be the system default ("0" or "system") or a mount-specific value.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The authentication path for the authentication backend in which to tune. 
        /// (with or without trailing slashes. it doesn't matter)</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The mounted secret backend's configuration values.
        /// </returns>
        Task<Secret<BackendConfig>> GetAuthBackendConfigAsync(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tunes the mount configuration parameters for the given <see paramref="path" />.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The authentication path for the authentication backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <param name="backendConfig"><para>[required]</para>
        /// The mount configuration with the required setting values.
        /// Provide a value of <value>"0"</value> for the TTL settings if you want to use the system defaults.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task
        /// </returns>
        Task ConfigureAuthBackendAsync(string path, BackendConfig backendConfig, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the capabilities of the token on the given path.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// Path on which the token's capabilities will be checked.</param>
        /// <param name="token"><para>[required]</para>
        /// Token for which capabilities are being queried.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The list of capabilities.</returns>
        Task<Secret<TokenCapability>> GetTokenCapabilitiesAsync(string path, string token, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the capabilities of the token associated with the accessor on the given path.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// Path on which the token's capabilities will be checked.</param>
        /// <param name="tokenAccessor"><para>[required]</para>
        /// Accessor to the Token for which capabilities are being queried.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The list of capabilities.</returns>
        Task<Secret<TokenCapability>> GetTokenCapabilitiesByAcessorAsync(string path, string tokenAccessor, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the capabilities of the calling token.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// Path on which the token's capabilities will be checked.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The list of capabilities.</returns>
        Task<Secret<TokenCapability>> GetCallingTokenCapabilitiesAsync(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the request headers configured to be audited.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns></returns>
        Task<Secret<RequestHeaderSet>> GetAuditRequestHeadersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a particular request header.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>Header details.</returns>
        Task<Secret<RequestHeader>> GetAuditRequestHeaderAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates/updates the request header to be audited.
        /// </summary>
        /// <param name="name"><para>[required]</para>
        /// The name fo the header.
        /// </param>
        /// <param name="hmac"><para>[optional]</para>
        /// Specifies if this header's value should be HMAC'ed in the audit logs.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The task.</returns>
        Task PutAuditRequestHeaderAsync(string name, bool hmac = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a particular request header.
        /// </summary>
        /// <param name="name">The name of the header.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>Header details.</returns>
        Task DeleteAuditRequestHeaderAsync(string name, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the current CORS configuration.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>Config</returns>
        Task<Secret<CORSConfig>> GetCORSConfigAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Configures CORS.
        /// </summary>
        /// <param name="corsConfig">
        /// The CORS Configuration.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>Task</returns>
        Task ConfigureCORSAsync(CORSConfig corsConfig, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the current CORS configuration.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>Task</returns>
        Task DeleteCORSConfigAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the configuration and progress of the current root generation attempt.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The root status</returns>
        Task<RootTokenGenerationStatus> GetRootTokenGenerationStatusAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Initializes a new root generation attempt. 
        /// Only a single root generation attempt can take place at a time. 
        /// One (and only one) of <see paramref="base64EncodedOneTimePassword"/> or <see paramref="pgpKey"/> are required.
        /// </summary>
        /// <param name="base64EncodedOneTimePassword"><para>[optional]</para>
        /// A base64-encoded 16-byte value. The raw bytes of the token will be XOR'd with this 
        /// value before being returned to the final unseal key provider.</param>
        /// <param name="pgpKey"><para>[optional]</para>
        /// A base64-encoded PGP public key. The raw bytes of the token will be encrypted with this value before being 
        /// returned to the final unseal key provider.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The root token generation status.</returns>
        Task<RootTokenGenerationStatus> InitiateRootTokenGenerationAsync(string base64EncodedOneTimePassword, string pgpKey, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels any in-progress root generation attempt. 
        /// This clears any progress made. 
        /// This must be called to change the OTP or PGP key being used.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The task.</returns>
        Task CancelRootTokenGenerationAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Continues the root generation process.
        /// Enter a single master key share to progress the root generation attempt. 
        /// If the threshold number of master key shares is reached, 
        /// Vault will complete the root generation and issue the new token. 
        /// Otherwise, this API must be called multiple times until that threshold is met. 
        /// The attempt nonce must be provided with each call.
        /// </summary>
        /// <param name="masterShareKey"><para>[required]</para>
        /// A single master share key.</param>
        /// <param name="nonce"><para>[required]</para>
        /// The nonce of the root generation attempt.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// An object indicating the attempt nonce, and completion status, 
        /// and the encoded root token, if the attempt is complete.
        /// </returns>
        Task<RootTokenGenerationStatus> ContinueRootTokenGenerationAsync(string masterShareKey, string nonce, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates a root token in a single call. 
        /// Call this after calling the <see cref="InitiateRootTokenGenerationAsync"/> method.
        /// Provide all the master keys together.
        /// </summary>
        /// <param name="thresholdMasterShareKeys">Threshold master share keys.</param>
        /// <param name="nonce"><para>[required]</para>
        /// The nonce of the root generation attempt.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The final Status after all the share keys are applied.</returns>
        Task<RootTokenGenerationStatus> QuickRootTokenGenerationAsync(string[] thresholdMasterShareKeys, string nonce, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the health status of Vault. This provides a simple way to monitor the health of a Vault instance.
        /// This is an unauthenticated call and does not use the credentials.
        /// </summary>
        /// <param name="standbyOk"><para>[optional]</para>
        /// A flag to indicate that being a standby should still return the active status code 
        /// instead of the standby status code of HTTP 429 (or whatever is provided as standbyStatusCode)
        /// DEFAULTs to <value>false</value>, meaning a standby code will be returned.
        /// This is useful when Vault is behind a non-configurable load balance that just wants a 200-level response.
        /// </param>
        /// <param name="activeStatusCode"><para>[optional]</para>
        /// A user defined status code provided to indicate the status code that should be returned 
        /// for an active node instead of the default successful response of HTTP 200.
        /// DEFAULTs to <value>200</value>, meaning the default HTTP 200 Status code will be returned.
        /// </param>
        /// <param name="standbyStatusCode"><para>[optional]</para>
        /// A user defined status code provided to indicate the status code that should be returned 
        /// for an standby node instead of the default error response of HTTP 429.
        /// DEFAULTs to <value>429</value>, meaning the default HTTP 429 Status code will be returned.
        /// </param>
        /// <param name="sealedStatusCode"><para>[optional]</para>
        /// A user defined status code provided to indicate the status code that should be returned 
        /// for an sealed node instead of the default error response of HTTP 503.
        /// DEFAULTs to <value>503</value>, meaning the default HTTP 503 Status code will be returned.
        /// </param>
        /// <param name="uninitializedStatusCode"><para>[optional]</para>
        /// A user defined status code provided to indicate the status code that should be returned 
        /// for an uninitialized vault node instead of the default error response of HTTP 501.
        /// DEFAULTs to <value>501</value>, meaning the default HTTP 501 Status code will be returned.
        /// </param>
        /// <param name="queryHttpMethod"><para>[optional]</para>
        /// The <see cref="HttpMethod"/> to be used to query vault. By default <see cref="HttpMethod.Get"/> will be used.
        /// You can change it to <see cref="HttpMethod.Head"/>.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The health status.
        /// </returns>
        Task<HealthStatus> GetHealthStatusAsync(bool standbyOk = false, int activeStatusCode = (int)HttpStatusCode.OK, int standbyStatusCode = 429, int sealedStatusCode = (int)HttpStatusCode.ServiceUnavailable, int uninitializedStatusCode = (int)HttpStatusCode.NotImplemented, HttpMethod queryHttpMethod = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the initialization status of Vault.
        /// This is an unauthenticated call and does not use the credentials.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The initialization status of Vault.
        /// </returns>
        Task<bool> GetInitStatusAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Initializes a new Vault. The Vault must not have been previously initialized. 
        /// The recovery options, as well as the stored shares option, are only available when using Vault HSM.
        /// </summary>
        /// <param name="initOptions"><para>[required]</para>
        /// The initialization options.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// An object including the (possibly encrypted, if pgp_keys was provided) master keys and initial root token.
        /// </returns>
        Task<MasterCredentials> InitAsync(InitOptions initOptions, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets information about the current encryption key used by Vault
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The status of the encryption key.
        /// </returns>
        Task<Secret<EncryptionKeyStatus>> GetKeyStatusAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the high availability status and current leader instance of Vault.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The leader info.
        /// </returns>
        Task<Leader> GetLeaderAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the lease metadata.
        /// </summary>
        /// <param name="leaseId"><para>[required]</para>
        /// The lease id.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>Info.</returns>
        Task<Secret<Lease>> GetLeaseAsync(string leaseId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the list of lease ids.
        /// </summary>
        /// <param name="prefix"><para>[required]</para>
        /// The prefix for the leases.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The lease ids.</returns>
        Task<Secret<ListInfo>> GetAllLeasesAsync(string prefix, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the lease metadata.
        /// </summary>
        /// <param name="leaseId"><para>[required]</para>
        /// The ID of the lease to extend.
        /// </param>
        /// <param name="incrementSeconds"><para>[required]</para>
        /// Specifies the requested amount of time (in seconds) to extend the lease.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>Info.</returns>
        Task<Secret<RenewedLease>> RenewLeaseAsync(string leaseId, int incrementSeconds, CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes a lease immediately.
        /// </summary>
        /// <param name="leaseId"><para>[required]</para>
        /// The lease id.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>Task.</returns>
        Task RevokeLeaseAsync(string leaseId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes all secrets or tokens generated under a given prefix immediately. 
        /// Unlike <see cref="RevokePrefixLeaseAsync"/>, this path ignores backend errors encountered during revocation. 
        /// This is potentially very dangerous and should only be used in specific emergency situations where 
        /// errors in the backend or the connected backend service prevent normal revocation. 
        /// By ignoring these errors, Vault abdicates responsibility for ensuring that the issued 
        /// credentials or secrets are properly revoked and/or cleaned up.
        /// Access to this endpoint should be tightly controlled.
        /// </summary>
        /// <param name="prefix"><para>[required]</para>
        /// Specifies the prefix to revoke.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>Task.</returns>
        Task ForceRevokeLeaseAsync(string prefix, CancellationToken cancellationToken = default);

        /// <summary>
        /// Revokes revokes all secrets (via a lease ID prefix) or tokens (via the tokens' path property) generated under a given prefix immediately. 
        /// This requires sudo capability and access to it should be tightly controlled as it can be used 
        /// to revoke very large numbers of secrets/tokens at once.
        /// </summary>
        /// <param name="prefix"><para>[required]</para>
        /// Specifies the prefix to revoke.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>Task.</returns>
        Task RevokePrefixLeaseAsync(string prefix, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the verbosity level of all loggers.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The verbosity levels</returns>
        Task<Secret<Dictionary<string, object>>> GetVerbosityLevelOfAllLoggersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the verbosity level of a logger.
        /// </summary>
        /// <param name="loggerName">
        /// Specifies the logger name.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The verbosity level</returns>
        Task<Secret<Dictionary<string, object>>> GetVerbosityLevelOfLoggerAsync(string loggerName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Modify the verbosity level of all loggers.
        /// </summary>
        /// <param name="logVerbosityLevel">
        /// Specifies the log verbosity level to be set for all loggers.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        Task ModifyVerbosityLevelForAllLoggersAsync(LogVerbosityLevel logVerbosityLevel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Modify the verbosity level of a logger.
        /// </summary>
        /// <param name="loggerName">
        /// Specifies the logger to be modified.
        /// </param>
        /// <param name="logVerbosityLevel">
        /// Specifies the log verbosity level to be set for all loggers.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        Task ModifyVerbosityLevelForLoggerAsync(string loggerName, LogVerbosityLevel logVerbosityLevel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reverts the verbosity level of all loggers.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        Task RevertVerbosityLevelForAllLoggersAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Revert the verbosity level of a logger
        /// </summary>
        /// <param name="loggerName">
        /// Specifies the logger to be modified.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        Task RevertVerbosityLevelForLoggerAsync(string loggerName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a specific mounted secret backend.
        /// </summary>
        /// <param name="mountPoint">The secret backend.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The mounted secret backend.
        /// </returns>
        Task<Secret<SecretsEngine>> GetSecretBackendAsync(string mountPoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all the mounted secret backends.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The mounted secret backends.
        /// </returns>
        Task<Secret<Dictionary<string, SecretsEngine>>> GetSecretBackendsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Mounts the new secret backend to the mount point in the URL.
        /// </summary>
        /// <param name="secretBackend">The secret backend.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task
        /// </returns>
        Task MountSecretBackendAsync(SecretsEngine secretBackend, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unmounts the mount point specified in the URL.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The mount point for the secret backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task
        /// </returns>
        Task UnmountSecretBackendAsync(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the mounted secret backend's configuration values.
        /// Unlike the <see cref="GetSecretBackendsAsync"/> method, 
        /// this will return the current time in seconds for each TTL, 
        /// which may be the system default or a mount-specific value.
        /// </summary>
        /// <param name="mountPoint"><para>[required]</para>
        /// The mount point for the secret backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The mounted secret backend's configuration values.
        /// </returns>
        Task<Secret<BackendConfig>> GetSecretBackendConfigAsync(string mountPoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Tunes the mount configuration parameters for the given <see paramref="path" />.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The mount point for the secret backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <param name="backendConfig"><para>[required]</para>
        /// The mount configuration with the required setting values.
        /// Provide a value of <value>0</value> for the TTL settings 
        /// if you want to use the system defaults.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task
        /// </returns>
        Task ConfigureSecretBackendAsync(string path, BackendConfig backendConfig, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all the available policy names in the system.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The policy names.
        /// </returns>
        Task<Secret<ListInfo>> GetPoliciesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the rules for the named policy.
        /// </summary>
        /// <param name="policyName">
        /// <para>[required]</para>
        /// The name of the policy.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The rules for the policy.
        /// </returns>
        Task<Secret<Policy>> GetPolicyAsync(string policyName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds or updates the policy.
        /// Once a policy is updated, it takes effect immediately to all associated users.
        /// </summary>
        /// <param name="policy"><para>[required]</para>
        /// The policy to be added or updated.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task WritePolicyAsync(Policy policy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the named policy. This will immediately affect all associated users.
        /// </summary>
        /// <param name="policyName"><para>[required]</para>
        /// The name of the policy.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeletePolicyAsync(string policyName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all the ACL policy names in the system.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The policy names.
        /// </returns>
        Task<Secret<ListInfo>> GetACLPoliciesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the rules for the named ACL policy.
        /// </summary>
        /// <param name="policyName">
        /// <para>[required]</para>
        /// The name of the policy.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The rules for the policy.
        /// </returns>
        Task<Secret<ACLPolicy>> GetACLPolicyAsync(string policyName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds or updates the ACL policy.
        /// Once a policy is updated, it takes effect immediately to all associated users.
        /// </summary>
        /// <param name="policy"><para>[required]</para>
        /// The policy to be added or updated.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task WriteACLPolicyAsync(ACLPolicy policy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the named ACL policy. This will immediately affect all associated users.
        /// </summary>
        /// <param name="policyName"><para>[required]</para>
        /// The name of the policy.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeleteACLPolicyAsync(string policyName, CancellationToken cancellationToken = default);

        /// <summary>
        /// This endpoint generates a password from the specified existing password policy.
        /// </summary>
        /// <param name="passwordPolicyName">
        /// <para>[required]</para>
        /// Specifies the name of the password policy to generate a password from. 
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The generated password.</returns>
        Task<Secret<string>> GeneratePasswordFromPasswordPolicyAsync(string passwordPolicyName, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Returns a list keys for a given path prefix.
        /// </summary>
        /// <param name="storagePathPrefix"><para>[required]</para>
        /// Raw path in the storage backend and not the logical path that is exposed via the mount system.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>Keys.</returns>
        Task<Secret<ListInfo>> GetRawSecretKeysAsync(string storagePathPrefix, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reads the value of the key at the given path.
        /// This is the raw path in the sorage backend and not the logical path that is exposed via the mount system.
        /// </summary>
        /// <param name="storagePath"><para>[required]</para>
        /// Raw path in the storage backend and not the logical path that is exposed via the mount system.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The SecretsEngine with raw data.
        /// </returns>
        Task<Secret<Dictionary<string, object>>> ReadRawSecretAsync(string storagePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update the value of the key at the given path.
        /// This is the raw path in the storage backend and not the logical path that is exposed via the mount system.
        /// </summary>
        /// <param name="storagePath"><para>[required]</para>
        /// Raw path in the storage backend and not the logical path that is exposed via the mount system.</param>
        /// <param name="values"><para>[required]</para>
        /// The values to write. The dictionary will be JSONized as a raw value string before being written.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task WriteRawSecretAsync(string storagePath, Dictionary<string, object> values, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete the key with given path.
        /// This is the raw path in the storage backend and not the logical path that is exposed via the mount system.
        /// </summary>
        /// <param name="storagePath"><para>[required]</para>
        /// Raw path in the storage backend and not the logical path that is exposed via the mount system.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeleteRawSecretAsync(string storagePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the configuration and progress of the current rekey attempt.
        /// Information about the new shares to generate and the threshold required for the new shares,
        /// the number of unseal keys provided for this rekey and the required number of unseal keys is returned.
        /// This is an unauthenticated call and does not need credentials.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The rekey status.
        /// </returns>
        Task<RekeyStatus> GetRekeyStatusAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Initiates a new rekey attempt.
        /// Only a single rekey attempt can take place at a time, and changing the parameters of a rekey requires canceling and starting a new rekey.
        /// This is an unauthenticated call and does not need credentials.
        /// </summary>
        /// <param name="secretShares"><para>[required]</para>
        /// The number of shares to split the master key into.</param>
        /// <param name="secretThreshold"><para>[required]</para>
        /// The number of shares required to reconstruct the master key.
        /// This must be less than or equal to <see paramref="secretShares" />.</param>
        /// <param name="pgpKeys"><para>[optional]</para>
        /// An array of PGP public keys used to encrypt the output unseal keys.
        /// Ordering is preserved.
        /// The keys must be base64-encoded from their original binary representation.
        /// The size of this array must be the same as <see paramref="secretShares" />.</param>
        /// <param name="backup"><para>[optional]</para>
        /// If using PGP-encrypted keys, whether Vault should also back them up to a well-known 
        /// location in physical storage. These can then be retrieved and removed 
        /// via the GetRekeyBackupAsync endpoint. Makes sense only when pgp keys are provided.
        /// Defaults to 'false', meaning no backup.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The rekey status.
        /// </returns>
        Task<RekeyStatus> InitiateRekeyAsync(int secretShares, int secretThreshold, string[] pgpKeys = null, bool backup = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Cancels any in-progress rekey. This clears the rekey settings as well as any progress made.
        /// This must be called to change the parameters of the rekey.
        /// This is an unauthenticated call and does not need credentials.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task CancelRekeyAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the the backup copy of PGP-encrypted unseal keys. 
        /// The returned value is the nonce of the rekey operation and a map of PGP key 
        /// fingerprint to hex-encoded PGP-encrypted key.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The rekey backup info.</returns>
        Task<Secret<RekeyBackupInfo>> GetRekeyBackupKeysAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the backup copy of PGP-encrypted unseal keys.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The task.</returns>
        Task DeleteRekeyBackupKeysAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Continues the rekey process. Enter a single master key share to progress the rekey of the Vault.
        /// If the threshold number of master key shares is reached, Vault will complete the rekey.
        /// Otherwise, this API must be called multiple times until that threshold is met.
        /// This is an unauthenticated call and does not need credentials.
        /// </summary>
        /// <param name="masterShareKey"><para>[required]</para>
        /// A single master share key.</param>
        /// <param name="rekeyNonce"><para>[required]</para>
        /// The nonce of the rekey operation.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// An object indicating the rekey operation nonce and completion status; 
        /// if completed, the new master keys are returned. 
        /// If the keys are PGP-encrypted, an array of key fingerprints will also be provided 
        /// (with the order in which the keys were used for encryption) along with whether 
        /// or not the keys were backed up to physical storage.
        /// </returns>
        /// <remarks>
        /// This method returns <see cref="RekeyStatus" /> till the rekey is complete.
        /// Then it returns <see cref="RekeyProgress" /> type, when the rekey is complete.
        /// Unfortunately, in C# we can return only one type, and it is not worth all the inheritance/jugaad.
        /// So for most-valued simplicity, the method returns the final data type. [potential future raja todo]
        /// </remarks>
        Task<RekeyProgress> ContinueRekeyAsync(string masterShareKey, string rekeyNonce, CancellationToken cancellationToken = default);

        /// <summary>
        /// Rekeys the Vault in a single call.
        /// Call this after calling the <see cref="InitiateRekeyAsync"/> method.
        /// Provide all the master keys together.
        /// This is an unauthenticated call and does not need credentials.
        /// </summary>
        /// <param name="thresholdMasterShareKeys">Threshold master share keys.</param>
        /// <param name="rekeyNonce"><para>[required]</para>
        /// The nonce of the rekey operation.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The final Rekey progress after all the share keys are applied.</returns>
        Task<RekeyProgress> QuickRekeyAsync(string[] thresholdMasterShareKeys, string rekeyNonce, CancellationToken cancellationToken = default);

        /// <summary>
        /// Seals the Vault. In HA mode, only an active node can be sealed. 
        /// Standby nodes should be restarted to get the same effect. 
        /// Requires a token with root policy or sudo capability on the path.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task SealAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the seal status of the Vault.
        /// This is an unauthenticated call and does not need credentials.
        /// </summary>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The seal status of the Vault.
        /// </returns>
        Task<SealStatus> GetSealStatusAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Progresses the unsealing of the Vault.
        /// Enter a single master key share to progress the unsealing of the Vault.
        /// If the threshold number of master key shares is reached, Vault will attempt to unseal the Vault.
        /// Otherwise, this API must be called multiple times until that threshold is met.
        /// <para>
        /// Either the <see paramref="masterShareKey" /> or <see paramref="resetCompletely" /> parameter must be provided; 
        /// if both are provided, <see paramref="resetCompletely" /> takes precedence.
        /// </para>
        /// This is an unauthenticated call and does not use the credentials.
        /// </summary>
        /// <param name="masterShareKey">A single master share key.</param>
        /// <param name="resetCompletely">When <value>true</value>, the previously-provided unseal keys are discarded from memory 
        /// and the unseal process is completely reset.
        /// Default value is <value>false</value>.
        /// If you make a call with the value as <value>true</value>, it doesn't matter if this call has a valid unused <see paramref="masterShareKey" />. 
        /// It'll be ignored.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>
        /// The seal status of the Vault.
        /// </returns>
        Task<SealStatus> UnsealAsync(string masterShareKey = null, bool resetCompletely = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Unseals the Vault in a single call.
        /// Provide all the master keys together.
        /// </summary>
        /// <param name="thresholdMasterShareKeys">Threshold Master share keys.</param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The final Seal Status after all the share keys are applied.</returns>
        Task<SealStatus> QuickUnsealAsync(string[] thresholdMasterShareKeys, CancellationToken cancellationToken = default);

        /// <summary>
        /// Wraps the given user-supplied data inside a response-wrapped token.
        /// </summary>
        /// <param name="data">
        /// <para>[required]</para>
        /// The user supplied data.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The wrapped response.</returns>
        Task<Secret<object>> WrapResponseDataAsync(Dictionary<string, object> data, string wrapTimeToLive, CancellationToken cancellationToken = default);

        /// <summary>
        /// Looks up wrapping properties for the given token.
        /// </summary>
        /// <param name="tokenId">
        /// <para>[required]</para>
        /// The wrapping token identifier.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The token wrap info.</returns>
        Task<Secret<TokenWrapData>> LookupTokenWrapInfoAsync(string tokenId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Rewraps a response-wrapped token; the new token will use the same creation TTL as 
        /// the original token and contain the same response. 
        /// The old token will be invalidated. 
        /// This can be used for long-term storage of a secret in a response-wrapped 
        /// token when rotation is a requirement.
        /// </summary>
        /// <param name="tokenId">
        /// <para>[required]</para>
        /// The wrapping token identifier.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The secret with re-wrapped info.</returns>
        Task<Secret<object>> RewrapWrappedResponseDataAsync(string tokenId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the original response inside the given wrapping token. 
        /// This endpoint provides additional validation checks on the token, 
        /// returns the original value on the wire and ensures that the response is properly audit-logged.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="tokenId">
        /// <para>[required]</para>
        /// The wrapping token identifier.
        /// </param>
        /// <param name="cancellationToken"><para>[optional]</para>The token to monitor for cancellation requests.</param>
        /// <returns>The unwrapped original data.</returns>
        Task<Secret<TData>> UnwrapWrappedResponseDataAsync<TData>(string tokenId, CancellationToken cancellationToken = default);
    }
}