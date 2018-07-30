using VaultSharp.Core;

namespace VaultSharp.V1.SecretsEngines.KeyValue
{
    internal class KeyValueSecretsEngineProvider : IKeyValueSecretsEngine
    {
        private readonly Polymath _polymath;

        public KeyValueSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }
    }
}