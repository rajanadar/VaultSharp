
using System.Threading.Tasks;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.AliCloud;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.AWS;
using VaultSharp.V1.AuthMethods.Azure;
using VaultSharp.V1.AuthMethods.Cert;
using VaultSharp.V1.AuthMethods.CloudFoundry;
using VaultSharp.V1.AuthMethods.GitHub;
using VaultSharp.V1.AuthMethods.GoogleCloud;
using VaultSharp.V1.AuthMethods.JWT;
using VaultSharp.V1.AuthMethods.Kerberos;
using VaultSharp.V1.AuthMethods.Kubernetes;
using VaultSharp.V1.AuthMethods.LDAP;
using VaultSharp.V1.AuthMethods.OCI;
using VaultSharp.V1.AuthMethods.Okta;
using VaultSharp.V1.AuthMethods.RADIUS;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods.UserPass;

namespace VaultSharp.V1.AuthMethods
{
    internal class AuthMethodProvider : IAuthMethod
    {
        private readonly Polymath _polymath;

        public AuthMethodProvider(Polymath polymath)
        {
            _polymath = polymath;

            AliCloud = new AliCloudAuthMethodProvider(_polymath);
            AppRole = new AppRoleAuthMethodProvider(_polymath);
            AWS = new AWSAuthMethodProvider(_polymath);
            Azure = new AzureAuthMethodProvider(_polymath);
            CloudFoundry = new CloudFoundryAuthMethodProvider(_polymath);
            GitHub = new GitHubAuthMethodProvider(_polymath);
            GoogleCloud = new GoogleCloudAuthMethodProvider(_polymath);
            JWT = new JWTAuthMethodProvider(_polymath);
            Kerberos = new KerberosAuthMethodProvider(_polymath);
            Kubernetes = new KubernetesAuthMethodProvider(_polymath);
            LDAP = new LDAPAuthMethodProvider(_polymath);
            OCI = new OCIAuthMethodProvider(_polymath);
            Okta = new OktaAuthMethodProvider(_polymath);
            RADIUS = new RADIUSAuthMethodProvider(_polymath);
            Cert = new CertAuthMethodProvider(_polymath);
            Token = new TokenAuthMethodProvider(_polymath);
            UserPass = new UserPassAuthMethodProvider(_polymath);
        }

        public IAliCloudAuthMethod AliCloud { get; }

        public IAppRoleAuthMethod AppRole { get; }

        public IAWSAuthMethod AWS { get; }

        public IAzureAuthMethod Azure { get; }

        public ICloudFoundryAuthMethod CloudFoundry { get; }

        public IGitHubAuthMethod GitHub { get; }

        public IGoogleCloudAuthMethod GoogleCloud { get; }

        public IJWTAuthMethod JWT { get; }

        public IKubernetesAuthMethod Kubernetes { get; }

        public ILDAPAuthMethod LDAP { get; }

        public IKerberosAuthMethod Kerberos { get; }

        public IOCIAuthMethod OCI { get; }

        public IOktaAuthMethod Okta { get; }

        public IRADIUSAuthMethod RADIUS { get; }

        public ICertAuthMethod Cert { get; }

        public ITokenAuthMethod Token { get; }

        public IUserPassAuthMethod UserPass { get; }

        public void ResetVaultToken()
        {
            _polymath.SetVaultTokenDelegate();
        }

        public async Task PerformImmediateLogin()
        {
            await _polymath.PerformImmediateLogin().ConfigureAwait(_polymath.VaultClientSettings.ContinueAsyncTasksOnCapturedContext);
        }
    }
}