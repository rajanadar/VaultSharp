using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Backends.Audit.Models;
using VaultSharp.Backends.Authentication.Models;
using VaultSharp.Backends.Authentication.Models.AwsEc2;
using VaultSharp.Backends.Authentication.Models.Token;
using VaultSharp.Backends.Secret.Models;
using VaultSharp.Backends.Secret.Models.AWS;
using VaultSharp.Backends.Secret.Models.Cassandra;
using VaultSharp.Backends.Secret.Models.Consul;
using VaultSharp.Backends.Secret.Models.MicrosoftSql;
using VaultSharp.Backends.Secret.Models.MongoDb;
using VaultSharp.Backends.Secret.Models.MySql;
using VaultSharp.Backends.Secret.Models.PKI;
using VaultSharp.Backends.Secret.Models.PostgreSql;
using VaultSharp.Backends.Secret.Models.RabbitMQ;
using VaultSharp.Backends.Secret.Models.SSH;
using VaultSharp.Backends.Secret.Models.Transit;
using VaultSharp.Backends.System.Models;

namespace VaultSharp
{
    /// <summary>
    /// Provides an interface to interact with Vault as a client.
    /// This is the only entry point for consuming the Vault Client.
    /// </summary>
    public interface IVaultClient
    {
        /// <summary>
        /// Gets the initialization status of Vault.
        /// This is an unauthenticated call and does not use the credentials.
        /// </summary>
        /// <returns>
        /// The initialization status of Vault.
        /// </returns>
        Task<bool> GetInitializationStatusAsync();

        /// <summary>
        /// Initializes a new Vault. The Vault must have not been previously initialized.
        /// This is an unauthenticated call and does not use the credentials.
        /// </summary>
        /// <param name="initializeOptions"><para>[required]</para>
        /// The initialization options.
        /// </param>
        /// <returns>
        /// An object including the (possibly encrypted, if pgp_keys was provided) master keys and initial root token.
        /// </returns>
        Task<MasterCredentials> InitializeAsync(InitializeOptions initializeOptions);

        /// <summary>
        /// Gets the configuration and progress of the current root generation attempt.
        /// </summary>
        /// <returns>The root status</returns>
        Task<RootTokenGenerationStatus> GetRootTokenGenerationStatusAsync();

        /// <summary>
        /// Initializes a new root generation attempt. 
        /// Only a single root generation attempt can take place at a time. 
        /// One (and only one) of <see cref="base64EncodedOneTimePassword"/> or <see cref="pgpKey"/> are required.
        /// </summary>
        /// <param name="base64EncodedOneTimePassword"><para>[optional]</para>
        /// A base64-encoded 16-byte value. The raw bytes of the token will be XOR'd with this 
        /// value before being returned to the final unseal key provider.</param>
        /// <param name="pgpKey"><para>[optional]</para>
        /// A base64-encoded PGP public key. The raw bytes of the token will be encrypted with this value before being 
        /// returned to the final unseal key provider.</param>
        /// <returns>The root token generation status.</returns>
        Task<RootTokenGenerationStatus> InitiateRootTokenGenerationAsync(string base64EncodedOneTimePassword = null, string pgpKey = null);

        /// <summary>
        /// Cancels any in-progress root generation attempt. 
        /// This clears any progress made. 
        /// This must be called to change the OTP or PGP key being used.
        /// </summary>
        /// <returns>The task.</returns>
        Task CancelRootTokenGenerationAsync();

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
        /// <returns>
        /// An object indicating the attempt nonce, and completion status, 
        /// and the encoded root token, if the attempt is complete.
        /// </returns>
        Task<RootTokenGenerationStatus> ContinueRootTokenGenerationAsync(string masterShareKey, string nonce);

        /// <summary>
        /// Generates a root token in a single call. 
        /// Call this after calling the <see cref="InitiateRootTokenGenerationAsync"/> method.
        /// Provide all the master keys together.
        /// </summary>
        /// <param name="allMasterShareKeys">All the master share keys.</param>
        /// <param name="nonce"><para>[required]</para>
        /// The nonce of the root generation attempt.</param>
        /// <returns>The final Status after all the share keys are applied.</returns>
        Task<RootTokenGenerationStatus> QuickRootTokenGenerationAsync(string[] allMasterShareKeys, string nonce);

        /// <summary>
        /// Gets the seal status of the Vault.
        /// This is an unauthenticated call and does not need credentials.
        /// </summary>
        /// <returns>
        /// The seal status of the Vault.
        /// </returns>
        Task<SealStatus> GetSealStatusAsync();

        /// <summary>
        /// Seals the Vault.
        /// In HA mode, only an active node can be sealed. Standby nodes should be restarted to get the same effect. 
        /// Requires a token with root policy.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        Task SealAsync();

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
        /// <param name="resetCompletely">When <value>true</value>, the previously-provided unseal keys are discarded from memory and the unseal process is completely reset.
        /// Default value is <value>false</value>.
        /// If you make a call with the value as <value>true</value>, it doesn't matter if this call has a valid unused <see cref="masterShareKey" />. It'll be ignored.</param>
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

        /// <summary>
        /// Gets all the mounted secret backends.
        /// </summary>
        /// <returns>
        /// The mounted secret backends.
        /// </returns>
        Task<Secret<IEnumerable<SecretBackend>>> GetAllMountedSecretBackendsAsync();

        /// <summary>
        /// Mounts the new secret backend to the mount point in the URL.
        /// </summary>
        /// <param name="secretBackend">The secret backend.</param>
        /// <returns>
        /// A task
        /// </returns>
        Task MountSecretBackendAsync(SecretBackend secretBackend);

        /// <summary>
        /// Quick api to mount the secret backend with default settings.
        /// </summary>
        /// <param name="secretBackendType"><para>[required]</para>
        /// The backend type to mount.</param>
        /// <returns>
        /// A task
        /// </returns>
        Task QuickMountSecretBackendAsync(SecretBackendType secretBackendType);

        /// <summary>
        /// Unmounts the mount point specified in the URL.
        /// </summary>
        /// <param name="mountPoint"><para>[required]</para>
        /// The mount point for the secret backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <returns>
        /// A task
        /// </returns>
        Task UnmountSecretBackendAsync(string mountPoint);

        /// <summary>
        /// Quick api to unmounts the secret backend from the default mount point.
        /// </summary>
        /// <param name="secretBackendType"><para>[required]</para>
        /// The backend type to unmount.</param>
        /// <returns>
        /// A task
        /// </returns>
        Task QuickUnmountSecretBackendAsync(SecretBackendType secretBackendType);

        /// <summary>
        /// Gets the mounted secret backend's configuration values.
        /// Unlike the <see cref="GetAllMountedSecretBackendsAsync"/> method, 
        /// this will return the current time in seconds for each TTL, 
        /// which may be the system default or a mount-specific value.
        /// </summary>
        /// <param name="mountPoint"><para>[required]</para>
        /// The mount point for the secret backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <returns>
        /// The mounted secret backend's configuration values.
        /// </returns>
        Task<Secret<MountConfiguration>> GetMountedSecretBackendConfigurationAsync(string mountPoint);

        /// <summary>
        /// Tunes the mount configuration parameters for the given <see cref="mountPoint" />.
        /// </summary>
        /// <param name="mountPoint"><para>[required]</para>
        /// The mount point for the secret backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <param name="mountConfiguration"><para>[optional]</para>
        /// The mount configuration with the required setting values.
        /// Provide a value of <value>"0"</value> or <value>"system"</value> for the TTL settings 
        /// if you want to use the system defaults.</param>
        /// <returns>
        /// A task
        /// </returns>
        Task TuneSecretBackendConfigurationAsync(string mountPoint, MountConfiguration mountConfiguration = null);

        /// <summary>
        /// Remounts the secret backend from the previous mount point to the new mount point.
        /// </summary>
        /// <param name="previousMountPoint"><para>[required]</para>
        /// The previous mount point for the secret backend.</param>
        /// <param name="newMountPoint"><para>[required]</para>
        /// The new mount point for the secret backend.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task RemountSecretBackendAsync(string previousMountPoint, string newMountPoint);

        /// <summary>
        /// Gets all the enabled authentication backends.
        /// </summary>
        /// <returns>
        /// The enabled authentication backends.
        /// </returns>
        Task<Secret<IEnumerable<AuthenticationBackend>>> GetAllEnabledAuthenticationBackendsAsync();

        /// <summary>
        /// Enables a new authentication backend.
        /// The auth backend can be accessed and configured via the auth path specified in the URL. 
        /// </summary>
        /// <param name="authenticationBackend">The authentication backend.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task EnableAuthenticationBackendAsync(AuthenticationBackend authenticationBackend);

        /// <summary>
        /// Quickly enables a new authentication backend at the default mountpoint.
        /// </summary>
        /// <param name="authenticationBackendType">The authentication backend type.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task QuickEnableAuthenticationBackendAsync(AuthenticationBackendType authenticationBackendType);

        /// <summary>
        /// Disables the authentication backend at the given mount point.
        /// </summary>
        /// <param name="authenticationPath"><para>[required]</para>
        /// The authentication path for the authentication backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DisableAuthenticationBackendAsync(string authenticationPath);

        /// <summary>
        /// Gets the mounted authentication backend's configuration values.
        /// The lease values for each TTL may be the system default ("0" or "system") or a mount-specific value.
        /// </summary>
        /// <param name="authenticationPath"><para>[required]</para>
        /// The authentication path for the authentication backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <returns>
        /// The mounted secret backend's configuration values.
        /// </returns>
        Task<MountConfiguration> GetMountedAuthenticationBackendConfigurationAsync(string authenticationPath);

        /// <summary>
        /// Tunes the mount configuration parameters for the given <see cref="authenticationPath" />.
        /// </summary>
        /// <param name="authenticationPath"><para>[required]</para>
        /// The authentication path for the authentication backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <param name="mountConfiguration"><para>[required]</para>
        /// The mount configuration with the required setting values.
        /// Provide a value of <value>"0"</value> or <value>"system"</value> for the TTL settings if you want to use the system defaults.</param>
        /// <returns>
        /// A task
        /// </returns>
        Task TuneAuthenticationBackendConfigurationAsync(string authenticationPath, MountConfiguration mountConfiguration = null);

        /// <summary>
        /// Gets all the available policy names in the system.
        /// </summary>
        /// <returns>
        /// The policy names.
        /// </returns>
        Task<IEnumerable<string>> GetAllPoliciesAsync();

        /// <summary>
        /// Gets the rules for the named policy.
        /// </summary>
        /// <param name="policyName">
        /// <para>[required]</para>
        /// The name of the policy.</param>
        /// <returns>
        /// The rules for the policy.
        /// </returns>
        Task<Policy> GetPolicyAsync(string policyName);

        /// <summary>
        /// Adds or updates the policy.
        /// Once a policy is updated, it takes effect immediately to all associated users.
        /// </summary>
        /// <param name="policy"><para>[required]</para>
        /// The policy to be added or updated.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task WritePolicyAsync(Policy policy);

        /// <summary>
        /// Deletes the named policy. This will immediately affect all associated users.
        /// </summary>
        /// <param name="policyName"><para>[required]</para>
        /// The name of the policy.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeletePolicyAsync(string policyName);

        /// <summary>
        /// Gets the capabilities of the token on the given path.
        /// </summary>
        /// <param name="token"><para>[required]</para>
        /// Token for which capabilities are being queried.</param>
        /// <param name="path"><para>[required]</para>
        /// Path on which the token's capabilities will be checked.</param>
        /// <returns>The list of capabilities.</returns>
        Task<IEnumerable<string>> GetTokenCapabilitiesAsync(string token, string path);

        /// <summary>
        /// Gets the capabilities of client token on the given path. 
        /// Client token is the Vault token with which this API call is made.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// Path on which the token's capabilities will be checked.</param>
        /// <returns>The list of capabilities.</returns>
        Task<IEnumerable<string>> GetCallingTokenCapabilitiesAsync(string path);

        /// <summary>
        /// Gets the capabilities of the token associated with an accessor, on the given path.
        /// </summary>
        /// <param name="tokenAccessor"><para>[required]</para>
        /// Token accessor for which capabilities are being queried.</param>
        /// <param name="path"><para>[required]</para>
        /// Path on which the token's capabilities will be checked.</param>
        /// <returns>The list of capabilities.</returns>
        Task<IEnumerable<string>> GetTokenAccessorCapabilitiesAsync(string tokenAccessor, string path);

        /// <summary>
        /// Gets all the enabled audit backends.
        /// </summary>
        /// <returns>
        /// The enabled audit backends.
        /// </returns>
        Task<Secret<IEnumerable<AuditBackend>>> GetAllEnabledAuditBackendsAsync();

        /// <summary>
        /// Enables a new audit backend at the specified mount point.
        /// </summary>
        /// <param name="auditBackend">The audit backend.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task EnableAuditBackendAsync(AuditBackend auditBackend);

        /// <summary>
        /// Disables the audit backend at the given mount point.
        /// </summary>
        /// <param name="mountPoint"><para>[required]</para>
        /// The mount point for the audit backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DisableAuditBackendAsync(string mountPoint);

        /// <summary>
        /// Hash the given input data with the specified audit backend's hash function and salt.
        /// This endpoint can be used to discover whether a given plaintext string (the input parameter) appears in
        /// the audit log in obfuscated form.
        /// Note that the audit log records requests and responses; since the Vault API is JSON-based,
        /// any binary data returned from an API call (such as a DER-format certificate) is base64-encoded by
        /// the Vault server in the response, and as a result such information should also be base64-encoded
        /// to supply into the <see cref="inputToHash" /> parameter.
        /// </summary>
        /// <param name="mountPoint"><para>[required]</para>
        /// The mount point for the audit backend. (with or without trailing slashes. it doesn't matter)</param>
        /// <param name="inputToHash"><para>[required]</para>
        /// The input value to hash</param>
        /// <returns>
        /// The hashed value.
        /// </returns>
        Task<string> HashWithAuditBackendAsync(string mountPoint, string inputToHash);

        /// <summary>
        /// Renews the secret by requesting to extending the lease.
        /// </summary>
        /// <param name="leaseId"><para>[required]</para>
        /// The lease identifier for the secret.</param>
        /// <param name="incrementSeconds"><para>[optional]</para>
        /// A requested amount of time in seconds to extend the lease. This is advisory.</param>
        /// <returns>
        /// The secret with the lease information and the data as a dictionary.
        /// </returns>
        Task<Secret<Dictionary<string, object>>> RenewSecretAsync(string leaseId, int? incrementSeconds = null);

