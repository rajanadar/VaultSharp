using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace VaultSharp.V1.SystemBackend
{
    /// <summary>
    /// Converts the <see cref="AbstractAuditBackend" /> object from JSON.
    /// </summary>
    internal class AuditBackendJsonConverter : JsonConverter<AbstractAuditBackend>
    {
        public override void Write(Utf8JsonWriter writer, AbstractAuditBackend value, JsonSerializerOptions serializer)
        {
            if (value != null)
            {
                writer.WriteStringValue(JsonSerializer.Serialize(value));
            }
        }

        public override AbstractAuditBackend Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            using (var jsonDocument = JsonDocument.ParseValue(ref reader))
            {
                if (!jsonDocument.RootElement.TryGetProperty("type",
                    out var typeProperty))
                {
                    throw new JsonException();
                }

                var auditBackendType = new AuditBackendType(typeProperty.GetString());
                var jsonObject = jsonDocument.RootElement.GetRawText();

                if (auditBackendType == AuditBackendType.File)
                {
                    return (FileAuditBackend)JsonSerializer.Deserialize(jsonObject, type, options);
                }

                if (auditBackendType == AuditBackendType.Syslog)
                {
                    return (SyslogAuditBackend)JsonSerializer.Deserialize(jsonObject, type, options);
                }

                return (CustomAuditBackend)JsonSerializer.Deserialize(jsonObject, type, options);
            }
        }
    }
}