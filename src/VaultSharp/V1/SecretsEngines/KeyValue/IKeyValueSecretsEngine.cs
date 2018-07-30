using VaultSharp.V1.SecretsEngines.KeyValue.V1;
using VaultSharp.V1.SecretsEngines.KeyValue.V2;

namespace VaultSharp.V1.SecretsEngines.KeyValue
{
    public interface IKeyValueSecretsEngine
    {
        IKeyValueSecretsEngineV1 V1 { get; }

        IKeyValueSecretsEngineV2 V2 { get; }
    }
}