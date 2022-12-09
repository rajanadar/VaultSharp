
namespace VaultSharp.V1.Commons
{
    /// <summary>
    /// Represents a generic Vault Request.
    /// </summary>
    /// <typeparam name="TRequestData">The type of the data contained in the request.</typeparam>
    public class VaultApiRequest<TRequestData> : VaultApiRequest
    {
        /// <summary>
        /// Gets or sets the request data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public TRequestData RequestData { get; set; }
    }
}