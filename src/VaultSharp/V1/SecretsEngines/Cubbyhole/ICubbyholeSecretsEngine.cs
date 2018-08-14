using System.Collections.Generic;
using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Cubbyhole
{
    /// <summary>
    /// Cubbyhole Engine.
    /// </summary>
    public interface ICubbyholeSecretsEngine
    {
        /// <summary>
        /// Retrieves the secret at the specified location.
        /// </summary>
        /// <param name="secretPath"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the data.
        /// </returns>
        Task<Secret<Dictionary<string, object>>> ReadSecretAsync(string secretPath, string wrapTimeToLive = null);

        /// <summary>
        /// Retrieves the secret location path entries at the specified location.
        /// Folders are suffixed with /. The input must be a folder; list on a file will not return a value. 
        /// The values themselves are not accessible via this API.
        /// </summary>
        /// <param name="folderPath"><para>[required]</para>
        /// The location path where the secret needs to be read from.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[required]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret list with the data.
        /// </returns>
        Task<Secret<ListInfo>> ReadSecretPathsAsync(string folderPath, string wrapTimeToLive = null);

        /// <summary>
        /// Stores a secret at the specified location.
        /// </summary>
        /// <param name="secretPath"><para>[required]</para>
        /// The location path where the secret needs to be stored.</param>
        /// <param name="values"><para>[required]</para>
        /// The values to be written. The values will be overwritten.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task WriteSecretAsync(string secretPath, IDictionary<string, object> values);

        /// <summary>
        /// Deletes the secret at the specified location.
        /// </summary>
        /// <param name="secretPath"><para>[required]</para>
        /// The location path where the secret needs to be deleted from.</param>
        /// <returns>
        /// The task.
        /// </returns>
        Task DeleteSecretAsync(string secretPath);
    }
}