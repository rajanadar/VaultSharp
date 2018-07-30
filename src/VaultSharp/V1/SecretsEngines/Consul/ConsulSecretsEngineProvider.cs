using VaultSharp.Core;

namespace VaultSharp.V1.SecretsEngines.Consul
{
    internal class ConsulSecretsEngineProvider : IConsulSecretsEngine
    {
        private readonly Polymath _polymath;

        public ConsulSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }
    }
}