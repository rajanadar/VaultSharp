using Newtonsoft.Json;

namespace VaultSharp.V1.AuthMethods.Kubernetes.Models;

public class KubernetesConfig
{
    /// <summary>
    ///     Disable defaulting to the local CA cert and service account JWT when running in a Kubernetes pod.
    /// </summary>
    [JsonProperty("disable_local_ca_jwt")] public bool disable_local_ca_jwt;

    /// <summary>
    ///     PEM encoded CA cert for use by the TLS client used to talk with the Kubernetes API.
    ///     NOTE: Every line must end with a newline: \n If not set, the local CA cert will be used if running in a Kubernetes
    ///     pod.
    /// </summary>
    [JsonProperty("kubernetes_ca_cert")] public string? kubernetes_ca_cert = "";

    /// <summary>
    ///     required
    ///     Host must be a host string, a host:port pair, or a URL to the base of the Kubernetes API server.
    /// </summary>
    [JsonProperty("kubernetes_host")] public string? kubernetes_host;

    /// <summary>
    ///     Optional list of PEM-formatted public keys or certificates used to verify the signatures of Kubernetes service
    ///     account JWTs.
    ///     If a certificate is given, its public key will be extracted.Not every installation of Kubernetes exposes these
    ///     keys.
    /// </summary>
    [JsonProperty("pem_keys")] public string[]? pem_keys;

    /// <summary>
    ///     A service account JWT used to access the TokenReview API to validate other JWTs during login.
    ///     If not set, the local service account token is used if running in a Kubernetes pod,
    ///     otherwise the JWT submitted in the login payload will be used to access the Kubernetes TokenReview API.
    /// </summary>
    [JsonProperty("token_reviewer_jwt")] public string? token_reviewer_jwt = "";
}