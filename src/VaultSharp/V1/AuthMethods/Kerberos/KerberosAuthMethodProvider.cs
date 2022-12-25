using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.Kerberos
{
    internal class KerberosAuthMethodProvider : IKerberosAuthMethod
    {
        private readonly Polymath _polymath;

        public KerberosAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }
    }
}