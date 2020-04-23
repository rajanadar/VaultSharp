using System;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.AliCloud;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.AWS;
using VaultSharp.V1.AuthMethods.Azure;
using VaultSharp.V1.AuthMethods.Cert;
using VaultSharp.V1.AuthMethods.Custom;
using VaultSharp.V1.AuthMethods.GitHub;
using VaultSharp.V1.AuthMethods.GoogleCloud;
using VaultSharp.V1.AuthMethods.JWT;
using VaultSharp.V1.AuthMethods.Kubernetes;
using VaultSharp.V1.AuthMethods.LDAP;
using VaultSharp.V1.AuthMethods.Okta;
using VaultSharp.V1.AuthMethods.RADIUS;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods.UserPass;

namespace VaultSharp.V1.AuthMethods
{
    internal static class AuthProviderFactory
    {
        public static IAuthMethodLoginProvider CreateAuthenticationProvider(IAuthMethodInfo authInfo, Polymath polymath)
        {
            if (authInfo.AuthMethodType == AuthMethodType.AliCloud)
            {
                return new AliCloudAuthMethodLoginProvider(authInfo as AliCloudAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.AppRole)
            {
                return new AppRoleAuthMethodLoginProvider(authInfo as AppRoleAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.AWS)
            {
                return new AWSAuthMethodLoginProvider(authInfo as AbstractAWSAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.Azure)
            {
                return new AzureAuthMethodLoginProvider(authInfo as AzureAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.GitHub)
            {
                return new GitHubAuthMethodLoginProvider(authInfo as GitHubAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.GoogleCloud)
            {
                return new GoogleCloudAuthMethodLoginProvider(authInfo as GoogleCloudAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.JWT)
            {
                return new JWTAuthMethodLoginProvider(authInfo as JWTAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.Kubernetes)
            {
                return new KubernetesAuthMethodLoginProvider(authInfo as KubernetesAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.LDAP)
            {
                return new LDAPAuthMethodLoginProvider(authInfo as LDAPAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.Okta)
            {
                return new OktaAuthMethodLoginProvider(authInfo as OktaAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.RADIUS)
            {
                return new RADIUSAuthMethodLoginProvider(authInfo as RADIUSAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.Cert)
            {
                // we have attached the certificates to request elsewhere.
                return new CertAuthMethodLoginProvider(authInfo as CertAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.Token)
            {
                return new TokenAuthMethodLoginProvider(authInfo as TokenAuthMethodInfo, polymath);
            }

            if (authInfo.AuthMethodType == AuthMethodType.UserPass)
            {
                return new UserPassAuthMethodLoginProvider(authInfo as UserPassAuthMethodInfo, polymath);
            }

            var customAuthMethodInfo = authInfo as CustomAuthMethodInfo;

            if (customAuthMethodInfo != null)
            {
                return new CustomAuthMethodLoginProvider(customAuthMethodInfo, polymath);
            }

            throw new NotSupportedException("The requested authentication backend type is not supported: " + authInfo.AuthMethodType);
        }
    }
}