        /// <summary>
        /// Revokes the secret effective immediately.
        /// </summary>
        /// <param name="leaseId"><para>[required]</para>
        /// The lease identifier for the secret.</param>
        /// <returns>
        /// The task
        /// </returns>
        Task RevokeSecretAsync(string leaseId);

        /// <summary>
        /// Revokes all the secrets or tokens generated under the given prefix immediately.
        /// Access to it should be tightly controlled as it can be used to revoke very large numbers of secrets/tokens at once.
        /// </summary>
        /// <param name="pathPrefix"><para>[required]</para>
        /// The path prefix. (with or without trailing slashes. it doesn't matter)</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task RevokeAllSecretsOrTokensUnderPrefixAsync(string pathPrefix);

        /// <summary>
        /// Revoke all secrets or tokens generated under a given prefix immediately. 
        /// Unlike <see cref="RevokeAllSecretsOrTokensUnderPrefixAsync"/>, this path ignores backend errors 
        /// encountered during revocation. 
        /// This is potentially very dangerous and should only be used in specific emergency 
        /// situations where errors in the backend or the connected backend service 
        /// prevent normal revocation. 
        /// By ignoring these errors, Vault abdicates responsibility for ensuring that the issued 
        /// credentials or secrets are properly revoked and/or cleaned up. 
        /// Access to this endpoint should be tightly controlled.
        /// </summary>
        /// <param name="pathPrefix"><para>[required]</para>The path prefix.</param>
        /// <returns>The task.</returns>
        Task ForceRevokeAllSecretsOrTokensUnderPrefixAsync(string pathPrefix);

        /// <summary>
        /// Looks up wrapping properties for the given token.
        /// </summary>
        /// <param name="tokenId">
        /// <para>[required]</para>
        /// The wrapping token identifier.
        /// </param>
        /// <returns>The token wrap info.</returns>
        Task<Secret<TokenWrapInfo>> LookupTokenWrapInfoAsync(string tokenId);

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
        /// <returns>The secret with re-wrapped info.</returns>
        Task<Secret<object>> RewrapWrappedResponseDataAsync(string tokenId);

        /// <summary>
        /// Returns the original response inside the given wrapping token. 
        /// This endpoint provides additional validation checks on the token, 
        /// returns the original value on the wire and ensures that the response is properly audit-logged.
        /// </summary>
        /// <param name="tokenId">
        /// <para>[required]</para>
        /// The wrapping token identifier.
        /// </param>
        /// <returns>The unwrapped original data.</returns>
        Task<Secret<Dictionary<string, object>>> UnwrapWrappedResponseDataAsync(string tokenId);

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
        /// <returns>The unwrapped original data.</returns>
        Task<Secret<TData>> UnwrapWrappedResponseDataAsync<TData>(string tokenId);

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
        /// <returns>The wrapped response.</returns>
        Task<Secret<object>> WrapResponseDataAsync(Dictionary<string, object> data, string wrapTimeToLive);

        /// <summary>
        /// Gets the high availability status and current leader instance of Vault.
        /// </summary>
        /// <returns>
        /// The leader info.
        /// </returns>
        Task<Leader> GetLeaderAsync();

        /// <summary>
        /// Forces the node to give up active status. 
        /// If the node does not have active status, this endpoint does nothing. 
        /// Note that the node will sleep for ten seconds before attempting to grab the active lock again, 
        /// but if no standby nodes grab the active lock in the interim, 
        /// the same node may become the active node again. 
        /// This API is a root protected call.
        /// </summary>
        /// <returns>The task.</returns>
        Task StepDownActiveNodeAsync();

        /// <summary>
        /// Gets information about the current encryption key used by Vault
        /// </summary>
        /// <returns>
        /// The status of the encryption key.
        /// </returns>
        Task<EncryptionKeyStatus> GetEncryptionKeyStatusAsync();

        /// <summary>
        /// Gets the configuration and progress of the current rekey attempt.
        /// Information about the new shares to generate and the threshold required for the new shares,
        /// the number of unseal keys provided for this rekey and the required number of unseal keys is returned.
        /// This is an unauthenticated call and does not need credentials.
        /// </summary>
        /// <returns>
        /// The rekey status.
        /// </returns>
        Task<RekeyStatus> GetRekeyStatusAsync();

        /// <summary>
        /// Initiates a new rekey attempt.
        /// Only a single rekey attempt can take place at a time, and changing the parameters of a rekey requires canceling and starting a new rekey.
        /// This is an unauthenticated call and does not need credentials.
        /// </summary>
        /// <param name="secretShares"><para>[required]</para>
        /// The number of shares to split the master key into.</param>
        /// <param name="secretThreshold"><para>[required]</para>
        /// The number of shares required to reconstruct the master key.
        /// This must be less than or equal to <see cref="secretShares" />.</param>
        /// <param name="pgpKeys"><para>[optional]</para>
        /// An array of PGP public keys used to encrypt the output unseal keys.
        /// Ordering is preserved.
        /// The keys must be base64-encoded from their original binary representation.
        /// The size of this array must be the same as <see cref="secretShares" />.</param>
        /// <param name="backup"><para>[optional]</para>
        /// If using PGP-encrypted keys, whether Vault should also back them up to a well-known 
        /// location in physical storage. These can then be retrieved and removed 
        /// via the GetRekeyBackupAsync endpoint. Makes sense only when pgp keys are provided.
        /// Defaults to 'false', meaning no backup.
        /// </param>
        /// <returns>
        /// The rekey status.
        /// </returns>
        Task<RekeyStatus> InitiateRekeyAsync(int secretShares, int secretThreshold, string[] pgpKeys = null, bool backup = false);

        /// <summary>
        /// Cancels any in-progress rekey. This clears the rekey settings as well as any progress made.
        /// This must be called to change the parameters of the rekey.
        /// This is an unauthenticated call and does not need credentials.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        Task CancelRekeyAsync();

        /// <summary>
        /// Gets the the backup copy of PGP-encrypted unseal keys. 
        /// The returned value is the nonce of the rekey operation and a map of PGP key 
        /// fingerprint to hex-encoded PGP-encrypted key.
        /// </summary>
        /// <returns>The rekey backup info.</returns>
        Task<RekeyBackupInfo> GetRekeyBackupKeysAsync();

        /// <summary>
        /// Deletes the backup copy of PGP-encrypted unseal keys.
        /// </summary>
        /// <returns>The task.</returns>
        Task DeleteRekeyBackupKeysAsync();

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
        /// <returns>
        /// An object indicating the rekey operation nonce and completion status; 
        /// if completed, the new master keys are returned. 
        /// If the keys are PGP-encrypted, an array of key fingerprints will also be provided 
        /// (with the order in which the keys were used for encryption) along with whether 
        /// or not the keys were backed up to physical storage.
        /// </returns>
        Task<RekeyProgress> ContinueRekeyAsync(string masterShareKey, string rekeyNonce);

        /// <summary>
        /// Rekeys the Vault in a single call.
        /// Call this after calling the <see cref="InitiateRekeyAsync"/> method.
        /// Provide all the master keys together.
        /// This is an unauthenticated call and does not need credentials.
        /// </summary>
        /// <param name="allMasterShareKeys">All the master share keys.</param>
        /// <param name="rekeyNonce"><para>[required]</para>
        /// The nonce of the rekey operation.</param>
        /// <returns>The final Rekey progress after all the share keys are applied.</returns>
        Task<RekeyProgress> QuickRekeyAsync(string[] allMasterShareKeys, string rekeyNonce);

        /// <summary>
        /// Trigger a rotation of the backend encryption key. 
        /// This is the key that is used to encrypt data written to the storage backend, and is not provided to operators.
        /// This operation is done online. 
        /// Future values are encrypted with the new key, while old values are decrypted with previous encryption keys.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        Task RotateEncryptionKeyAsync();

        /// <summary>
        /// Reads the value of the key at the given path.
        /// This is the raw path in the sorage backend and not the logical path that is exposed via the mount system.
        /// </summary>
        /// <param name="storagePath"><para>[required]</para>
        /// Raw path in the storage backend and not the logical path that is exposed via the mount system.</param>
        /// <returns>
        /// The Secret with raw data.
        /// </returns>
        /// t
        Task<Secret<RawData>> ReadRawSecretAsync(string storagePath);

        /// <summary>
        /// Update the value of the key at the given path.
        /// This is the raw path in the storage backend and not the logical path that is exposed via the mount system.
        /// </summary>
        /// <param name="storagePath"><para>[required]</para>
        /// Raw path in the storage backend and not the logical path that is exposed via the mount system.</param>
        /// <param name="values"><para>[required]</para>
        /// The values to write. The dictionary will be JSONized as a raw value string before being written.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task WriteRawSecretAsync(string storagePath, IDictionary<string, object> values);

        /// <summary>
        /// Delete the key with given path.
        /// This is the raw path in the storage backend and not the logical path that is exposed via the mount system.
        /// </summary>
        /// <param name="storagePath"><para>[required]</para>
        /// Raw path in the storage backend and not the logical path that is exposed via the mount system.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeleteRawSecretAsync(string storagePath);

        /// <summary>
        /// Gets the health status of Vault. This provides a simple way to monitor the health of a Vault instance.
        /// This is an unauthenticated call and does not use the credentials.
        /// </summary>
        /// <param name="standbyOk"><para>[optional]</para>
        /// A flag to indicate that being a standby should still return the active status code 
        /// instead of the standby status code of HTTP 429 (or whatever is provided as standbyStatusCode)
        /// DEFAULTs to <value>null</value>, meaning a standby code will be returned.
        /// </param>
        /// <param name="activeStatusCode"><para>[optional]</para>
        /// A user defined status code provided to indicate the status code that should be returned 
        /// for an active node instead of the default successful response of HTTP 200.
        /// DEFAULTs to <value>null</value>, meaning the default HTTP 200 Status code will be returned.
        /// </param>
        /// <param name="standbyStatusCode"><para>[optional]</para>
        /// A user defined status code provided to indicate the status code that should be returned 
        /// for an standby node instead of the default error response of HTTP 429.
        /// DEFAULTs to <value>null</value>, meaning the default HTTP 429 Status code will be returned.
        /// </param>
        /// <param name="sealedStatusCode"><para>[optional]</para>
        /// A user defined status code provided to indicate the status code that should be returned 
        /// for an sealed node instead of the default error response of HTTP 503.
        /// DEFAULTs to <value>null</value>, meaning the default HTTP 503 Status code will be returned.
        /// </param>
        /// <param name="uninitializedStatusCode"><para>[optional]</para>
        /// A user defined status code provided to indicate the status code that should be returned 
        /// for an uninitialized vault node instead of the default error response of HTTP 501.
        /// DEFAULTs to <value>null</value>, meaning the default HTTP 501 Status code will be returned.
        /// </param>
        /// <param name="queryHttpMethod"><para>[optional]</para>
        /// The <see cref="HttpMethod"/> to be used to query vault. By default <see cref="HttpMethod.Get"/> will be used.
        /// You can change it to <see cref="HttpMethod.Head"/>.
        /// </param>
        /// <returns>
        /// The health status.
        /// </returns>
        Task<HealthStatus> GetHealthStatusAsync(bool? standbyOk = null, int? activeStatusCode = null, int? standbyStatusCode = null, int? sealedStatusCode = null, int? uninitializedStatusCode = null, HttpMethod queryHttpMethod = null);

        /// <summary>
        /// Configures the root IAM credentials used. 
        /// If static credentials are not provided using this endpoint, then the credentials will be retrieved 
        /// from the environment variables AWS_ACCESS_KEY, AWS_SECRET_KEY and AWS_REGION respectively. 
        /// If the credentials are still not found and if the backend is configured on 
        /// an EC2 instance with metadata querying capabilities, 
        /// the credentials are fetched automatically.
        /// This API is a root protected call.
        /// </summary>
        /// <param name="awsRootCredentials"><para>[optional]</para>
        /// The root credentials need permission to perform various IAM actions.
        /// These are the actions that the AWS secret backend uses to manage IAM credentials.
        /// Provide null, if you want to use the environment variables of Vault Server or fetch from metadata of EC2 instance. 
        /// </param>
        /// <param name="awsBackendMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretBackendType.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task AWSConfigureRootCredentialsAsync(AWSRootCredentials awsRootCredentials, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS);

        /// <summary>
        /// Configures the lease settings for generated credentials.
        /// This API is a root protected call.
        /// </summary>
        /// <param name="credentialLeaseSettings"><para>[required]</para>
        /// The credential lease settings.</param>
        /// <param name="awsBackendMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretBackendType.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task AWSConfigureCredentialLeaseSettingsAsync(CredentialLeaseSettings credentialLeaseSettings, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS);

        /// <summary>
        /// Creates or updates a named AWS role.
        /// </summary>
        /// <param name="awsRoleName"><para>[required]</para>
        /// Name of the AWS role.</param>
        /// <param name="awsRoleDefinition"><para>[required]</para>
        /// The AWS role definition with IAM policy or full ARN. Provide one of the two.</param>
        /// <param name="awsBackendMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretBackendType.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task AWSWriteNamedRoleAsync(string awsRoleName, AWSRoleDefinition awsRoleDefinition, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS);

        /// <summary>
        /// Queries a named AWS role definition to get the IAM policy.
        /// </summary>
        /// <param name="awsRoleName"><para>[required]</para>
        /// Name of the AWS role.</param>
        /// <param name="awsBackendMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretBackendType.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the AWS Role definition as an IAM policy in a string JSON format.
        /// </returns>
        Task<Secret<AWSRoleDefinition>> AWSReadNamedRoleAsync(string awsRoleName, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS, string wrapTimeToLive = null);

        /// <summary>
        /// Deletes a named AWS role.
        /// </summary>
        /// <param name="awsRoleName"><para>[required]</para>
        /// Name of the AWS role.</param>
        /// <param name="awsBackendMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretBackendType.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task AWSDeleteNamedRoleAsync(string awsRoleName, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS);

        /// <summary>
        /// Get the list of existing roles in the backend.
        /// </summary>
        /// <param name="awsBackendMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretBackendType.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The role list.</returns>
        Task<Secret<ListInfo>> AWSGetRoleListAsync(string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS, string wrapTimeToLive = null);

        /// <summary>
        /// Generates a dynamic IAM AWS credential based on the named role.
        /// </summary>
        /// <param name="awsRoleName"><para>[required]</para>
        /// Name of the AWS role.</param>
        /// <param name="awsBackendMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretBackendType.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="AWSCredentials" /> as the data.
        /// </returns>
        Task<Secret<AWSCredentials>> AWSGenerateDynamicCredentialsAsync(string awsRoleName, string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS, string wrapTimeToLive = null);

