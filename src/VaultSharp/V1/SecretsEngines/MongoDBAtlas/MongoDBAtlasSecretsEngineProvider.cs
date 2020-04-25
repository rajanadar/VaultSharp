using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.MongoDBAtlas
{
    internal class MongoDBAtlasSecretsEngineProvider : IMongoDBAtlasSecretsEngine
    {
        private readonly Polymath _polymath;

        public MongoDBAtlasSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<MongoDBAtlasCredentials>> GetCredentialsAsync(string name, string mountPoint = SecretsEngineDefaultPaths.MongoDBAtlas, string wrapTimeToLive = null)
        {
            Checker.NotNull(mountPoint, "mountPoint");
            Checker.NotNull(name, "name");

            return await _polymath.MakeVaultApiRequest<Secret<MongoDBAtlasCredentials>>("v1/" + mountPoint.Trim('/') + "/creds/" + name.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}