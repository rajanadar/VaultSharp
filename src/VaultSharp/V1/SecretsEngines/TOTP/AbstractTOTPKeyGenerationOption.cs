namespace VaultSharp.V1.SecretsEngines.TOTP
{
    public abstract class AbstractTOTPKeyGenerationOption
    {
        /// <summary>
        /// Gets or sets the name of the issuing organization.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the name of the account associated with the key.
        /// </summary>
        public string AccountName { get; set; }
    }
}