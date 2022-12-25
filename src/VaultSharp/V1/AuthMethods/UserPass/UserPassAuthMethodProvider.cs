using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.UserPass
{
    internal class UserPassAuthMethodProvider : IUserPassAuthMethod
    {
        private readonly Polymath _polymath;

        public UserPassAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }
    }
}