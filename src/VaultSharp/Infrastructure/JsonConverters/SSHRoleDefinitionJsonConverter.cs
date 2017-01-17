using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VaultSharp.Backends.Secret.Models.SSH;

namespace VaultSharp.Infrastructure.JsonConverters
{
    /// <summary>
    /// Converts the <see cref="SSHRoleDefinition" /> object from JSON.
    /// </summary>
    public class SSHRoleDefinitionJsonConverter : JsonConverter
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON.
        /// </summary>
        /// <value>
        /// <c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can write JSON; otherwise, <c>false</c>.
        /// </value>
        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <exception cref="System.NotImplementedException">Unnecessary because CanWrite is false. The type will skip the converter.</exception>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
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
            object target = null;

            if (jtoken != null && jtoken.HasValues && jtoken["key_type"] != null)
            {
                var keyType = jtoken["key_type"].Value<string>();

                if (string.Equals(keyType, SSHKeyType.otp.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    target = new SSHOTPRoleDefinition();
                }
                else
                {
                    if (string.Equals(keyType, SSHKeyType.dynamic.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        target = new SSHDynamicRoleDefinition();
                    }
                }
            }

            if (target != null)
            {
                serializer.Populate(jtoken.CreateReader(), target);
            }

            return target;
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
            return objectType == typeof (SSHRoleDefinition);
        }
    }
}