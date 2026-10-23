using System.Threading;
using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.PKI.Issuers
{
    public interface IPKIIssuers
    {
        Task<Secret<KeysData>> ListIssuers(string pkiBackendMountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default);

        Task<Secret<IssuerResponseData>> AddIssuer(IssuerData data, string pkiBackendMountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default);

        Task<Secret<IssuerConfigResponseData>> GetIssuerConfigData(string issuerName,
            string pkiBackendMountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default);

        Task<Secret<IssuerConfigResponseData>> ConfigureIssuer(IssuerConfigRequestData data,
            string pkiBackendMountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default);

        Task DeleteIssuer(string issuerName, string pkiBackendMountPoint = null, string wrapTimeToLive = null, CancellationToken cancellationToken = default);
    }
}