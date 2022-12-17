using System.Linq;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.AppRole;

namespace VaultSharp.Samples
{
    partial class Program
    {
        private static void RunAuthMethodSamples()
        {
            // Token Apis.
            var callingTokenInfo = _authenticatedVaultClient.V1.Auth.Token.LookupSelfAsync().Result;
            DisplayJson(callingTokenInfo);

            // use a renewable token here.
            // var renewInfo = _authenticatedVaultClient.V1.Auth.Token.RenewSelfAsync().Result;
            // DisplayJson(renewInfo);

            // _authenticatedVaultClient.V1.Auth.Token.RevokeSelfAsync().Wait();

            // Needs Manual pre-steps.
            // Startup vault with normal dev mode. not real.
            /*
                .\vault.exe auth enable approle
                .\vault.exe write auth/approle/role/my-role secret_id_ttl=10m  token_num_uses=10  token_ttl=20m   token_max_ttl=30m  secret_id_num_uses=40
                .\vault.exe read auth/approle/role/my-role/role-id
                << note roleid >>
                .\vault.exe write -f auth/approle/role/my-role/secret-id
                << .\vault.exe secret id >>

                .]vault.exe write auth/approle/login role_id=335277eb-932a-71ef-825d-d403a1663f0d  secret_id=5ed1d413-4d06-1604-8db3-9000b4bd9204
             */

            string appRoleId = "71fce15a-3edb-4263-9227-9d623f1f9a22";
            string secretId = "df1326a5-07f2-814c-4f83-9a00c99694d6";

            IAuthMethodInfo appRoleAuthMethodInfo = new AppRoleAuthMethodInfo(appRoleId, secretId);

            VaultClientSettings authVaultClientSettings = GetVaultClientSettings(appRoleAuthMethodInfo);

            var token = "";
            authVaultClientSettings.BeforeApiRequestAction = (c, r) =>
            {
                if (r.Headers.Contains("X-Vault-Token"))
                {
                    token = r.Headers.Single(h => h.Key == "X-Vault-Token").Value.Single();
                }
                else if (r.Headers.Contains("Authorization"))
                {
                    var val = r.Headers.Single(h => h.Key == "Authorization").Value.Single();
                    token = val.Substring(val.IndexOf(' ') + 1, val.Length - 1);
                }
            };

            // IVaultClient vaultClient = new VaultClient(authVaultClientSettings);

            // var result = vaultClient.V1.System.GetCallingTokenCapabilitiesAsync("v1/sys").Result;
            // Assert.True(result.Data.Capabilities.Any());

            // token
            // IAuthMethodInfo tokenAuthMethodInfo = new TokenAuthMethodInfo(token); // throws null

            // authVaultClientSettings = GetVaultClientSettings(tokenAuthMethodInfo);
            // vaultClient = new VaultClient(authVaultClientSettings);

            // result = vaultClient.V1.System.GetCallingTokenCapabilitiesAsync("v1/sys").Result;
            // Assert.True(result.Data.Capabilities.Any());
        }
    }
}
