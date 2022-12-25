using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.Cert
{
    internal class CertAuthMethodProvider : ICertAuthMethod
    {
        private readonly Polymath _polymath;

        public CertAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }
    }
}