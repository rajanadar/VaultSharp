using VaultSharp.Core;
using VaultSharp.V1.SecretsEngines.KeyValue.V1;
using VaultSharp.V1.SecretsEngines.KeyValue.V2;

namespace VaultSharp.V1.SecretsEngines.KeyValue
{
    internal class KeyValueSecretsEngineProvider : IKeyValueSecretsEngine
    {
        public KeyValueSecretsEngineProvider(Polymath polymath)
        {
            V1 = new KeyValueSecretsEngineV1Provider(polymath);
            V2 = new KeyValueSecretsEngineV2Provider(polymath);
        }

        public IKeyValueSecretsEngineV1 V1 { get; }

        public IKeyValueSecretsEngineV2 V2 { get; }
    }
}
