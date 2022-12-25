using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.GitHub
{
    internal class GitHubAuthMethodProvider : IGitHubAuthMethod
    {
        private readonly Polymath _polymath;

        public GitHubAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }
    }
}