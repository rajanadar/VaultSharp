using System;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// The set of hash algorithms that are currently supported by Vault.
    /// </summary>
    public enum HashAlgorithm
    {
        /// <summary>
        /// Specify in order to use whatever the default hash algorithm for the key is; use for ed25519 keys, which specify their own hash algorithms.
        /// </summary>
        Default,
        [Obsolete]
        sha1,
        sha2_224,
        sha2_256,
        sha2_384,
        sha2_512,
        sha3_224,
        sha3_256,
        sha3_384,
        sha3_512
    }
}