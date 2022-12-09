using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.AuthMethods.Okta
{
    /// <summary>
    /// Non login operations.
    /// </summary>
    public interface IOktaAuthMethod
    {
        /// <summary>
        /// Verify a number challenge that may result from an Okta Verify Push challenge.
        /// </summary>
        /// <param name="nonce">
        /// [required]
        /// Nonce provided if performing login that requires number verification challenge. 
        /// Logins through the vault login CLI command will automatically generate a nonce.
        /// </param>
        /// <param name="mountPoint">Mount point of the Okta Auth method</param>
        /// <returns>Correct answer</returns>
        Task<Secret<OktaVerifyPushChallengeResponse>> VerifyPushChallengeAsync(string nonce, string mountPoint = "okta");
    }
}