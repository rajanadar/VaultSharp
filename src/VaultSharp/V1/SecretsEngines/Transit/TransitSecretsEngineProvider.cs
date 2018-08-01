using VaultSharp.Core;

namespace VaultSharp.V1.SecretsEngines.Transit
{
    internal class TransitSecretsEngineProvider : ITransitSecretsEngine
    {
        private readonly Polymath _polymath;

        public TransitSecretsEngineProvider(Polymath polymath)
        {
            _polymath = polymath;
        }
    }
}