        /// <summary>
        /// Generates a dynamic IAM AWS credential  with an STS token based on the named role.
        /// The TTL will be 3600 seconds (one hour).
        /// </summary>
        /// <param name="awsRoleName"><para>[required]</para>
        /// Name of the AWS role.</param>
        /// <param name="timeToLive"><para>[optional]</para>
        /// Time to live. Defaults to 1 hour</param>
        /// <param name="awsBackendMountPoint"><para>[optional]</para>
        /// The mount point for the AWS backend. Defaults to <see cref="SecretBackendType.AWS" />
        /// Provide a value only if you have customized the AWS mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="AWSCredentials" /> as the data.
        /// </returns>
        Task<Secret<AWSCredentials>> AWSGenerateDynamicCredentialsWithSecurityTokenAsync(string awsRoleName, string timeToLive = "1h", string awsBackendMountPoint = SecretBackendDefaultMountPoints.AWS, string wrapTimeToLive = null);

        /// <summary>
        /// Configures the connection information used to communicate with Cassandra.
        /// </summary>
        /// <param name="cassandraConnectionInfo"><para>[required]</para>
        /// The cassandra connection information.</param>
        /// <param name="cassandraBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Cassandra backend. Defaults to <see cref="SecretBackendType.Cassandra" />
        /// Provide a value only if you have customized the Cassandra mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task CassandraConfigureConnectionAsync(CassandraConnectionInfo cassandraConnectionInfo, string cassandraBackendMountPoint = SecretBackendDefaultMountPoints.Cassandra);

        /// <summary>
        /// Creates or updates a named Cassandra role.
        /// </summary>
        /// <param name="cassandraRoleName"><para>[required]</para>
        /// Name of the Cassandra role.</param>
        /// <param name="cassandraRoleDefinition"><para>[required]</para>
        /// The Cassandra role definition with the creation, rollback query and lease information.</param>
        /// <param name="cassandraBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Cassandra backend. Defaults to <see cref="SecretBackendType.Cassandra" />
        /// Provide a value only if you have customized the Cassandra mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task CassandraWriteNamedRoleAsync(string cassandraRoleName, CassandraRoleDefinition cassandraRoleDefinition, string cassandraBackendMountPoint = SecretBackendDefaultMountPoints.Cassandra);

        /// <summary>
        /// Queries a named Cassandra role definition
        /// </summary>
        /// <param name="cassandraRoleName"><para>[required]</para>
        /// Name of the Cassandra role.</param>
        /// <param name="cassandraBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Cassandra backend. Defaults to <see cref="SecretBackendType.Cassandra" />
        /// Provide a value only if you have customized the Cassandra mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the Cassandra role definition with the creation, rollback query and lease information.
        /// </returns>
        Task<Secret<CassandraRoleDefinition>> CassandraReadNamedRoleAsync(string cassandraRoleName, string cassandraBackendMountPoint = SecretBackendDefaultMountPoints.Cassandra, string wrapTimeToLive = null);

        /// <summary>
        /// Deletes a named Cassandra role definition
        /// </summary>
        /// <param name="cassandraRoleName"><para>[required]</para>
        /// Name of the Cassandra role.</param>
        /// <param name="cassandraBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Cassandra backend. Defaults to <see cref="SecretBackendType.Cassandra" />
        /// Provide a value only if you have customized the Cassandra mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task CassandraDeleteNamedRoleAsync(string cassandraRoleName, string cassandraBackendMountPoint = SecretBackendDefaultMountPoints.Cassandra);

        /// <summary>
        /// Generates a new set of dynamic credentials based on the named role.
        /// </summary>
        /// <param name="cassandraRoleName"><para>[required]</para>
        /// Name of the Cassandra role.</param>
        /// <param name="cassandraBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Cassandra backend. Defaults to <see cref="SecretBackendType.Cassandra" />
        /// Provide a value only if you have customized the Cassandra mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="UsernamePasswordCredentials" /> as the data.
        /// </returns>
        Task<Secret<UsernamePasswordCredentials>> CassandraGenerateDynamicCredentialsAsync(string cassandraRoleName, string cassandraBackendMountPoint = SecretBackendDefaultMountPoints.Cassandra, string wrapTimeToLive = null);

        /// <summary>
        /// Configures the access information for Consul.
        /// This API is a root protected call.
        /// </summary>
        /// <param name="consulAccessInfo"><para>[required]</para>
        /// The consul access information.</param>
        /// <param name="consulBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Consul backend. Defaults to <see cref="SecretBackendType.Consul" />
        /// Provide a value only if you have customized the Consul mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task ConsulConfigureAccessAsync(ConsulAccessInfo consulAccessInfo, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul);

        /// <summary>
        /// Creates or updates a named Consul role.
        /// </summary>
        /// <param name="consulRoleName"><para>[required]</para>
        /// Name of the Consul role.</param>
        /// <param name="consulRoleDefinition"><para>[required]</para>
        /// The Consul role definition with the policy and token information.</param>
        /// <param name="consulBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Consul backend. Defaults to <see cref="SecretBackendType.Consul" />
        /// Provide a value only if you have customized the Consul mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task ConsulWriteNamedRoleAsync(string consulRoleName, ConsulRoleDefinition consulRoleDefinition, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul);

        /// <summary>
        /// Queries a named Consul role.
        /// </summary>
        /// <param name="consulRoleName"><para>[required]</para>
        /// Name of the Consul role.</param>
        /// <param name="consulBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Consul backend. Defaults to <see cref="SecretBackendType.Consul" />
        /// Provide a value only if you have customized the Consul mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the Consul role definition.
        /// </returns>
        Task<Secret<ConsulRoleDefinition>> ConsulReadNamedRoleAsync(string consulRoleName, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul, string wrapTimeToLive = null);

        /// <summary>
        /// Returns a list of available roles. Only the role names are returned, not any values.
        /// </summary>
        /// <param name="consulBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Consul backend. Defaults to <see cref="SecretBackendType.Consul" />
        /// Provide a value only if you have customized the Consul mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// A list of available roles. Only the role names are returned, not any values.
        /// </returns>
        Task<Secret<ListInfo>> ConsulReadRoleListAsync(string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul, string wrapTimeToLive = null);

        /// <summary>
        /// Deletes a Consul role definition.
        /// </summary>
        /// <param name="consulRoleName"><para>[required]</para>
        /// Name of the Consul role.</param>
        /// <param name="consulBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Consul backend. Defaults to <see cref="SecretBackendType.Consul" />
        /// Provide a value only if you have customized the Consul mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task ConsulDeleteNamedRoleAsync(string consulRoleName, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul);

        /// <summary>
        /// Generates a dynamic Consul token based on the role definition.
        /// </summary>
        /// <param name="consulRoleName"><para>[required]</para>
        /// Name of the Consul role.</param>
        /// <param name="consulBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Consul backend. Defaults to <see cref="SecretBackendType.Consul" />
        /// Provide a value only if you have customized the Consul mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="ConsulCredentials" /> as the data.
        /// </returns>
        Task<Secret<ConsulCredentials>> ConsulGenerateDynamicCredentialsAsync(string consulRoleName, string consulBackendMountPoint = SecretBackendDefaultMountPoints.Consul, string wrapTimeToLive = null);

        /// <summary>
        /// Retrieves the secret at the specified location.
        /// </summary>
        /// <param name="locationPath"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the data.
        /// </returns>
        Task<Secret<Dictionary<string, object>>> CubbyholeReadSecretAsync(string locationPath, string wrapTimeToLive = null);

        /// <summary>
        /// Retrieves the secret location path entries at the specified location.
        /// Folders are suffixed with /. The input must be a folder; list on a file will not return a value. 
        /// The values themselves are not accessible via this API.
        /// </summary>
        /// <param name="locationPath"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret list with the data.
        /// </returns>
        Task<Secret<ListInfo>> CubbyholeReadSecretLocationPathListAsync(string locationPath, string wrapTimeToLive = null);

        /// <summary>
        /// Stores a secret at the specified location.
        /// </summary>
        /// <param name="locationPath"><para>[required]</para>
        /// The location path where the secret needs to be stored.</param>
        /// <param name="values"><para>[required]</para>
        /// The values to be written. The values will be overwritten.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task CubbyholeWriteSecretAsync(string locationPath, IDictionary<string, object> values);

        /// <summary>
        /// Deletes the secret at the specified location.
        /// </summary>
        /// <param name="locationPath"><para>[required]</para>
        /// The location path where the secret needs to be deleted from.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task CubbyholeDeleteSecretAsync(string locationPath);

        /// <summary>
        /// Retrieves the secret at the specified location.
        /// </summary>
        /// <param name="locationPath"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// <param name="genericBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Generic backend. Defaults to <see cref="SecretBackendType.Generic" />
        /// Provide a value only if you have customized the Generic mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the data.
        /// </returns>
        Task<Secret<Dictionary<string, object>>> GenericReadSecretAsync(string locationPath, string genericBackendMountPoint = SecretBackendDefaultMountPoints.Generic, string wrapTimeToLive = null);

        /// <summary>
        /// Retrieves the secret location path entries at the specified location.
        /// Folders are suffixed with /. The input must be a folder; list on a file will not return a value. 
        /// The values themselves are not accessible via this API.
        /// </summary>
        /// <param name="locationPath"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// <param name="genericBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Generic backend. Defaults to <see cref="SecretBackendType.Generic" />
        /// Provide a value only if you have customized the Generic mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret list with the data.
        /// </returns>
        Task<Secret<ListInfo>> GenericReadSecretLocationPathListAsync(string locationPath, string genericBackendMountPoint = SecretBackendDefaultMountPoints.Generic, string wrapTimeToLive = null);

        /// <summary>
        /// Stores a secret at the specified location.
        /// If the value does not yet exist, the calling token must have an ACL policy granting the create capability. 
        /// If the value already exists, the calling token must have an ACL policy granting the update capability.
        /// </summary>
        /// <param name="locationPath"><para>[required]</para>
        /// The location path where the secret needs to be stored.</param>
        /// <param name="values"><para>[required]</para>
        /// The values to be written. The values will be overwritten.</param>
        /// <param name="genericBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Generic backend. Defaults to <see cref="SecretBackendType.Generic" />
        /// Provide a value only if you have customized the Generic mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task GenericWriteSecretAsync(string locationPath, IDictionary<string, object> values, string genericBackendMountPoint = SecretBackendDefaultMountPoints.Generic);

        /// <summary>
        /// Deletes the secret at the specified location.
        /// </summary>
        /// <param name="locationPath"><para>[required]</para>
        /// The location path where the secret needs to be deleted from.</param>
        /// <param name="genericBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Generic backend. Defaults to <see cref="SecretBackendType.Generic" />
        /// Provide a value only if you have customized the Generic mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task GenericDeleteSecretAsync(string locationPath, string genericBackendMountPoint = SecretBackendDefaultMountPoints.Generic);

        /// <summary>
        /// Configures the connection information used to communicate with MongoDb Server.
        /// This API is a root protected call.
        /// </summary>
        /// <param name="mongoDbConnectionInfo"><para>[required]</para>
        /// The MongoDb connection information.</param>
        /// <param name="mongoDbBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MongoDb backend. Defaults to <see cref="SecretBackendType.MongoDb" />
        /// Provide a value only if you have customized the MongoDb mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The task.
        /// </returns>
        Task<Secret<object>> MongoDbConfigureConnectionAsync(MongoDbConnectionInfo mongoDbConnectionInfo, string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb, string wrapTimeToLive = null);

        /// <summary>
        /// Queries the connection configuration. Access to this endpoint should be controlled via ACLs as it will 
        /// return the connection URI as it is, including passwords, if any.
        /// </summary>
        /// <param name="mongoDbBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MongoDb backend. Defaults to <see cref="SecretBackendType.MongoDb" />
        /// Provide a value only if you have customized the MongoDb mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The MongoDb connection information.
        /// </returns>
        Task<Secret<MongoDbConnectionInfo>> MongoDbReadConnectionInfoAsync(string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb, string wrapTimeToLive = null);

        /// <summary>
        /// Configures the lease settings for generated credentials.
        /// </summary>
        /// <param name="credentialTimeToLiveSettings"><para>[required]</para>
        /// The credential lease settings.</param>
        /// <param name="mongoDbBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MongoDb backend. Defaults to <see cref="SecretBackendType.MongoDb" />
        /// Provide a value only if you have customized the MongoDb mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MongoDbConfigureCredentialLeaseSettingsAsync(CredentialTimeToLiveSettings credentialTimeToLiveSettings, string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb);

        /// <summary>
        /// Queries the lease configuration.
        /// </summary>
        /// <param name="mongoDbBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MongoDb backend. Defaults to <see cref="SecretBackendType.MongoDb" />
        /// Provide a value only if you have customized the MongoDb mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The MongoDb lease settings.
        /// </returns>
        Task<Secret<CredentialTimeToLiveSettings>> MongoDbReadCredentialLeaseSettingsAsync(string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb, string wrapTimeToLive = null);
        
        /// <summary>
        /// Creates or updates a named MongoDb role.
        /// </summary>
        /// <param name="mongoDbRoleName"><para>[required]</para>
        /// Name of the MongoDb role.</param>
        /// <param name="mongoDbRoleDefinition"><para>[required]</para>
        /// The MongoDb role definition with the creation, rollback query and lease information.</param>
        /// <param name="mongoDbBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MongoDb backend. Defaults to <see cref="SecretBackendType.MongoDb" />
        /// Provide a value only if you have customized the MongoDb mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MongoDbWriteNamedRoleAsync(string mongoDbRoleName, MongoDbRoleDefinition mongoDbRoleDefinition, string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb);

        /// <summary>
        /// Queries a named MongoDb role definition
        /// </summary>
        /// <param name="mongoDbRoleName"><para>[required]</para>
        /// Name of the MongoDb role.</param>
        /// <param name="mongoDbBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MongoDb backend. Defaults to <see cref="SecretBackendType.MongoDb" />
        /// Provide a value only if you have customized the MongoDb mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the MongoDb role definition with the creation, rollback query and lease information.
        /// </returns>
        Task<Secret<MongoDbRoleDefinition>> MongoDbReadNamedRoleAsync(string mongoDbRoleName, string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb, string wrapTimeToLive = null);

        /// <summary>
        /// Returns a list of available roles. Only the role names are returned, not any values.
        /// </summary>
        /// <param name="mongoDbBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MongoDb backend. Defaults to <see cref="SecretBackendType.MongoDb" />
        /// Provide a value only if you have customized the MongoDb mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// A list of available roles. Only the role names are returned, not any values.
        /// </returns>
        Task<Secret<ListInfo>> MongoDbReadRoleListAsync(string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb, string wrapTimeToLive = null);

