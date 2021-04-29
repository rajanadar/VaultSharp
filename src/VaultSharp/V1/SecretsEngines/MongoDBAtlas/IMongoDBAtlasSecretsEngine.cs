using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.MongoDBAtlas
{
    /// <summary>
    /// MongoDBAtlas Secrets Engine.
    /// </summary>
    public interface IMongoDBAtlasSecretsEngine
    {
        /// <summary>
        /// Generates a dynamic MongoDBAtlas cred based on the role definition.
        /// </summary>
        /// <param name="name"><para>[required]</para>
        /// Unique identifier name of the credential
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the MongoDBAtlas backend. Defaults to <see cref="SecretsEngineMountPoints.MongoDBAtlas" />
        /// Provide a value only if you have customized the MongoDBAtlas mount point.</param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with the <see cref="MongoDBAtlasCredentials" /> as the data.
        /// </returns>
        Task<Secret<MongoDBAtlasCredentials>> GetCredentialsAsync(string name, string mountPoint = null, string wrapTimeToLive = null);
    }
}