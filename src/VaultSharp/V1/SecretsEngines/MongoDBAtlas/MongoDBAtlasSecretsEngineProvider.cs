using System.Net.Http;
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.MongoDBAtlas
{
    internal class MongoDBAtlasSecretsEngineProvider : IMongoDBAtlasSecretsEngine
    {
        private readonly Polymath _polymath;

        private string MountPoint
        {
            get 
            {
                _polymath.VaultClientSettings.SecretEngineMountPoints.TryGetValue(nameof(SecretsEngineDefaultPaths.MongoDBAtlas), out var mountPoint);
                return mountPoint ?? SecretsEngineDefaultPaths.MongoDBAtlas;
            }
        }

        public MongoDBAtlasSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }

        public async Task<Secret<MongoDBAtlasCredentials>> GetCredentialsAsync(string name, string mountPoint = null, string wrapTimeToLive = null)
        {
            Checker.NotNull(name, "name");

            return await _polymath.MakeVaultApiRequest<Secret<MongoDBAtlasCredentials>>(mountPoint ?? MountPoint, "/creds/" + name.Trim('/'), HttpMethod.Get, wrapTimeToLive: wrapTimeToLive).ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}