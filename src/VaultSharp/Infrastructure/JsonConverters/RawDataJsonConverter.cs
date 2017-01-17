using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VaultSharp.Backends.System.Models;

namespace VaultSharp.Infrastructure.JsonConverters
{

    /// <summary>
    /// Converts the <see cref="RawData" /> object to and from JSON.
    /// </summary>
    public class RawDataJsonConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var rawData = value as RawData;
            var rawValues = rawData != null ? rawData.RawValues : null;

            writer.WriteValue(JsonConvert.SerializeObject(rawValues));
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jtoken = JToken.Load(reader);

            if (jtoken != null && jtoken.HasValues && jtoken["value"] != null)
            {
                var value = jtoken["value"].Value<string>();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    var rawValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(value);

                    return new RawData
                    {
                        RawValues = rawValues
                    };
                }
            }

            return new RawData();
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(RawData);
        }
    }
}