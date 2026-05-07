using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.PKI.Keys
{
    public interface IPKIKeys
    {
        Task<Secret<KeysData>> ListKeys(string pkiBackendMountPoint = null, string wrapTimeToLive = null);


        Task<Secret<KeyData>> GenerateKey(KeyData data, string pkiBackendMountPoint = null, string wrapTimeToLive = null);

        Task<Secret<KeyData>> GetKey(string keyName, string pkiBackendMountPoint = null, string wrapTimeToLive = null);

        Task<Secret<ImportKeyData>> ImportKey(ImportKeyData data, string pkiBackendMountPoint = null,
            string wrapTimeToLive = null);


        Task DeleteKey(string keyName, string pkiBackendMountPoint = null, string wrapTimeToLive = null);
    }
}