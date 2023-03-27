using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SecretsEngines.Database
{
    /// <summary>
    /// Converts the <see cref="DatabaseProviderType" /> object to and from JSON.
    /// </summary>
    internal class DatabaseProviderTypeJsonConverter : JsonConverter<DatabaseProviderType>
    {
        public override void Write(Utf8JsonWriter writer, DatabaseProviderType value, JsonSerializerOptions serializer)
        {
            if (value != null)
            {
                writer.WriteStringValue(value.Type);
            }
        }

        public override DatabaseProviderType Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            return new DatabaseProviderType(reader.GetString());
        }
    }
}