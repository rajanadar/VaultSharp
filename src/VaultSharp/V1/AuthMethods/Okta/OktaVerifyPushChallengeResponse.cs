﻿using System.Text.Json.Serialization;

namespace VaultSharp.V1.AuthMethods.Okta
{
    public class OktaVerifyPushChallengeResponse
    {
        /// <summary>
        /// The correct push challenge answer
        /// </summary>
        [JsonPropertyName("correct_answer")]
        public int CorrectAnswer { get; set; }
    }
}