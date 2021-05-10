using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Enterprise.KeyManagement
{
    /// <summary>
    /// The KeyManagement Secrets Engine.
    /// </summary>
    public interface IKeyManagementSecretsEngine
    {
        /// <summary>
        /// Reads information about a named key.
        /// The keys object will hold information regarding each key version. 
        /// Different information will be returned depending on the key type. 
        /// For example, an asymmetric key will return its public key in a standard format for the type.
        /// </summary>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the key to read.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.KeyManagement" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>Key Info.</returns>
        Task<Secret<KeyManagementKey>> ReadKeyAsync(string keyName, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// Reads information about a key that's been distributed to a KMS provider.
        /// </summary>
        /// <param name="kmsName"><para>[required]</para>
        /// Specifies the name of the KMS provider.
        /// </param>
        /// <param name="keyName"><para>[required]</para>
        /// Specifies the name of the key to read.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the backend. Defaults to <see cref="SecretsEngineMountPoints.KeyManagement" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>Key Info.</returns>
        Task<Secret<KeyManagementKMSKey>> ReadKeyInKMSAsync(string kmsName, string keyName, string mountPoint = null, string wrapTimeToLive = null);
    }
}