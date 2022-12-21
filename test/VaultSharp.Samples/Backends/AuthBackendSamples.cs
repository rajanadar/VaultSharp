using System.Linq;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private static void RunAuthMethodSamples()
        {
            RunAliCloudAuthMethodSamples();
            RunAppRoleAuthMethodSamples();
            RunAWSAuthMethodSamples();

            RunTokenAuthMethodSamples();
        }
    }
}
