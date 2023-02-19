using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.AuthMethods
{
    /// <summary>
    /// Converts the <see cref="AuthMethodType" /> object to and from JSON.
    /// </summary>
    internal class AuthMethodTypeJsonConverter : JsonConverter<AuthMethodType>
    {
        public override void Write(Utf8JsonWriter writer, AuthMethodType value, JsonSerializerOptions serializer)
        {
            if (value != null)
            {
                writer.WriteStringValue(value.Type);
            }
        }

        public override AuthMethodType Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            return new AuthMethodType(reader.GetString());
        }
    }
}