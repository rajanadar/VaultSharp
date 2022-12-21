using Xunit;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private static void RunTokenAuthMethodSamples()
        {
            var callingTokenInfo = _authenticatedVaultClient.V1.Auth.Token.LookupSelfAsync().Result;
            DisplayJson(callingTokenInfo);
            Assert.NotNull(callingTokenInfo.Data.Id);            
        }
    }
}