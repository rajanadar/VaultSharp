using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VaultSharp.V1.SystemBackend.Enterprise
{
    /// <summary>
    /// Converts the <see cref="EnforcementLevel" /> object to and from JSON.
    /// </summary>
    internal class EnforcementLevelJsonConverter : JsonConverter<EnforcementLevel>
    {
        public override void Write(Utf8JsonWriter writer, EnforcementLevel value, JsonSerializerOptions serializer)
        {
            if (value != null)
            {
                writer.WriteStringValue(value.Value);
            }
        }

        public override EnforcementLevel Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            return new EnforcementLevel(reader.GetString());
        }
    }
}