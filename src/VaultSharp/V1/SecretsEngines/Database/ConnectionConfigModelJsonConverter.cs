using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using VaultSharp.V1.SecretsEngines.Database.Models;
using VaultSharp.V1.SecretsEngines.Database.Models.PostgreSQL;

namespace VaultSharp.V1.SecretsEngines.Database
{
    /// <summary>
    /// Converts the <see cref="ConnectionConfigModel" /> object from JSON.
    /// </summary>
    internal class ConnectionConfigModelJsonConverter : JsonConverter<ConnectionConfigModel>
    {
        public override void Write(Utf8JsonWriter writer, ConnectionConfigModel value, JsonSerializerOptions serializer)
        {
            // VERY IMPORTANT
            // This will only work if the converter is registered programmatically and not via attributes
            // Via attributes, the code goes into a stack overflow exception.
            JsonSerializer.Serialize(writer, value);
        }
        
        public override ConnectionConfigModel Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            using (var jsonDocument = JsonDocument.ParseValue(ref reader))
            {
                if (!jsonDocument.RootElement.TryGetProperty("plugin_name",
                        out var pluginNameProperty))
                {
                    throw new JsonException();
                }
                
                string pluginName = pluginNameProperty.GetString();
                var jsonString = jsonDocument.RootElement.GetRawText();

                if (pluginName.Equals("postgresql-database-plugin"))
                {
                    return JsonSerializer.Deserialize<PostgreSQLConnectionConfigModel>(jsonString);
                }
                
                return JsonSerializer.Deserialize<ConnectionConfigModel>(jsonString);
            }
        }
    }
}