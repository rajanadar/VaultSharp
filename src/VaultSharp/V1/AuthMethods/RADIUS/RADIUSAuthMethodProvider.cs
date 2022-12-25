using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.RADIUS
{
    internal class RADIUSAuthMethodProvider : IRADIUSAuthMethod
    {
        private readonly Polymath _polymath;

        public RADIUSAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }
    }
}