using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.PKI.Issuers
{
    public interface IPKIIssuers
    {
        Task<Secret<KeysData>> ListIssuers(string pkiBackendMountPoint = null, string wrapTimeToLive = null);

        Task<Secret<IssuerResponseData>> AddIssuer(IssuerData data, string pkiBackendMountPoint = null, string wrapTimeToLive = null);

        Task<Secret<IssuerConfigResponseData>> GetIssuerConfigData(string issuerName,
            string pkiBackendMountPoint = null, string wrapTimeToLive = null);

        Task<Secret<IssuerConfigResponseData>> ConfigureIssuer(IssuerConfigRequestData data,
            string pkiBackendMountPoint = null, string wrapTimeToLive = null);

        Task DeleteIssuer(string issuerName, string pkiBackendMountPoint = null, string wrapTimeToLive = null);
    }
}