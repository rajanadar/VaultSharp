using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using VaultSharp.V1.AuthMethods;

namespace VaultSharp.V1.SecretsEngines
{
    /// <summary>
    /// Converts the <see cref="SecretsEngineType" /> object to and from JSON.
    /// </summary>
    internal class SecretsEngineTypeJsonConverter : JsonConverter<SecretsEngineType>
    {
        public override void Write(Utf8JsonWriter writer, SecretsEngineType value, JsonSerializerOptions serializer)
        {
            if (value != null)
            {
                writer.WriteStringValue(value.Type);
            }
        }

        public override SecretsEngineType Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            return new SecretsEngineType(reader.GetString());
        }
    }
}