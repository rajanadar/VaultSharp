using System;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.AliCloud;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.AWS;
using VaultSharp.V1.AuthMethods.Azure;
using VaultSharp.V1.AuthMethods.Cert;
using VaultSharp.V1.AuthMethods.GitHub;
using VaultSharp.V1.AuthMethods.Kubernetes;
using VaultSharp.V1.AuthMethods.LDAP;
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

            Token = new TokenAuthMethodProvider(_polymath);
        }

        public IAliCloudAuthMethod AliCloud => throw new NotImplementedException();

        public IAppRoleAuthMethod AppRole => throw new NotImplementedException();

        public IAWSAuthMethod AWS => throw new NotImplementedException();

        public IAzureAuthMethod Azure => throw new NotImplementedException();

        public IGitHubAuthMethod GitHub => throw new NotImplementedException();

        public IGitHubAuthMethod GoogleCloud => throw new NotImplementedException();

        public IKubernetesAuthMethod Kubernetes => throw new NotImplementedException();

        public ILDAPAuthMethod LDAP => throw new NotImplementedException();

        public IOktaAuthMethod Okta => throw new NotImplementedException();

        public IRADIUSAuthMethod RADIUS => throw new NotImplementedException();

        public ICertAuthMethod Cert => throw new NotImplementedException();

        public ITokenAuthMethod Token { get; }

        public IUserPassAuthMethod UserPass => throw new NotImplementedException();
    }
}