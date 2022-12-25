using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.OCI
{
    internal class OCIAuthMethodProvider : IOCIAuthMethod
    {
        private readonly Polymath _polymath;

        public OCIAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }
    }
}