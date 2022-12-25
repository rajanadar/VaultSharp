using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.Kubernetes
{
    internal class KubernetesAuthMethodProvider : IKubernetesAuthMethod
    {
        private readonly Polymath _polymath;

        public KubernetesAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }
    }
}