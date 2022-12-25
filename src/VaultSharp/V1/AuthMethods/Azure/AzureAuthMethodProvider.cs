using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.Azure
{
    internal class AzureAuthMethodProvider : IAzureAuthMethod
    {
        private readonly Polymath _polymath;

        public AzureAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }
    }
}