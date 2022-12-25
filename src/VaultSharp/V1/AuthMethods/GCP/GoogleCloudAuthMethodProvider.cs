using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.GoogleCloud
{
    internal class GoogleCloudAuthMethodProvider : IGoogleCloudAuthMethod
    {
        private readonly Polymath _polymath;

        public GoogleCloudAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }
    }
}