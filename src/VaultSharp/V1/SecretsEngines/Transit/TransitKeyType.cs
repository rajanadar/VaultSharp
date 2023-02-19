using System.Runtime.Serialization;
using System.Text.Json.Serialization;


namespace VaultSharp.V1.SecretsEngines.Transit
{
    /// <summary>
    /// Represents the type of Transit key to be generated.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransitKeyType
    {
        /// <summary>
        /// AES-256 wrapped with GCM using a 96-bit nonce size AEAD (symmetric, supports derivation and convergent encryption, default)
        /// </summary>
        [EnumMember(Value = "aes256-gcm96")]
        aes256_gcm96 = 1,

        /// <summary>
        /// ECDSA using the P-256 elliptic curve (asymmetric)
        /// </summary>
        [EnumMember(Value = "ecdsa-p256")]
        ecdsa_p256 = 2,

        /// <summary>
        /// AES-128 wrapped with GCM using a 96-bit nonce size AEAD (symmetric, supports derivation and convergent encryption)
        /// </summary>
        [EnumMember(Value = "aes128-gcm96")]
        aes128_gcm96 = 3,

        /// <summary>
        /// ChaCha20-Poly1305 AEAD (symmetric, supports derivation and convergent encryption)
        /// </summary>
        [EnumMember(Value = "chacha20-poly1305")]
        chacha20_poly1305 = 4,

        /// <summary>
        /// ED25519 (asymmetric, supports derivation). When using derivation, a sign operation with the
        /// same context will derive the same key and signature; this is a signing analogue to
        /// convergent_encryption.
        /// </summary>
        [EnumMember(Value = "ed25519")]
        ed25519 = 5,

        /// <summary>
        /// ECDSA using the P-384 elliptic curve (asymmetric)
        /// </summary>
        [EnumMember(Value = "ecdsa-p384")]
        ecdsa_p384 = 6,

        /// <summary>
        /// ECDSA using the P-521 elliptic curve (asymmetric)
        /// </summary>
        [EnumMember(Value = "ecdsa-p521")]
        ecdsa_p521 = 7,

        /// <summary>
        /// RSA with bit size of 2048 (asymmetric)
        /// </summary>
        [EnumMember(Value = "rsa-2048")]
        rsa_2048 = 8,

        /// <summary>
        /// RSA with bit size of 3072 (asymmetric)
        /// </summary>
        [EnumMember(Value = "rsa-3072")]
        rsa_3072 = 9,

        /// <summary>
        /// RSA with bit size of 4096 (asymmetric)
        /// </summary>
        [EnumMember(Value = "rsa-4096")]
        rsa_4096 = 10,

        /// <summary>
        /// HMAC (HMAC generation, verification)
        /// </summary>
        [EnumMember(Value = "hmac")]
        hmac = 11,
    }
}