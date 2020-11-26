using VaultSharp.Core;
using VaultSharp.V1.SecretsEngines.Enterprise.KeyManagement;
using VaultSharp.V1.SecretsEngines.Enterprise.KMIP;
using VaultSharp.V1.SecretsEngines.Enterprise.Transform;

namespace VaultSharp.V1.SecretsEngines.Enterprise
{
    /// <summary>
    /// Enterprise Secrets Engines
    /// </summary>
    internal class EnterpriseProvider : IEnterprise
    {
        public EnterpriseProvider(Polymath polymath)
        {
            KeyManagement = new KeyManagementSecretsEngineProvider(polymath);
            KMIP = new KMIPSecretsEngineProvider(polymath);
            Transform = new TransformSecretsEngineProvider(polymath);
        }

        public IKeyManagementSecretsEngine KeyManagement { get; }
        public IKMIPSecretsEngine KMIP { get; }
        public ITransformSecretsEngine Transform { get; }
    }
}