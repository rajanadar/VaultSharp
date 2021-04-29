using System.Threading.Tasks;
using VaultSharp.V1.Commons;

namespace VaultSharp.V1.SecretsEngines.Enterprise.Transform
{
    /// <summary>
    /// The Transform Secrets Engine.
    /// </summary>
    public interface ITransformSecretsEngine
    {
        /// <summary>
        /// This endpoint encodes the provided value using a named role.
        /// </summary>
        /// <param name="roleName">
        /// [required]
        /// Specifies the role name to use for this operation.
        /// </param>
        /// <param name="encodeRequestOptions"><para>[required]</para>
        /// The options.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transform backend. Defaults to <see cref="SecretsEngineMountPoints.Transform" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with encoded text.
        /// </returns>
        Task<Secret<EncodedResponse>> EncodeAsync(string roleName, EncodeRequestOptions encodeRequestOptions, string mountPoint = null, string wrapTimeToLive = null);

        /// <summary>
        /// This endpoint decodes the provided value using a named role.
        /// </summary>
        /// <param name="roleName">
        /// [required]
        /// Specifies the role name to use for this operation.
        /// </param>
        /// <param name="decodeRequestOptions"><para>[required]</para>
        /// The options.
        /// </param>
        /// <param name="mountPoint"><para>[optional]</para>
        /// The mount point for the Transform backend. Defaults to <see cref="SecretsEngineMountPoints.Transform" />
        /// Provide a value only if you have customized the mount point.
        /// </param>
        /// <param name="wrapTimeToLive">
        /// <para>[optional]</para>
        /// The TTL for the token and can be either an integer number of seconds or a string duration of seconds.
        /// </param>
        /// <returns>
        /// The secret with decoded text.
        /// </returns>
        Task<Secret<DecodedResponse>> DecodeAsync(string roleName, DecodeRequestOptions decodeRequestOptions, string mountPoint = null, string wrapTimeToLive = null);
    }
}