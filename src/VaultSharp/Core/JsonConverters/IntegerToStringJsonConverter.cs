using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VaultSharp.Core
{
    /// <summary>
    /// System.Text.Json cannot handle int to string automatically.
    /// This converter helps.
    /// </summary>
    internal class IntegerToStringJsonConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                var stringValue = reader.GetInt32();
                return stringValue.ToString();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }

            throw new JsonException("IntegerToStringJsonConverter was used in a non-integer or string field.");
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}