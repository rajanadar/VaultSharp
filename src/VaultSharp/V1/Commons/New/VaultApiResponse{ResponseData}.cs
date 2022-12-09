
namespace VaultSharp.V1.Commons
{
    /// <summary>
    /// Represents a generic Vault Response.
    /// </summary>
    /// <typeparam name="TResponseData">The type of the data contained in the response.</typeparam>
    public class VaultApiResponse<TResponseData> : VaultApiResponse
    {
        /// <summary>
        /// Gets or sets the response data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public TResponseData ResponseData { get; set; }
    }
}