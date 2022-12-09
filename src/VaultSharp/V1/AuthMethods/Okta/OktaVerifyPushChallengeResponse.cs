using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.Okta
{
    public class OktaVerifyPushChallengeResponse
    {
        /// <summary>
        /// The correct push challenge answer
        /// </summary>
        [JsonProperty("correct_answer")]
        public int CorrectAnswer { get; set; }
    }
}