        /// <summary>
        /// Deletes a named MongoDb role definition
        /// </summary>
        /// <param name="mongoDbRoleName"><para>[required]</para>
        /// Name of the MongoDb role.</param>
        /// <param name="mongoDbBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MongoDb backend. Defaults to <see cref="SecretBackendType.MongoDb" />
        /// Provide a value only if you have customized the MongoDb mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MongoDbDeleteNamedRoleAsync(string mongoDbRoleName, string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb);

        /// <summary>
        /// Generates a new set of dynamic credentials based on the named role.
        /// </summary>
        /// <param name="mongoDbRoleName"><para>[required]</para>
        /// Name of the MongoDb role.</param>
        /// <param name="mongoDbBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MongoDb backend. Defaults to <see cref="SecretBackendType.MongoDb" />
        /// Provide a value only if you have customized the MongoDb mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="MongoDbUsernamePasswordCredentials" /> as the data.
        /// </returns>
        Task<Secret<MongoDbUsernamePasswordCredentials>> MongoDbGenerateDynamicCredentialsAsync(string mongoDbRoleName, string mongoDbBackendMountPoint = SecretBackendDefaultMountPoints.MongoDb, string wrapTimeToLive = null);

        /// <summary>
        /// Configures the connection information used to communicate with Microsoft Sql Server.
        /// This API is a root protected call.
        /// </summary>
        /// <param name="microsoftSqlConnectionInfo"><para>[required]</para>
        /// The Microsoft Sql connection information.</param>
        /// <param name="microsoftSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Microsoft Sql backend. Defaults to <see cref="SecretBackendType.MicrosoftSql" />
        /// Provide a value only if you have customized the Microsoft Sql mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MicrosoftSqlConfigureConnectionAsync(MicrosoftSqlConnectionInfo microsoftSqlConnectionInfo, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql);

        // Task<Secret<MicrosoftSqlConnectionInfo>> MicrosoftSqlReadConnectionInfoAsync(string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql);

        /// <summary>
        /// Configures the lease settings for generated credentials.
        /// </summary>
        /// <param name="credentialTimeToLiveSettings"><para>[required]</para>
        /// The credential lease settings.</param>
        /// <param name="microsoftSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MicrosoftSql backend. Defaults to <see cref="SecretBackendType.MicrosoftSql" />
        /// Provide a value only if you have customized the MicrosoftSql mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MicrosoftSqlConfigureCredentialLeaseSettingsAsync(CredentialTimeToLiveSettings credentialTimeToLiveSettings, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql);

        /// <summary>
        /// Queries the Microsoft SQL credential lease settings.
        /// </summary>
        /// <param name="microsoftSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MicrosoftSql backend. Defaults to <see cref="SecretBackendType.MicrosoftSql" />
        /// Provide a value only if you have customized the MicrosoftSql mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The lease settings.</returns>
        Task<Secret<CredentialTimeToLiveSettings>> MicrosoftSqlReadCredentialLeaseSettingsAsync(string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql, string wrapTimeToLive = null);

        /// <summary>
        /// Creates or updates a named MicrosoftSql role definition.
        /// </summary>
        /// <param name="microsoftSqlRoleName"><para>[required]</para>
        /// Name of the MicrosoftSql role.</param>
        /// <param name="microsoftSqlRoleDefinition"><para>[required]</para>
        /// The MicrosoftSql role definition with the creation, rollback query and lease information.</param>
        /// <param name="microsoftSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MicrosoftSql backend. Defaults to <see cref="SecretBackendType.MicrosoftSql" />
        /// Provide a value only if you have customized the MicrosoftSql mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MicrosoftSqlWriteNamedRoleAsync(string microsoftSqlRoleName, MicrosoftSqlRoleDefinition microsoftSqlRoleDefinition, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql);

        /// <summary>
        /// Queries a named MicrosoftSql role definition
        /// </summary>
        /// <param name="microsoftSqlRoleName"><para>[required]</para>
        /// Name of the MicrosoftSql role.</param>
        /// <param name="microsoftSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MicrosoftSql backend. Defaults to <see cref="SecretBackendType.MicrosoftSql" />
        /// Provide a value only if you have customized the MicrosoftSql mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the MicrosoftSql role definition with the creation, rollback query and lease information.
        /// </returns>
        Task<Secret<MicrosoftSqlRoleDefinition>> MicrosoftSqlReadNamedRoleAsync(string microsoftSqlRoleName, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql, string wrapTimeToLive = null);

        /// <summary>
        /// Returns a list of available roles. Only the role names are returned, not any values.
        /// </summary>
        /// <param name="microsoftSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MicrosoftSql backend. Defaults to <see cref="SecretBackendType.MicrosoftSql" />
        /// Provide a value only if you have customized the MicrosoftSql mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// A list of available roles. Only the role names are returned, not any values.
        /// </returns>
        Task<Secret<ListInfo>> MicrosoftSqlReadRoleListAsync(string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql, string wrapTimeToLive = null);

        /// <summary>
        /// Deletes a named MicrosoftSql role definition
        /// </summary>
        /// <param name="microsoftSqlRoleName"><para>[required]</para>
        /// Name of the MicrosoftSql role.</param>
        /// <param name="microsoftSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MicrosoftSql backend. Defaults to <see cref="SecretBackendType.MicrosoftSql" />
        /// Provide a value only if you have customized the MicrosoftSql mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MicrosoftSqlDeleteNamedRoleAsync(string microsoftSqlRoleName, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql);

        /// <summary>
        /// Generates a new set of dynamic credentials based on the named role.
        /// </summary>
        /// <param name="microsoftSqlRoleName"><para>[required]</para>
        /// Name of the MicrosoftSql role.</param>
        /// <param name="microsoftSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MicrosoftSql backend. Defaults to <see cref="SecretBackendType.MicrosoftSql" />
        /// Provide a value only if you have customized the MicrosoftSql mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="UsernamePasswordCredentials" /> as the data.
        /// </returns>
        Task<Secret<UsernamePasswordCredentials>> MicrosoftSqlGenerateDynamicCredentialsAsync(string microsoftSqlRoleName, string microsoftSqlBackendMountPoint = SecretBackendDefaultMountPoints.MicrosoftSql, string wrapTimeToLive = null);
        
        /// <summary>
        /// Configures the connection information used to communicate with MySql.
        /// This API is a root protected call.
        /// </summary>
        /// <param name="mySqlConnectionInfo"><para>[required]</para>
        /// The MySql connection information.</param>
        /// <param name="mySqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MySql backend. Defaults to <see cref="SecretBackendType.MySql" />
        /// Provide a value only if you have customized the MySql mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MySqlConfigureConnectionAsync(MySqlConnectionInfo mySqlConnectionInfo, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql);

        /// <summary>
        /// Reads the connection information used to communicate with MySql.
        /// </summary>
        /// <param name="mySqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MySql backend. Defaults to <see cref="SecretBackendType.MySql" />
        /// Provide a value only if you have customized the MySql mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The connection information.
        /// </returns>
        Task<Secret<MySqlConnectionInfo>> MySqlReadConnectionInfoAsync(string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql, string wrapTimeToLive = null);

        /// <summary>
        /// Configures the lease settings for generated credentials.
        /// If not configured, leases default to 1 hour.
        /// This API is a root protected call.
        /// </summary>
        /// <param name="credentialLeaseSettings"><para>[required]</para>
        /// The credential lease settings.</param>
        /// <param name="mySqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MySql backend. Defaults to <see cref="SecretBackendType.MySql" />
        /// Provide a value only if you have customized the MySql mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MySqlConfigureCredentialLeaseSettingsAsync(CredentialLeaseSettings credentialLeaseSettings, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql);

        /// <summary>
        /// Queries the MySql credential lease settings.
        /// </summary>
        /// <param name="mySqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MySql backend. Defaults to <see cref="SecretBackendType.MySql" />
        /// Provide a value only if you have customized the MySql mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The lease settings.</returns>
        Task<Secret<CredentialLeaseSettings>> MySqlReadCredentialLeaseSettingsAsync(string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql, string wrapTimeToLive = null);

        /// <summary>
        /// Creates or updates a named MySql role.
        /// </summary>
        /// <param name="mySqlRoleName"><para>[required]</para>
        /// Name of the MySql role.</param>
        /// <param name="mySqlRoleDefinition"><para>[required]</para>
        /// The MySql role definition with the creation, rollback query and lease information.</param>
        /// <param name="mySqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MySql backend. Defaults to <see cref="SecretBackendType.MySql" />
        /// Provide a value only if you have customized the MySql mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MySqlWriteNamedRoleAsync(string mySqlRoleName, MySqlRoleDefinition mySqlRoleDefinition, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql);

        /// <summary>
        /// Queries a named MySql role definition
        /// </summary>
        /// <param name="mySqlRoleName"><para>[required]</para>
        /// Name of the MySql role.</param>
        /// <param name="mySqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MySql backend. Defaults to <see cref="SecretBackendType.MySql" />
        /// Provide a value only if you have customized the MySql mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the MySql role definition with the creation, rollback query and lease information.
        /// </returns>
        Task<Secret<MySqlRoleDefinition>> MySqlReadNamedRoleAsync(string mySqlRoleName, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql, string wrapTimeToLive = null);

        /// <summary>
        /// Returns a list of available roles. Only the role names are returned, not any values.
        /// </summary>
        /// <param name="mySqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MySql backend. Defaults to <see cref="SecretBackendType.MySql" />
        /// Provide a value only if you have customized the MySql mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// A list of available roles. Only the role names are returned, not any values.
        /// </returns>
        Task<Secret<ListInfo>> MySqlReadRoleListAsync(string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql, string wrapTimeToLive = null);

        /// <summary>
        /// Deletes a named MySql role definition
        /// </summary>
        /// <param name="mySqlRoleName"><para>[required]</para>
        /// Name of the MySql role.</param>
        /// <param name="mySqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MySql backend. Defaults to <see cref="SecretBackendType.MySql" />
        /// Provide a value only if you have customized the MySql mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task MySqlDeleteNamedRoleAsync(string mySqlRoleName, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql);

        /// <summary>
        /// Generates a new set of dynamic credentials based on the named role.
        /// </summary>
        /// <param name="mySqlRoleName"><para>[required]</para>
        /// Name of the MySql role.</param>
        /// <param name="mySqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the MySql backend. Defaults to <see cref="SecretBackendType.MySql" />
        /// Provide a value only if you have customized the MySql mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="UsernamePasswordCredentials" /> as the data.
        /// </returns>
        Task<Secret<UsernamePasswordCredentials>> MySqlGenerateDynamicCredentialsAsync(string mySqlRoleName, string mySqlBackendMountPoint = SecretBackendDefaultMountPoints.MySql, string wrapTimeToLive = null);

