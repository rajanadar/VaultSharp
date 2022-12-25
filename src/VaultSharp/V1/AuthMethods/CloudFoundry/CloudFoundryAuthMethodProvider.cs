using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.CloudFoundry
{
    internal class CloudFoundryAuthMethodProvider : ICloudFoundryAuthMethod
    {
        private readonly Polymath _polymath;

        public CloudFoundryAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }
    }
}