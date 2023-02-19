using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using VaultSharp.V1.SecretsEngines;

namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Converts the <see cref="AuditBackendType" /> object to and from JSON.
    /// </summary>
    internal class AuditBackendTypeJsonConverter : JsonConverter<AuditBackendType>
    {
        public override void Write(Utf8JsonWriter writer, AuditBackendType value, JsonSerializerOptions serializer)
        {
            if (value != null)
            {
                writer.WriteStringValue(value.Value);
            }
        }

        public override AuditBackendType Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            return new AuditBackendType(reader.GetString());
        }
    }
}