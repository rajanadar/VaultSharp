using System.Collections.Generic;
using Newtonsoft.Json;
using VaultSharp.Infrastructure.JsonConverters;

namespace VaultSharp.Backends.System.Models
{
    /// <summary>
    /// Represents the raw secret data.
    /// </summary>
    [JsonConverter(typeof(RawDataJsonConverter))]
    public class RawData
    {
        /// <summary>
        /// Gets or sets the raw values.
        /// </summary>
        /// <value>
        /// The raw values.
        /// </value>
        public IDictionary<string, object> RawValues { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RawData"/> class.
        /// </summary>
        public RawData()
        {
            RawValues = new Dictionary<string, object>();
        }
    }
}