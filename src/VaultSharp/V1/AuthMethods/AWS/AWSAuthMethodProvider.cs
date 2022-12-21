using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.AWS;

namespace VaultSharp.V1.AuthMethods.AppRole
{
    internal class AWSAuthMethodProvider : IAWSAuthMethod
    {
        private readonly Polymath _polymath;

        public AWSAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }
    }
}