        /// <summary>
        /// Retrieves the CA certificate in raw DER-encoded form. 
        /// This is a bare endpoint that does not return a standard Vault data structure. 
        /// The CA certificate can be returned in DER or PEM format. 
        /// </summary>
        /// <param name="certificateFormat"><para>[optional]</para>
        /// The certificate format needed.
        /// Defaults to <see cref="CertificateFormat.der" /></param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The raw certificate data.
        /// </returns>
        Task<RawCertificateData> PKIReadCACertificateAsync(CertificateFormat certificateFormat = CertificateFormat.der, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Retrieves one of a selection of certificates.
        /// This is an unauthenticated call and does not use the credentials.
        /// </summary>
        /// <param name="predicate"><para>[required]</para>
        /// The predicate to select a certificate.
        /// Valid values: ca for the CA certificate, crl for the current CRL, or a serial number in either hyphen-separated or colon-separated octal format.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The secret with the raw certificate data.
        /// </returns>
        Task<Secret<RawCertificateData>> PKIReadCertificateAsync(string predicate, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Returns a list of the current certificates by serial number only.
        /// </summary>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>The list of the current certificates by serial number only.</returns>
        Task<Secret<ListInfo>> PKIReadCertificateListAsync(string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);
        
        /// <summary>
        /// Allows submitting the CA information for the backend via a PEM file containing the CA certificate and its private key, concatenated.
        /// Not needed if you are generating a self-signed root certificate, and not used if you have a signed intermediate CA certificate with a generated key.
        /// If you have already set a certificate and key, they will be overridden.
        /// </summary>
        /// <param name="pemBundle"><para>[required]</para>
        /// The private key and CA certificate contents concatenated in PEM format with newlines replaced with \n.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task PKIConfigureCACertificateAsync(string pemBundle, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Allows getting the duration for which the generated CRL should be marked valid.
        /// </summary>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The secret with the expiry data.
        /// </returns>
        Task<Secret<ExpiryData>> PKIReadCRLExpirationAsync(string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Allows setting the duration for which the generated CRL should be marked valid.
        /// </summary>
        /// <param name="expiry"><para>[required]</para>
        /// The time until expiration. Defaults to 72h.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task PKIWriteCRLExpirationAsync(string expiry = "72h", string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Fetch the URLs to be encoded in generated certificates.
        /// </summary>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The secret with the URLs.
        /// </returns>
        Task<Secret<CertificateEndpointData>> PKIReadCertificateEndpointsAsync(string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Allows setting the issuing certificate endpoints, CRL distribution points, and OCSP server endpoints that will be encoded into issued certificates.
        /// You can update any of the values at any time without affecting the other existing values.
        /// </summary>
        /// <param name="certificateEndpointOptions"><para>[required]</para>
        /// The certificate URLs to be encoded in generated certificates.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task PKIWriteCertificateEndpointsAsync(CertificateEndpointOptions certificateEndpointOptions, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Retrieves the current CRL in the raw requested format.
        /// This endpoint is suitable for usage in the CRL Distribution Points extension in a CA certificate.
        /// This is an unauthenticated call and does not use the credentials.
        /// </summary>
        /// <param name="certificateFormat"><para>[optional]</para>
        /// The certificate format needed.
        /// Defaults to <see cref="CertificateFormat.der" /></param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The raw certificate data.
        /// </returns>
        Task<RawCertificateData> PKIReadCRLCertificateAsync(CertificateFormat certificateFormat = CertificateFormat.der, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Forces a rotation of the CRL.
        /// This can be used by administrators to cut the size of the CRL if it contains a number of certificates that have now expired,
        /// but has not been rotated due to no further certificates being revoked.
        /// </summary>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// A status indicating if the rotation succeeded.
        /// </returns>
        Task<Secret<bool>> PKIRotateCRLAsync(string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Generates a new private key and a CSR for signing.
        /// If using Vault as a root, and for many other CAs, the various parameters on the final certificate are set at signing time and may or may not honor the parameters set here.
        /// This will overwrite any previously existing CA private key.
        /// If the 'ExportprivateKey' option is requested, the private key will be returned in the response;
        /// else the private key will not be returned and cannot be retrieved later.
        /// This is mostly meant as a helper function, and not all possible parameters that can be set in a CSR are supported.
        /// </summary>
        /// <param name="certificateSigningRequestOptions"><para>[required]</para>
        /// The certificate signing request options.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The secret with the raw CSR object.
        /// </returns>
        Task<Secret<RawCertificateSigningRequestData>> PKIGenerateIntermediateCACertificateSigningRequestAsync(CertificateSigningRequestOptions certificateSigningRequestOptions, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Allows submitting the signed CA certificate corresponding to a private key generated via <see cref="PKIGenerateIntermediateCACertificateSigningRequestAsync" />.
        /// The certificate should be submitted in PEM format
        /// </summary>
        /// <param name="certificateInPemFormat"><para>[required]</para>
        /// The certificate in pem format.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task PKISetSignedIntermediateCACertificateAsync(string certificateInPemFormat, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Generates a new set of credentials (private key and certificate) based on the role named in the endpoint.
        /// The issuing CA certificate is returned as well, so that only the root CA need be in a client's trust store.
        /// The private key is not stored.
        /// If you do not save the private key, you will need to request a new certificate.
        /// </summary>
        /// <param name="pkiRoleName"><para>[required]</para>
        /// Name of the PKI role.</param>
        /// <param name="certificateCredentialRequestOptions"><para>[required]</para>
        /// The certificate credential request options.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The secret with the new Certificate credentials.
        /// </returns>
        Task<Secret<CertificateCredentials>> PKIGenerateDynamicCredentialsAsync(string pkiRoleName, CertificateCredentialsRequestOptions certificateCredentialRequestOptions, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Revokes a certificate using its serial number.
        /// This is an alternative option to the standard method of revoking using Vault lease IDs.
        /// A successful revocation will rotate the CRL.
        /// </summary>
        /// <param name="serialNumber">The serial number of the certificate to revoke, in hyphen-separated or colon-separated octal.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The secret with the revocation data.
        /// </returns>
        Task<Secret<RevocationData>> PKIRevokeCertificateAsync(string serialNumber, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Creates or updates the role definition.
        /// If a client requests a certificate that is not allowed by the CN policy in the role, the request is denied.
        /// </summary>
        /// <param name="pkiRoleName"><para>[required]</para>
        /// Name of the PKI role.</param>
        /// <param name="certificateRoleDefinition"><para>[optional]</para>
        /// The certificate role definition.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task PKIWriteNamedRoleAsync(string pkiRoleName, CertificateRoleDefinition certificateRoleDefinition = null, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Queries the named PKI role definition.
        /// </summary>
        /// <param name="pkiRoleName"><para>[required]</para>
        /// Name of the PKI role.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The secret with the certificate role definition.
        /// </returns>
        Task<Secret<CertificateRoleDefinition>> PKIReadNamedRoleAsync(string pkiRoleName, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Queries the list of available roles. Only the role names are returned, not any values.
        /// </summary>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The secret with the role names.
        /// </returns>
        Task<Secret<ListInfo>> PKIReadRoleListAsync(string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Deletes the named PKI role definition.
        /// </summary>
        /// <param name="pkiRoleName"><para>[required]</para>
        /// Name of the PKI role.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task PKIDeleteNamedRoleAsync(string pkiRoleName, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Generates a new self-signed CA certificate and private key.
        /// This will overwrite any previously-existing private key and certificate.
        /// If the private key needs to be exported, the private key will be returned in the response;
        /// if not the private key will not be returned and cannot be retrieved later.
        /// Distribution points use the values set via <see cref="PKIWriteCertificateEndpointsAsync" />
        /// As with other issued certificates, Vault will automatically revoke the generated root at the end of its lease period;
        /// the CA certificate will sign its own CRL.
        /// </summary>
        /// <param name="rootCertificateRequestOptions"><para>[required]</para>
        /// The root certificate request options.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The secret with the root certificate data.
        /// </returns>
        Task<Secret<RootCertificateData>> PKIGenerateRootCACertificateAsync(RootCertificateRequestOptions rootCertificateRequestOptions, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Uses the configured CA certificate to issue a certificate with appropriate values for acting as an intermediate CA.
        /// Distribution points use the values set via <see cref="PKIWriteCertificateEndpointsAsync" />.
        /// Values set in the CSR are ignored unless CSR values flag is set to true, in which case the values from the CSR are used verbatim.
        /// </summary>
        /// <param name="intermediateCertificateRequestOptions"><para>[required]</para>
        /// The intermediate certificate request options.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The secret with the intermediate certificate data.
        /// </returns>
        Task<Secret<IntermediateCertificateData>> PKISignIntermediateCACertificateAsync(IntermediateCertificateRequestOptions intermediateCertificateRequestOptions, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Signs a new certificate based upon the provided CSR and the supplied parameters, subject to the restrictions contained in the role named in the endpoint.
        /// The issuing CA certificate is returned as well, so that only the root CA need be in a client's trust store.
        /// </summary>
        /// <param name="pkiRoleName"><para>[required]</para>
        /// Name of the PKI role.</param>
        /// <param name="newCertificateSigningRequestOptions"><para>[required]</para>
        /// The new certificate request options.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The secret with the intermediate certificate data.
        /// </returns>
        Task<Secret<NewCertificateData>> PKISignCertificateAsync(string pkiRoleName, NewCertificateSigningRequestOptions newCertificateSigningRequestOptions, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Signs a new certificate based upon the provided CSR.
        /// Values are taken verbatim from the CSR; the only restriction is that this endpoint will refuse to issue an intermediate CA certificate
        /// This is a potentially dangerous endpoint and only highly trusted users should have access.
        /// </summary>
        /// <param name="verbatimCertificateSigningRequestOptions"><para>[required]</para>
        /// The verbatim certificate request options.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The secret with the new certificate data.
        /// </returns>
        Task<Secret<NewCertificateData>> PKISignCertificateVerbatimAsync(VerbatimCertificateSigningRequestOptions verbatimCertificateSigningRequestOptions, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Allows tidying up the backend storage and/or CRL by removing certificates that have expired 
        /// and are past a certain buffer period beyond their expiration time.
        /// </summary>
        /// <param name="tidyRequestOptions"><para>[optional]</para>
        /// The tidy request options.</param>
        /// <param name="pkiBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PKI backend. Defaults to <see cref="SecretBackendType.PKI" />
        /// Provide a value only if you have customized the PKI mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task PKITidyAsync(TidyRequestOptions tidyRequestOptions = null, string pkiBackendMountPoint = SecretBackendDefaultMountPoints.PKI);

        /// <summary>
        /// Configures the connection information used to communicate with PostgreSql.
        /// </summary>
        /// <param name="postgreSqlConnectionInfo"><para>[required]</para>
        /// The PostgreSql connection information.</param>
        /// <param name="postgreSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PostgreSql backend. Defaults to <see cref="SecretBackendType.PostgreSql" />
        /// Provide a value only if you have customized the PostgreSql mount point.</param>
        /// <returns>   
        /// The task.
        /// </returns>
        Task PostgreSqlConfigureConnectionAsync(PostgreSqlConnectionInfo postgreSqlConnectionInfo, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql);

        /// <summary>
        /// Reads the connection information used to communicate with PostgreSql.
        /// </summary>
        /// <param name="postgreSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PostgreSql backend. Defaults to <see cref="SecretBackendType.PostgreSql" />
        /// Provide a value only if you have customized the PostgreSql mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The connection information.
        /// </returns>
        Task<Secret<PostgreSqlConnectionInfo>> PostgreSqlReadConnectionInfoAsync(string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql, string wrapTimeToLive = null);

        /// <summary>
        /// Configures the lease settings for generated credentials. If not configured, leases default to 1 hour. 
        /// This API is a root protected call.
        /// </summary>
        /// <param name="credentialLeaseSettings"><para>[required]</para>
        /// The credential lease settings.</param>
        /// <param name="postgreSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PostgreSql backend. Defaults to <see cref="SecretBackendType.PostgreSql" />
        /// Provide a value only if you have customized the PostgreSql mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task PostgreSqlConfigureCredentialLeaseSettingsAsync(CredentialLeaseSettings credentialLeaseSettings, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql);

        /// <summary>
        /// Queries the PostgreSql credential lease settings.
        /// </summary>
        /// <param name="postgreSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PostgreSql backend. Defaults to <see cref="SecretBackendType.PostgreSql" />
        /// Provide a value only if you have customized the PostgreSql mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The lease settings.</returns>
        Task<Secret<CredentialLeaseSettings>> PostgreSqlReadCredentialLeaseSettingsAsync(string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql, string wrapTimeToLive = null);

        /// <summary>
        /// Creates or updates a named PostgreSql role definition.
        /// </summary>
        /// <param name="postgreSqlRoleName"><para>[required]</para>
        /// Name of the PostgreSql role.</param>
        /// <param name="postgreSqlRoleDefinition"><para>[required]</para>
        /// The PostgreSql role definition with the creation, rollback query and lease information.</param>
        /// <param name="postgreSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PostgreSql backend. Defaults to <see cref="SecretBackendType.PostgreSql" />
        /// Provide a value only if you have customized the PostgreSql mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task PostgreSqlWriteNamedRoleAsync(string postgreSqlRoleName, PostgreSqlRoleDefinition postgreSqlRoleDefinition, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql);

        /// <summary>
        /// Queries a named PostgreSql role definition
        /// </summary>
        /// <param name="postgreSqlRoleName"><para>[required]</para>
        /// Name of the PostgreSql role.</param>
        /// <param name="postgreSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PostgreSql backend. Defaults to <see cref="SecretBackendType.PostgreSql" />
        /// Provide a value only if you have customized the PostgreSql mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the PostgreSql role definition with the creation, rollback query and lease information.
        /// </returns>
        Task<Secret<PostgreSqlRoleDefinition>> PostgreSqlReadNamedRoleAsync(string postgreSqlRoleName, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql, string wrapTimeToLive = null);

        /// <summary>
        /// Returns a list of available roles. Only the role names are returned, not any values.
        /// </summary>
        /// <param name="postgreSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PostgreSql backend. Defaults to <see cref="SecretBackendType.PostgreSql" />
        /// Provide a value only if you have customized the PostgreSql mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// A list of available roles. Only the role names are returned, not any values.
        /// </returns>
        Task<Secret<ListInfo>> PostgreSqlReadRoleListAsync(string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql, string wrapTimeToLive = null);

        /// <summary>
        /// Deletes a named PostgreSql role definition
        /// </summary>
        /// <param name="postgreSqlRoleName"><para>[required]</para>
        /// Name of the PostgreSql role.</param>
        /// <param name="postgreSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PostgreSql backend. Defaults to <see cref="SecretBackendType.PostgreSql" />
        /// Provide a value only if you have customized the PostgreSql mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task PostgreSqlDeleteNamedRoleAsync(string postgreSqlRoleName, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql);

        /// <summary>
        /// Generates a new set of dynamic credentials based on the named role.
        /// </summary>
        /// <param name="postgreSqlRoleName"><para>[required]</para>
        /// Name of the PostgreSql role.</param>
        /// <param name="postgreSqlBackendMountPoint"><para>[optional]</para>
        /// The mount point for the PostgreSql backend. Defaults to <see cref="SecretBackendType.PostgreSql" />
        /// Provide a value only if you have customized the PostgreSql mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="UsernamePasswordCredentials" /> as the data.
        /// </returns>
        Task<Secret<UsernamePasswordCredentials>> PostgreSqlGenerateDynamicCredentialsAsync(string postgreSqlRoleName, string postgreSqlBackendMountPoint = SecretBackendDefaultMountPoints.PostgreSql, string wrapTimeToLive = null);

        /// <summary>
        /// Configures the connection information used to communicate with RabbitMQ.
        /// This API is a root protected call.
        /// </summary>
        /// <param name="rabbitMQConnectionInfo"><para>[required]</para>
        /// The RabbitMQ connection information.</param>
        /// <param name="rabbitMQBackendMountPoint"><para>[optional]</para>
        /// The mount point for the RabbitMQ backend. Defaults to <see cref="SecretBackendType.RabbitMQ" />
        /// Provide a value only if you have customized the RabbitMQ mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task RabbitMQConfigureConnectionAsync(RabbitMQConnectionInfo rabbitMQConnectionInfo, string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ);

        ///// <summary>
        ///// Reads the connection information used to communicate with RabbitMQ.
        ///// </summary>
        ///// <param name="rabbitMQBackendMountPoint"><para>[optional]</para>
        ///// The mount point for the RabbitMQ backend. Defaults to <see cref="SecretBackendType.RabbitMQ" />
        ///// Provide a value only if you have customized the RabbitMQ mount point.</param>
        ///// <returns>
        ///// The connection information.
        ///// </returns>
        //Task<Secret<RabbitMQConnectionInfo>> RabbitMQReadConnectionInfoAsync(string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ);

        /// <summary>
        /// Configures the lease settings for generated credentials. If not configured, leases default to 1 hour. 
        /// This API is a root protected call.
        /// </summary>
        /// <param name="credentialTimeToLiveSettings"><para>[required]</para>
        /// The credential lease settings.</param>
        /// <param name="rabbitMQBackendMountPoint"><para>[optional]</para>
        /// The mount point for the RabbitMQ backend. Defaults to <see cref="SecretBackendType.RabbitMQ" />
        /// Provide a value only if you have customized the RabbitMQ mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task RabbitMQConfigureCredentialLeaseSettingsAsync(CredentialTimeToLiveSettings credentialTimeToLiveSettings, string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ);

        /// <summary>
        /// Queries the RabbitMQ credential lease settings.
        /// </summary>
        /// <param name="rabbitMQBackendMountPoint"><para>[optional]</para>
        /// The mount point for the RabbitMQ backend. Defaults to <see cref="SecretBackendType.RabbitMQ" />
        /// Provide a value only if you have customized the RabbitMQ mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The lease settings.</returns>
        Task<Secret<CredentialTimeToLiveSettings>> RabbitMQReadCredentialLeaseSettingsAsync(string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ, string wrapTimeToLive = null);

        /// <summary>
        /// Creates or updates a named RabbitMQ role.
        /// </summary>
        /// <param name="rabbitMQRoleName"><para>[required]</para>
        /// Name of the RabbitMQ role.</param>
        /// <param name="rabbitMQRoleDefinition"><para>[required]</para>
        /// The RabbitMQ role definition with the creation, rollback query and lease information.</param>
        /// <param name="rabbitMQBackendMountPoint"><para>[optional]</para>
        /// The mount point for the RabbitMQ backend. Defaults to <see cref="SecretBackendType.RabbitMQ" />
        /// Provide a value only if you have customized the RabbitMQ mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task RabbitMQWriteNamedRoleAsync(string rabbitMQRoleName, RabbitMQRoleDefinition rabbitMQRoleDefinition, string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ);

        /// <summary>
        /// Queries a named RabbitMQ role definition
        /// </summary>
        /// <param name="rabbitMQRoleName"><para>[required]</para>
        /// Name of the RabbitMQ role.</param>
        /// <param name="rabbitMQBackendMountPoint"><para>[optional]</para>
        /// The mount point for the RabbitMQ backend. Defaults to <see cref="SecretBackendType.RabbitMQ" />
        /// Provide a value only if you have customized the RabbitMQ mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the RabbitMQ role definition with the creation, rollback query and lease information.
        /// </returns>
        Task<Secret<RabbitMQRoleDefinition>> RabbitMQReadNamedRoleAsync(string rabbitMQRoleName, string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ, string wrapTimeToLive = null);

        /// <summary>
        /// Returns a list of available roles. Only the role names are returned, not any values.
        /// </summary>
        /// <param name="rabbitMQBackendMountPoint"><para>[optional]</para>
        /// The mount point for the RabbitMQ backend. Defaults to <see cref="SecretBackendType.RabbitMQ" />
        /// Provide a value only if you have customized the RabbitMQ mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// A list of available roles. Only the role names are returned, not any values.
        /// </returns>
        Task<Secret<ListInfo>> RabbitMQReadRoleListAsync(string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ, string wrapTimeToLive = null);

        /// <summary>
        /// Deletes a named RabbitMQ role definition
        /// </summary>
        /// <param name="rabbitMQRoleName"><para>[required]</para>
        /// Name of the RabbitMQ role.</param>
        /// <param name="rabbitMQBackendMountPoint"><para>[optional]</para>
        /// The mount point for the RabbitMQ backend. Defaults to <see cref="SecretBackendType.RabbitMQ" />
        /// Provide a value only if you have customized the RabbitMQ mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task RabbitMQDeleteNamedRoleAsync(string rabbitMQRoleName, string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ);

        /// <summary>
        /// Generates a new set of dynamic credentials based on the named role.
        /// </summary>
        /// <param name="rabbitMQRoleName"><para>[required]</para>
        /// Name of the RabbitMQ role.</param>
        /// <param name="rabbitMQBackendMountPoint"><para>[optional]</para>
        /// The mount point for the RabbitMQ backend. Defaults to <see cref="SecretBackendType.RabbitMQ" />
        /// Provide a value only if you have customized the RabbitMQ mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="UsernamePasswordCredentials" /> as the data.
        /// </returns>
        Task<Secret<UsernamePasswordCredentials>> RabbitMQGenerateDynamicCredentialsAsync(string rabbitMQRoleName, string rabbitMQBackendMountPoint = SecretBackendDefaultMountPoints.RabbitMQ, string wrapTimeToLive = null);

        /// <summary>
        /// Creates or updates a named key.
        /// This API is a root protected call.
        /// </summary>
        /// <param name="sshKeyName"><para>[required]</para>
        /// Name of the SSH key.</param>
        /// <param name="sshPrivateKey"><para>[required]</para>
        /// SSH private key with appropriate privileges on remote hosts.</param>
        /// <param name="sshBackendMountPoint"><para>[optional]</para>
        /// The mount point for the SSH backend. Defaults to <see cref="SecretBackendType.SSH" />
        /// Provide a value only if you have customized the SSH mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task SSHWriteNamedKeyAsync(string sshKeyName, string sshPrivateKey, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH);

        /// <summary>
        /// Deletes a named key.
        /// This API is a root protected call.
        /// </summary>
        /// <param name="sshKeyName"><para>[required]</para>
        /// Name of the SSH key.</param>
        /// <param name="sshBackendMountPoint"><para>[optional]</para>
        /// The mount point for the SSH backend. Defaults to <see cref="SecretBackendType.SSH" />
        /// Provide a value only if you have customized the SSH mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task SSHDeleteNamedKeyAsync(string sshKeyName, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH);

        /// <summary>
        /// Creates or updates a named role.
        /// </summary>
        /// <param name="sshRoleName"><para>[required]</para>
        /// Name of the SSH Role.</param>
        /// <param name="sshRoleDefinition"><para>[required]</para>
        /// The SSH role definition.</param>
        /// <param name="sshBackendMountPoint"><para>[optional]</para>
        /// The mount point for the SSH backend. Defaults to <see cref="SecretBackendType.SSH" />
        /// Provide a value only if you have customized the SSH mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task SSHWriteNamedRoleAsync(string sshRoleName, SSHRoleDefinition sshRoleDefinition, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH);

        /// <summary>
        /// Queries a named role.
        /// </summary>
        /// <param name="sshRoleName"><para>[required]</para>
        /// Name of the SSH Role.</param>
        /// <param name="sshBackendMountPoint"><para>[optional]</para>
        /// The mount point for the SSH backend. Defaults to <see cref="SecretBackendType.SSH" />
        /// Provide a value only if you have customized the SSH mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the role definition.
        /// </returns>
        Task<Secret<SSHRoleDefinition>> SSHReadNamedRoleAsync(string sshRoleName, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH, string wrapTimeToLive = null);

        /// <summary>
        /// Returns a list of available roles. Only the role names are returned, not any values.
        /// </summary>
        /// <param name="sshBackendMountPoint"><para>[optional]</para>
        /// The mount point for the SSH backend. Defaults to <see cref="SecretBackendType.SSH" />
        /// Provide a value only if you have customized the SSH mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The list of role names.
        /// </returns>
        Task<Secret<ListInfo>> SSHReadRoleListAsync(string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH, string wrapTimeToLive = null);

        /// <summary>
        /// Deletes a named role.
        /// </summary>
        /// <param name="sshRoleName"><para>[required]</para>
        /// Name of the SSH Role.</param>
        /// <param name="sshBackendMountPoint"><para>[optional]</para>
        /// The mount point for the SSH backend. Defaults to <see cref="SecretBackendType.SSH" />
        /// Provide a value only if you have customized the SSH mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task SSHDeleteNamedRoleAsync(string sshRoleName, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH);

        /// <summary>
        /// Returns the list of configured zero-address roles.
        /// </summary>
        /// <param name="sshBackendMountPoint"><para>[optional]</para>
        /// The mount point for the SSH backend. Defaults to <see cref="SecretBackendType.SSH" />
        /// Provide a value only if you have customized the SSH mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The roles.
        /// </returns>
        Task<Secret<SSHRoleData>> SSHReadZeroAddressRolesAsync(string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH, string wrapTimeToLive = null);

        /// <summary>
        /// Configures zero-address roles.
        /// </summary>
        /// <param name="roleNames"><para>[required]</para>
        /// A string containing comma separated list of role names which allows credentials to be requested for any IP address. 
        /// CIDR blocks previously registered under these roles will be ignored.</param>
        /// <param name="sshBackendMountPoint"><para>[optional]</para>
        /// The mount point for the SSH backend. Defaults to <see cref="SecretBackendType.SSH" />
        /// Provide a value only if you have customized the SSH mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task SSHConfigureZeroAddressRolesAsync(string roleNames, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH);

        /// <summary>
        /// Deletes the zero-address roles configuration.
        /// </summary>
        /// <param name="sshBackendMountPoint"><para>[optional]</para>
        /// The mount point for the SSH backend. Defaults to <see cref="SecretBackendType.SSH" />
        /// Provide a value only if you have customized the SSH mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task SSHDeleteZeroAddressRolesAsync(string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH);

        /// <summary>
        /// Generates a dynamic SSH credentials for a specific username and IP Address based on the named role.
        /// </summary>
        /// <param name="sshRoleName"><para>[required]</para>
        /// Name of the SSH Role.</param>
        /// <param name="ipAddress"><para>[required]</para>
        /// The ip address of the remote host.</param>
        /// <param name="username"><para>[optional]</para>
        /// The username on the remote host.</param>
        /// <param name="sshBackendMountPoint"><para>[optional]</para>
        /// The mount point for the SSH backend. Defaults to <see cref="SecretBackendType.SSH" />
        /// Provide a value only if you have customized the SSH mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the SSH credentials.
        /// </returns>
        Task<Secret<SSHCredentials>>  SSHGenerateDynamicCredentialsAsync(string sshRoleName, string ipAddress, string username = null, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH, string wrapTimeToLive = null);

        /// <summary>
        /// Lists all of the roles with which the given IP is associated.
        /// </summary>
        /// <param name="ipAddress"><para>[required]</para>
        /// The ip address of the remote host.</param>
        /// <param name="sshBackendMountPoint"><para>[optional]</para>
        /// The mount point for the SSH backend. Defaults to <see cref="SecretBackendType.SSH" />
        /// Provide a value only if you have customized the SSH mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with list of roles
        /// </returns>
        Task<Secret<SSHRoleData>> SSHLookupRolesAsync(string ipAddress, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH, string wrapTimeToLive = null);

        /// <summary>
        /// Verifies if the given OTP is valid.
        /// This is an unauthenticated endpoint.
        /// </summary>
        /// <param name="otp"><para>[required]</para>
        /// One-Time-Key that needs to be validated.</param>
        /// <param name="sshBackendMountPoint"><para>[optional]</para>
        /// The mount point for the SSH backend. Defaults to <see cref="SecretBackendType.SSH" />
        /// Provide a value only if you have customized the SSH mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the OTP verification data.
        /// </returns>
        Task<Secret<SSHOTPVerificationData>>  SSHVerifyOTPAsync(string otp, string sshBackendMountPoint = SecretBackendDefaultMountPoints.SSH, string wrapTimeToLive = null);

        /// <summary>
        /// Creates a new named encryption key.
        /// This API is a root protected call.
        /// </summary>
        /// <param name="encryptionKeyName"><para>[required]</para>
        /// Name of the encryption key.</param>
        /// <param name="transitKeyType"><para>[required]</para>
        /// The type of key to create. The currently-supported types are: 
        /// <see cref="TransitKeyType.aes256_gcm96"/> : AES-256 wrapped with GCM using a 12-byte nonce size(symmetric)
        /// <see cref="TransitKeyType.ecdsa_p256"/> : ECDSA using the P-256 elliptic curve(asymmetric)
        /// Defaults to <see cref="TransitKeyType.aes256_gcm96"/>.</param>
        /// <param name="mustUseKeyDerivation"><para>[optional]</para>
        /// Boolean flag indicating if key derivation MUST be used.
        /// If enabled, all encrypt/decrypt requests to this named key must provide a context which is used for key derivation.
        /// Defaults to false.</param>
        /// <param name="doConvergentEncryption"><para>[optional]</para>
        /// Boolean flag when set, the key will support convergent encryption, where the same plaintext creates the same ciphertext. 
        /// This requires <see cref="mustUseKeyDerivation"/> to be set to true. 
        /// When enabled, each encryption(/decryption/rewrap/datakey) operation will require a nonce value to be specified. 
        /// Note that while this is useful for particular situations, all nonce values used with a given context value 
        /// must be unique or it will compromise the security of your key. 
        /// A common way to use this will be to generate a unique identifier for the given data (for instance, a SHA-512 sum), 
        /// then separate the bytes so that twelve bytes are used as the nonce and the remaining as the context, 
        /// ensuring that all bits of unique identity are used as a part of the encryption operation. 
        /// Defaults to false.</param>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the Transit mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task TransitCreateEncryptionKeyAsync(string encryptionKeyName, TransitKeyType transitKeyType = TransitKeyType.aes256_gcm96,  bool mustUseKeyDerivation = false, bool doConvergentEncryption = false, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit);

        /// <summary>
        /// Returns information about a named encryption key.
        /// This API is a root protected call.
        /// </summary>
        /// <param name="encryptionKeyName"><para>[required]</para>
        /// Name of the encryption key.</param>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the Transit mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the key information.
        /// </returns>
        Task<Secret<TransitEncryptionKeyInfo>> TransitGetEncryptionKeyInfoAsync(string encryptionKeyName, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null);

        /// <summary>
        /// Returns a list of keys. Only the key names are returned.
        /// </summary>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the Transit mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The list of key names.</returns>
        Task<Secret<ListInfo>> TransitGetEncryptionKeyListAsync(string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null);

        /// <summary>
        /// Deletes a named encryption key.
        /// This API is a root protected call.
        /// It will no longer be possible to decrypt any data encrypted with the named key.
        /// Because this is a potentially catastrophic operation, the 'DeletionAllowed' tunable must be set in the key's configuration.
        /// </summary>
        /// <param name="encryptionKeyName"><para>[required]</para>
        /// Name of the encryption key.</param>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the Transit mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task TransitDeleteEncryptionKeyAsync(string encryptionKeyName, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit);

        /// <summary>
        /// Allows tuning configuration values for a given key. (These values are returned during a read operation on the named key.)
        /// This API is a root protected call.
        /// </summary>
        /// <param name="encryptionKeyName"><para>[required]</para>
        /// Name of the encryption key.</param>
        /// <param name="minimumDecryptionVersion"><para>[optional]</para>
        /// The minimum version of ciphertext allowed to be decrypted.
        /// Adjusting this as part of a key rotation policy can prevent old copies of ciphertext from being decrypted,
        /// should they fall into the wrong hands.
        /// Defaults to 0.</param>
        /// <param name="isDeletionAllowed"><para>[optional]</para>
        /// When set, the key is allowed to be deleted. Defaults to false.</param>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the Transit mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task TransitConfigureEncryptionKeyAsync(string encryptionKeyName, int minimumDecryptionVersion = 0, bool isDeletionAllowed = false, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit);

        /// <summary>
        /// Rotates the version of the named key.
        /// After rotation, new plaintext requests will be encrypted with the new version of the key.
        /// To upgrade ciphertext to be encrypted with the latest version of the key,
        /// use the <see cref="TransitRewrapWithLatestEncryptionKeyAsync" /> endpoint.
        /// </summary>
        /// <param name="encryptionKeyName"><para>[required]</para>
        /// Name of the encryption key.</param>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the Transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the Transit mount point.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task TransitRotateEncryptionKeyAsync(string encryptionKeyName, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit);

        /// <summary>
        /// Encrypts the provided plaintext using the named key.
        /// This path supports the create and update policy capabilities as follows: 
        /// if the user has the create capability for this endpoint in their policies, 
        /// and the key does not exist, it will be upserted with default values 
        /// (whether the key requires derivation depends on whether the context parameter is empty or not). 
        /// If the user only has update capability and the key does not exist, an error will be returned.
        /// </summary>
        /// <param name="encryptionKeyName"><para>[required]</para>
        /// Name of the key used to encrypt the text.</param>
        /// <param name="base64EncodedPlainText"><para>[required]</para>
        /// The plaintext to encrypt, provided as base64 encoded.</param>
        /// <param name="base64EncodedKeyDerivationContext"><para>[optional]</para>
        /// The key derivation context, provided as base64 encoded. Must be provided if derivation is enabled.</param>
        /// <param name="convergentEncryptionBase64EncodedNonce"><para>[optional]</para>
        /// The nonce value, provided as base64 encoded. Must be provided if convergent encryption is enabled for this key. 
        /// The value must be exactly 96 bits (12 bytes) long and the user must ensure that for any given context 
        /// (and thus, any given encryption key) this nonce value is never reused.</param>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with cipher text.
        /// </returns>
        Task<Secret<CipherTextData>> TransitEncryptAsync(string encryptionKeyName, string base64EncodedPlainText, string base64EncodedKeyDerivationContext = null, string convergentEncryptionBase64EncodedNonce = null, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null);

        /// <summary>
        /// Decrypts the provided ciphertext using the named key.
        /// </summary>
        /// <param name="encryptionKeyName"><para>[required]</para>
        /// Name of the key used to decrypt the text.</param>
        /// <param name="cipherText"><para>[required]</para>
        /// The ciphertext to decrypt, provided as returned by encrypt.</param>
        /// <param name="base64EncodedKeyDerivationContext"><para>[optional]</para>
        /// The key derivation context, provided as base64 encoded. Must be provided if derivation is enabled.</param>
        /// <param name="convergentEncryptionBase64EncodedNonce"><para>[optional]</para>
        /// The nonce value used during encryption, provided as base64 encoded. 
        /// Must be provided if convergent encryption is enabled for this key.</param>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with plain text.
        /// </returns>
        Task<Secret<PlainTextData>> TransitDecryptAsync(string encryptionKeyName, string cipherText, string base64EncodedKeyDerivationContext = null, string convergentEncryptionBase64EncodedNonce = null, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null);

        /// <summary>
        /// Transits the rewrap with latest encryption key asynchronous.
        /// </summary>
        /// <param name="encryptionKeyName"><para>[required]</para>
        /// Name of the encryption key.</param>
        /// <param name="cipherText"><para>[required]</para>
        /// The ciphertext to decrypt, provided as returned by encrypt.</param>
        /// <param name="base64EncodedKeyDerivationContext"><para>[optional]</para>
        /// The key derivation context, provided as base64 encoded. Must be provided if derivation is enabled.</param>
        /// <param name="convergentEncryptionBase64EncodedNonce"><para>[optional]</para>
        /// The nonce value used during encryption, provided as base64 encoded. 
        /// Must be provided if convergent encryption is enabled for this key.</param>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with cipher text.
        /// </returns>
        Task<Secret<CipherTextData>> TransitRewrapWithLatestEncryptionKeyAsync(string encryptionKeyName, string cipherText, string base64EncodedKeyDerivationContext = null, string convergentEncryptionBase64EncodedNonce = null, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null);

        /// <summary>
        /// Generate a new high-entropy key and the valued encrypted with the named key.
        /// Optionally return the plaintext of the key as well.
        /// As a result, you can use Vault ACL policies to control whether a user is allowed to retrieve the plaintext value of a key.
        /// This is useful if you want an untrusted user or operation to generate keys that are then made available to trusted users.
        /// </summary>
        /// <param name="encryptionKeyName"><para>[required]</para>
        /// Name of the encryption key.</param>
        /// <param name="returnKeyAsPlainText"><para>[optional]</para>
        /// If true, the plaintext key will be returned along with the ciphertext.
        /// If false, only the ciphertext value will be returned.
        /// Defaults to false.</param>
        /// <param name="base64EncodedKeyDerivationContext"><para>[optional]</para>
        /// The key derivation context, provided as base64 encoded. Must be provided if derivation is enabled.</param>
        /// <param name="convergentEncryptionBase64EncodedNonce"><para>[optional]</para>
        /// The nonce value, provided as base64 encoded. Must be provided if convergent encryption is enabled for this key. 
        /// The value must be exactly 96 bits (12 bytes) long and the user must ensure that for any given context 
        /// (and thus, any given encryption key) this nonce value is never reused.</param>
        /// <param name="keyBits"><para>[optional]</para>
        /// The number of bits in the desired key.
        /// Can be 128, 256, or 512; if not given, defaults to 256.</param>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the cipher text key.
        /// If <see cref="returnKeyAsPlainText" /> is true, the plaintext key will be returned along with the ciphertext.
        /// If false, only the ciphertext value will be returned.
        /// </returns>
        Task<Secret<TransitKeyData>> TransitCreateDataKeyAsync(string encryptionKeyName, bool returnKeyAsPlainText = false, string base64EncodedKeyDerivationContext = null, string convergentEncryptionBase64EncodedNonce = null, int keyBits = 256, string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null);

        /// <summary>
        /// Return high-quality random bytes of the specified length.
        /// </summary>
        /// <param name="bytesToReturn"><para>[optional]</para>
        /// The number of bytes to return. Defaults to 32 (256 bits). </param>
        /// <param name="format"><para>[optional]</para>
        ///  The output encoding; can be either hex or base64. Defaults to base64.
        /// </param>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>Random bytes in the field 'random_bytes'.</returns>
        Task<Secret<dynamic>> TransitGenerateRandomBytes(int bytesToReturn = 32, string format = "base64", string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null);

        /// <summary>
        /// Returns the hash of given data using the specified algorithm. 
        /// </summary>
        /// <param name="base64EncodedInput"><para>[required]</para>
        /// The base64 encoded input.</param>
        /// <param name="algorithm"><para>[optional]</para>The hash algorithm to use. 
        /// Currently-supported algorithms are:
        /// sha2-224
        /// sha2-256
        /// sha2-384
        /// sha2-512
        /// Defaults to sha2-256.</param>
        /// <param name="format"><para>[optional]</para>
        ///  The output encoding; can be either hex or base64. Defaults to base64.
        /// </param>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The hashed value in the field 'sum'.</returns>
        Task<Secret<dynamic>> TransitHashInput(string base64EncodedInput, string algorithm = "sha2-256", string format = "base64", string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null);

        /// <summary>
        /// Returns the digest of given data using the specified hash algorithm and the named key. 
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// The key can be of any type supported by transit; the raw key will be marshalled into bytes 
        /// to be used for the HMAC function. 
        /// If the key is of a type that supports rotation, the latest (current) version will be used.</param>
        /// <param name="base64EncodedInput"><para>[required]</para>
        /// The base64 encoded input.</param>
        /// <param name="algorithm"><para>[optional]</para>The hash algorithm to use. 
        /// Currently-supported algorithms are:
        /// sha2-224
        /// sha2-256
        /// sha2-384
        /// sha2-512
        /// Defaults to sha2-256.</param>
        /// <param name="format"><para>[optional]</para>
        ///  The output encoding; can be either hex or base64. Defaults to base64.
        /// </param>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The hashed value in the field 'hmac'.</returns>
        Task<Secret<dynamic>> TransitDigestInput(string keyName, string base64EncodedInput, string algorithm = "sha2-256", string format = "base64", string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null);

        /// <summary>
        /// Returns the cryptographic signature of the given data using the named key and the specified hash algorithm.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        ///  The key must be of a type that supports signing.</param>
        /// <param name="base64EncodedInput"><para>[required]</para>
        /// The base64 encoded input.</param>
        /// <param name="algorithm"><para>[optional]</para>The hash algorithm to use. 
        /// Currently-supported algorithms are:
        /// sha2-224
        /// sha2-256
        /// sha2-384
        /// sha2-512
        /// Defaults to sha2-256.</param>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The signed value in the field 'signature'.</returns>
        Task<Secret<dynamic>> TransitSignInput(string keyName, string base64EncodedInput, string algorithm = "sha2-256", string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null);

        /// <summary>
        /// Returns whether the provided signature is valid for the given data.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        ///  The key must be of a type that supports signing.</param>
        /// <param name="base64EncodedInput"><para>[required]</para>
        /// The base64 encoded input.</param>
        /// <param name="signature"><para>[optional]</para>
        /// The signature string. Either this must be supplied or <see cref="hmac"/> must be supplied.</param>
        /// <param name="hmac"><para>[optional]</para>
        /// The hmac string. Either this must be supplied or <see cref = "signature" /> must be supplied.</param>
        /// <param name="algorithm"><para>[optional]</para>The hash algorithm to use. 
        /// Currently-supported algorithms are:
        /// sha2-224
        /// sha2-256
        /// sha2-384
        /// sha2-512
        /// Defaults to sha2-256.</param>
        /// <param name="transitBackendMountPoint"><para>[optional]</para>
        /// The mount point for the transit backend. Defaults to <see cref="SecretBackendType.Transit" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>The boolean verification result value in the field 'valid'.</returns>
        Task<Secret<dynamic>> TransitVerifySignature(string keyName, string base64EncodedInput, string signature = null, string hmac = null, string algorithm = "sha2-256", string transitBackendMountPoint = SecretBackendDefaultMountPoints.Transit, string wrapTimeToLive = null);

        /// <summary>
        /// Creates an App Id that associates with the policy value.
        /// The <see cref="displayName"/> sets the display name for audit logs and secrets. 
        /// </summary>
        /// <param name="appId">
        /// <para>[required]</para>
        /// The application identifier.</param>
        /// <param name="policyValue">
        /// <para>[required]</para>
        /// The policy value.</param>
        /// <param name="displayName">
        /// <para>[optional]</para>
        /// The display name.</param>
        /// <param name="authenticationPath"><para>[optional]</para>
        /// The path for the authentication backend. 
        /// Defaults to <see cref="AuthenticationBackendDefaultPaths.AppId" />
        /// Provide a value only if you have customized the path.</param>
        /// <returns>The task.</returns>
        [Obsolete("The AppId Authentication backend in Vault is now deprecated with the addition " +
          "of the new AppRole backend. There are no plans to remove it, but we encourage " +
          "using AppRole whenever possible, as it offers enhanced functionality " +
          "and can accommodate many more types of authentication paradigms.")]
        Task AppIdAuthenticationConfigureAppIdAsync(string appId, string policyValue, string displayName = null, string authenticationPath = AuthenticationBackendDefaultPaths.AppId);

        /// <summary>
        /// Configures the <see cref="userId"/> and says that it can be paired with <see cref="appIdValue"/> 
        /// but only if the client is in the <see cref="cidrBlock"/> (an optional construct).
        /// This means that if a client authenticates and provide both <see cref="appIdValue"/>  and <see cref="userId"/>, 
        /// then the <see cref="appIdValue"/> will authenticate that client with the policy.
        /// In practice, both the <see cref="userId"/> and <see cref="appIdValue"/>  are likely hard-to-guess UUID-like values.
        /// Note that it is possible to authorize multiple app IDs with each user ID by writing them as 
        /// comma-separated values to the user ID mapping. 
        /// </summary>
        /// <param name="userId">
        /// <para>[required]</para>
        /// The user identifier.</param>
        /// <param name="appIdValue">
        /// <para>[required]</para>
        /// The application identifier(s).</param>
        /// <param name="cidrBlock">
        /// <para>[optional]</para>
        /// The cidr block restriction for the user.</param>
        /// <param name="authenticationPath"><para>[optional]</para>
        /// The path for the authentication backend. 
        /// Defaults to <see cref="AuthenticationBackendDefaultPaths.AppId" />
        /// Provide a value only if you have customized the path.</param>
        /// <returns>The task.</returns>
        [Obsolete("The AppId Authentication backend in Vault is now deprecated with the addition " +
          "of the new AppRole backend. There are no plans to remove it, but we encourage " +
          "using AppRole whenever possible, as it offers enhanced functionality " +
          "and can accommodate many more types of authentication paradigms.")]
        Task AppIdAuthenticationConfigureUserIdAsync(string userId, string appIdValue, string cidrBlock = null, string authenticationPath = AuthenticationBackendDefaultPaths.AppId);

        /// <summary>
        /// Gets the list of existing AppRoles in the backend.
        /// raja todo.. fill in all strongly typed apis together.
        /// </summary>
        /// <returns>The list.</returns>
        Task<Secret<ListInfo>> AppRoleAuthenticationGetRolesAsync();

        /// <summary>
        /// Configures the credentials required to perform API calls to AWS. 
        /// The instance identity document fetched from the PKCS#7 signature will provide 
        /// the EC2 instance ID. The credentials configured using this endpoint will be used 
        /// to query the status of the instances via DescribeInstances API.
        /// If static credentials are not provided using this endpoint, then the credentials will be 
        /// retrieved from the environment variables AWS_ACCESS_KEY, AWS_SECRET_KEY and AWS_REGION respectively. 
        /// If the credentials are still not found and if the backend is configured 
        /// on an EC2 instance with metadata querying capabilities, the credentials are fetched automatically.
        /// </summary>
        /// <param name="awsEc2AccessCredentials">
        /// <para>[optional]</para>
        /// AWS Access credentials.</param>
        /// <param name="authenticationPath"><para>[optional]</para>
        /// The path for the authentication backend. 
        /// Defaults to <see cref="AuthenticationBackendDefaultPaths.AwsEc2" />
        /// Provide a value only if you have customized the path.</param>
        /// <returns>The task.</returns>
        Task AwsEc2AuthenticationConfigureClientAccessCredentialsAsync(AwsEc2AccessCredentials awsEc2AccessCredentials = null, string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2);

        /// <summary>
        /// Returns the configured AWS access credentials.
        /// </summary>
        /// <param name="authenticationPath"><para>[optional]</para>
        /// The path for the authentication backend. 
        /// Defaults to <see cref="AuthenticationBackendDefaultPaths.AwsEc2" />
        /// Provide a value only if you have customized the path.</param>
        /// <returns>The configured AWS access credentials.</returns>
        Task<Secret<AwsEc2AccessCredentials>> AwsEc2AuthenticationGetClientAccessCredentialsAsync(string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2);

        /// <summary>
        /// Deletes the configured AWS access credentials.
        /// </summary>
        /// <param name="authenticationPath"><para>[optional]</para>
        /// The path for the authentication backend. 
        /// Defaults to <see cref="AuthenticationBackendDefaultPaths.AwsEc2" />
        /// Provide a value only if you have customized the path.</param>
        /// <returns>The task.</returns>
        Task AwsEc2AuthenticationDeleteClientAccessCredentialsAsync(string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2);

        /// <summary>
        /// Registers an AWS public key to be used to verify the instance identity documents. 
        /// While the PKCS#7 signature of the identity documents have DSA digest, 
        /// the identity signature will have RSA digest, and hence the public keys for each type varies respectively. 
        /// Indicate the type of the public key using the "type" parameter.
        /// </summary>
        /// <param name="awsEc2PublicKeyInfo">The aws ec2 public key information.</param>
        /// <param name="authenticationPath">The authentication path.</param>
        /// <returns>The task.</returns>
        Task AwsEc2AuthenticationRegisterAwsPublicKeyAsync(AwsEc2PublicKeyInfo awsEc2PublicKeyInfo, string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2);

        /// <summary>
        /// Returns the configured AWS public key.
        /// </summary>
        /// <param name="certificateName">
        /// <para>[required]</para>
        /// The name of the certificate.
        /// </param>
        /// <param name="authenticationPath"><para>[optional]</para>
        /// The path for the authentication backend. 
        /// Defaults to <see cref="AuthenticationBackendDefaultPaths.AwsEc2" />
        /// Provide a value only if you have customized the path.</param>
        /// <returns>The configured AWS access credentials.</returns>
        Task<Secret<AwsEc2PublicKeyInfo>> AwsEc2AuthenticationGetAwsPublicKeyAsync(string certificateName, string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2);

        /// <summary>
        /// Returns all the AWS public certificates that are registered with the backend.
        /// </summary>
        /// <param name="authenticationPath"><para>[optional]</para>
        /// The path for the authentication backend. 
        /// Defaults to <see cref="AuthenticationBackendDefaultPaths.AwsEc2" />
        /// Provide a value only if you have customized the path.</param>
        /// <returns>The configured AWS access credentials.</returns>
        Task<Secret<ListInfo>> AwsEc2AuthenticationGetAwsPublicKeyListAsync(string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2);

        /// <summary>
        /// Configures the periodic tidying operation of the whitelisted identity entries.
        /// </summary>
        /// <param name="awsEc2AuthenticationTidyOptions">The aws ec2 authentication tidy options.</param>
        /// <param name="authenticationPath"><para>[optional]</para>
        /// The path for the authentication backend. 
        /// Defaults to <see cref="AuthenticationBackendDefaultPaths.AwsEc2" />
        /// Provide a value only if you have customized the path.</param>
        /// <returns>The task.</returns>
        Task AwsEc2AuthenticationConfigureIdentityWhitelistTidyOptionsAsync(AwsEc2AuthenticationTidyOptions awsEc2AuthenticationTidyOptions, string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2);

        /// <summary>
        /// Returns the configured periodic whitelist tidying settings.
        /// </summary>
        /// <param name="authenticationPath"><para>[optional]</para>
        /// The path for the authentication backend. 
        /// Defaults to <see cref="AuthenticationBackendDefaultPaths.AwsEc2" />
        /// Provide a value only if you have customized the path.</param>
        /// <returns>The tidy options.</returns>
        Task<Secret<AwsEc2AuthenticationTidyOptions>> AwsEc2AuthenticationGetIdentityWhitelistTidyOptionsAsync(string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2);

        /// <summary>
        /// Deletes the configured periodic whitelist tidying settings.
        /// </summary>
        /// <param name="authenticationPath"><para>[optional]</para>
        /// The path for the authentication backend. 
        /// Defaults to <see cref="AuthenticationBackendDefaultPaths.AwsEc2" />
        /// Provide a value only if you have customized the path.</param>
        /// <returns>The task.</returns>
        Task AwsEc2AuthenticationDeleteIdentityWhitelistTidyOptionsAsync(string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2);

        /// <summary>
        /// Configures the periodic tidying operation of the blacklisted identity entries.
        /// </summary>
        /// <param name="awsEc2AuthenticationTidyOptions">The aws ec2 authentication tidy options.</param>
        /// <param name="authenticationPath"><para>[optional]</para>
        /// The path for the authentication backend. 
        /// Defaults to <see cref="AuthenticationBackendDefaultPaths.AwsEc2" />
        /// Provide a value only if you have customized the path.</param>
        /// <returns>The task.</returns>
        Task AwsEc2AuthenticationConfigureRoletagBlacklistTidyOptionsAsync(AwsEc2AuthenticationTidyOptions awsEc2AuthenticationTidyOptions, string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2);

        /// <summary>
        /// Returns the configured periodic blacklisted tidying settings.
        /// </summary>
        /// <param name="authenticationPath"><para>[optional]</para>
        /// The path for the authentication backend. 
        /// Defaults to <see cref="AuthenticationBackendDefaultPaths.AwsEc2" />
        /// Provide a value only if you have customized the path.</param>
        /// <returns>The tidy options.</returns>
        Task<Secret<AwsEc2AuthenticationTidyOptions>> AwsEc2AuthenticationGetRoletagBlacklistTidyOptionsAsync(string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2);

        /// <summary>
        /// Deletes the configured periodic blacklisted tidying settings.
        /// </summary>
        /// <param name="authenticationPath"><para>[optional]</para>
        /// The path for the authentication backend. 
        /// Defaults to <see cref="AuthenticationBackendDefaultPaths.AwsEc2" />
        /// Provide a value only if you have customized the path.</param>
        /// <returns>The task.</returns>
        Task AwsEc2AuthenticationDeleteRoletagBlacklistTidyOptionsAsync(string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2);

        /// <summary>
        /// Registers a role in the backend. 
        /// Only those instances which are using the role registered using this endpoint, 
        /// will be able to perform the login operation. 
        /// Constraints can be specified on the role, that are applied on the instances attempting to login. 
        /// At least one constraint should be specified on the role.
        /// </summary>
        /// <param name="awsEc2AuthenticationRole">
        /// <para>[required]</para>
        /// The aws ec2 authentication role.</param>
        /// <param name="authenticationPath"><para>[optional]</para>
        /// The path for the authentication backend. 
        /// Defaults to <see cref="AuthenticationBackendDefaultPaths.AwsEc2" />
        /// Provide a value only if you have customized the path.</param>
        /// <returns></returns>
        Task AwsEc2AuthenticationRegisterRoleAsync(AwsEc2AuthenticationRole awsEc2AuthenticationRole, string authenticationPath = AuthenticationBackendDefaultPaths.AwsEc2);

        /// <summary>
        /// Enables multi factor authentication with the specific type.
        /// </summary>
        /// <param name="supportedAuthenticationBackendMountPoint">
        /// <para>[required]</para>
        /// The supported authentication backend mount point.
        /// </param>
        /// <param name="mfaType">
        /// <para>[required]</para>
        /// Type of the mfa.
        /// </param>
        /// <returns>The task.</returns>
        Task EnableMultiFactorAuthenticationAsync(string supportedAuthenticationBackendMountPoint, string mfaType = "duo");

        /// <summary>
        /// Writes contains connection information for the Duo Auth API.
        /// </summary>
        /// <param name="supportedAuthenticationBackendMountPoint">
        /// <para>[required]</para>
        /// The supported authentication backend mount point.
        /// </param>
        /// <param name="host">
        /// <para>[required]</para>
        /// The host.
        /// </param>
        /// <param name="integrationKey">
        /// <para>[required]</para>
        /// The integration key.
        /// </param>
        /// <param name="secretKey">
        /// <para>[required]</para>
        /// The secret key.
        /// </param>
        /// <returns>The task.</returns>
        Task WriteDuoAccessAsync(string supportedAuthenticationBackendMountPoint, string host, string integrationKey, string secretKey);

        /// <summary>
        /// Writes the duo configuration asynchronous.
        /// </summary>
        /// <param name="supportedAuthenticationBackendMountPoint">
        /// <para>[required]</para>
        /// The supported authentication backend mount point.
        /// </param>
        /// <param name="userAgent">
        /// <para>[required]</para>
        /// The user agent to use when connecting to Duo.
        /// </param>
        /// <param name="usernameFormat">
        /// <para>[required]</para>
        /// The username format which controls how the username used to login is transformed before authenticating with Duo. 
        /// This field is a format string that is passed the original username as its first argument and outputs the new username. 
        /// For example "%s@example.com" would append "@example.com" to the provided username before connecting to Duo.
        /// </param>
        /// <returns>The task.</returns>
        Task WriteDuoConfigurationAsync(string supportedAuthenticationBackendMountPoint, string userAgent, string usernameFormat);

        /// <summary>
        /// An all-purpose method to read any value from vault from any path.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The path where the value is stored.</param>
        /// <returns>
        /// The data as a general dictionary along with the lease information.
        /// </returns>
        Task<Secret<Dictionary<string, object>>> ReadSecretAsync(string path);

        /// <summary>
        /// An all-purpose method to write any value to vault at any path.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The path where the value is to be stored.</param>
        /// <param name="values"><para>[required]</para>
        /// The value to be written.</param>
        /// <returns>
        /// The data as a general dictionary if any data is returned by Vault.
        /// </returns>
        Task<Secret<Dictionary<string, object>>> WriteSecretAsync(string path, IDictionary<string, object> values);

        /// <summary>
        /// Deletes the value at the specified path in Vault.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The path where the value is to be stored.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeleteSecretAsync(string path);

        /// <summary>
        /// Gets the list of token accessors. This requires root capability, and access to it should be tightly 
        /// controlled as the accessors can be used to revoke very large numbers of tokens 
        /// and their associated leases at once.
        /// </summary>
        /// <returns>The list of accessors.</returns>
        Task<Secret<ListInfo>> GetTokenAccessorListAsync();

        /// <summary>
        /// Creates a new token.
        /// <para>
        /// Certain options are only available to when called by a root token.
        /// If used with the 'createAsOrphan' flag, a root token is not required to create an orphan token (otherwise set with the noParent option).
        /// </para>
        /// </summary>
        /// <param name="tokenCreationOptions">The token creation options.</param>
        /// <returns>
        /// The secret with the data.
        /// </returns>
        Task<Secret<Dictionary<string, object>>> CreateTokenAsync(TokenCreationOptions tokenCreationOptions = null);

        /// <summary>
        /// Gets the token information for the provided <see cref="token" />.
        /// </summary>
        /// <param name="token"><para>[required]</para>
        /// The token.</param>
        /// <returns>
        /// The secret with <see cref="TokenInfo" />.
        /// </returns>
        Task<Secret<TokenInfo>> GetTokenInfoAsync(string token);

        /// <summary>
        /// Gets the properties of the token associated with the accessor, 
        /// except the token ID. 
        /// This is meant for purposes where there is no access to token ID 
        /// but there is need to fetch the properties of a token.
        /// </summary>
        /// <param name="tokenAccessor"><para>[required]</para>
        ///  Accessor of the token to lookup.</param>
        /// <returns>The token info.</returns>
        Task<Secret<TokenAccessorInfo>> GetTokenInfoByAccessorAsync(string tokenAccessor);

        /// <summary>
        /// Gets the calling client token information. i.e. the token used by the client as part of this call.
        /// </summary>
        /// <returns>
        /// The secret with <see cref="CallingTokenInfo" />.
        /// </returns>
        Task<Secret<CallingTokenInfo>> GetCallingTokenInfoAsync();

        /// <summary>
        /// Renews a lease associated with the calling token.
        /// This is used to prevent the expiration of a token, and the automatic revocation of it.
        /// Token renewal is possible only if there is a lease associated with it.
        /// </summary>
        /// <param name="token"><para>[required]</para>
        /// The token to renew.</param>
        /// <param name="incrementSeconds"><para>[optional]</para>
        /// A requested amount of time in seconds to extend the lease. This is advisory and may be ignored.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task RenewTokenAsync(string token = null, int? incrementSeconds = null);

        /// <summary>
        /// Renews a lease associated with the calling token.
        /// This is used to prevent the expiration of a token, and the automatic revocation of it.
        /// Token renewal is possible only if there is a lease associated with it.
        /// </summary>
        /// <param name="incrementSeconds"><para>[optional]</para>
        /// A requested amount of time in seconds to extend the lease. This is advisory and may be ignored.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task RenewCallingTokenAsync(int? incrementSeconds = null);

        /// <summary>
        /// Revokes a token and all child tokens if the <see cref="revokeAllChildTokens" /> value is <value>true</value>.
        /// When the token is revoked, all secrets generated with it are also revoked.
        /// </summary>
        /// <param name="token"><para>[required]</para>
        /// The token to revoke.</param>
        /// <param name="revokeAllChildTokens"><para>[required]</para>
        /// if set to <c>true</c> [revoke all child tokens].
        /// else only the current token is revoked. All child tokens are orphaned, but can be revoked subsequently.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task RevokeTokenAsync(string token, bool revokeAllChildTokens);

        /// <summary>
        /// Revokes the token associated with the accessor and all the child tokens. 
        /// This is meant for purposes where there is no access to token ID 
        /// but there is need to revoke a token and its children.
        /// </summary>
        /// <param name="tokenAccessor"><para>[required]</para>
        /// Accessor of the token.</param>
        /// <returns>The token info.</returns>
        Task RevokeTokenByAccessorAsync(string tokenAccessor);

        /// <summary>
        /// Revokes the calling client token and all child tokens.
        /// When the token is revoked, all secrets generated with it are also revoked.
        /// </summary>
        /// <returns>
        /// The task.
        /// </returns>
        Task RevokeCallingTokenAsync();

        /// <summary>
        /// Deletes the token role.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>The task.</returns>
        Task DeleteTokenRoleAsync(string roleName);

        /// <summary>
        /// Gets the token role information.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns>The role configuration.</returns>
        Task<Secret<TokenRoleInfo>> GetTokenRoleInfoAsync(string roleName);

        /// <summary>
        /// Gets the token role list.
        /// </summary>
        /// <returns>The role list.</returns>
        Task<Secret<ListInfo>> GetTokenRoleListAsync();

        /// <summary>
        /// Creates (or replaces) the named role. 
        /// Roles enforce specific behavior when creating tokens that allow token functionality 
        /// that is otherwise not available or would require sudo/root privileges to access. 
        /// Role parameters, when set, override any provided options to the create endpoints. 
        /// The role name is also included in the token path, allowing all tokens created 
        /// against a role to be revoked using the <see cref="RevokeAllSecretsOrTokensUnderPrefixAsync"/> endpoint.
        /// </summary>
        /// <param name="tokenRoleDefinition">The token role definition.</param>
        /// <returns>The task.</returns>
        Task WriteTokenRoleInfoAsync(TokenRoleDefinition tokenRoleDefinition);
    }
}
