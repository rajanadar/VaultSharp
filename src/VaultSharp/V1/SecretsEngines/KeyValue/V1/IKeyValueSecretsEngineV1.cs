using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.KeyValue.V1
{
    public interface IKeyValueSecretsEngineV1
    {
        /// <summary>
        /// Retrieves the secret at the specified location.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the KeyValue backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValue" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the data.
        /// </returns>
        Task<Secret<Dictionary<string, object>>> ReadSecretAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValue, string wrapTimeToLive = null);

        /// <summary>
        /// Retrieves the secret location path entries at the specified location.
        /// Folders are suffixed with /. The input must be a folder; list on a file will not return a value. 
        /// The values themselves are not accessible via this API.
        /// </summary>
        /// <param name="path"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Generic backend. Defaults to <see cref="SecretsEngineDefaultPaths.KeyValue" />
        /// Provide a value only if you have customized the mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret list with the data.
        /// </returns>
        Task<Secret<ListInfo>> ReadSecretPathListAsync(string path, string mountPoint = SecretsEngineDefaultPaths.KeyValue, string wrapTimeToLive = null);
    }
}