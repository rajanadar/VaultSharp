using System;
using System.Text.Json;
using System.Text.Json.Nodes;
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
            if (value != null)
            {
                string pluginName = value.PluginName;
                
                // VERY IMPORTANT: Do not use this, as it'll go into a infinite loop
                // This is because attribute-based-converters go into a loop.
                // Use manual construction.
                // JsonSerializer.Serialize(writer, value);

                var connectionConfigModelJsonObject = new JsonObject();
                
                connectionConfigModelJsonObject.Add("plugin_name", value.PluginName);
                connectionConfigModelJsonObject.Add("plugin_version", value.PluginVersion);
                connectionConfigModelJsonObject.Add("verify_connection", value.VerifyConnection);

                // TODO: Is this the best way to convert list of string to Json node?
                JsonNode allowedRolesJsonNode = null;

                if (value.AllowedRoles != null)
                {
                    var allowedRolesJsonArray = new JsonArray();

                    foreach (var valueAllowedRole in value.AllowedRoles)
                    {
                        allowedRolesJsonArray.Add(valueAllowedRole);
                    }

                    allowedRolesJsonNode = allowedRolesJsonArray;
                }
                
                connectionConfigModelJsonObject.Add("allowed_roles", allowedRolesJsonNode);
                
                JsonNode rootRotationStatementsJsonNode = null;

                if (value.RootRotationStatements != null)
                {
                    var rootRotationStatementsJsonArray = new JsonArray();

                    foreach (var rootRotationStatement in value.RootRotationStatements)
                    {
                        rootRotationStatementsJsonArray.Add(rootRotationStatement);
                    }

                    rootRotationStatementsJsonNode = rootRotationStatementsJsonArray;
                }
                
                connectionConfigModelJsonObject.Add("root_rotation_statements", rootRotationStatementsJsonNode);
                
                connectionConfigModelJsonObject.Add("password_policy", value.PasswordPolicyName);
                connectionConfigModelJsonObject.Add("connection_url", value.ConnectionUrl);
                connectionConfigModelJsonObject.Add("username", value.Username);
                connectionConfigModelJsonObject.Add("password", value.Password);
                connectionConfigModelJsonObject.Add("disable_escaping", value.DisableEscapingSpecialCharactersInUsernameAndPassword);

                if (pluginName.Equals("postgresql-database-plugin"))
                {
                    var postgreSqlConnectionConfigModel =
                        value as PostgreSQLConnectionConfigModel;
                    
                    connectionConfigModelJsonObject.Add("max_open_connections", postgreSqlConnectionConfigModel.MaximumOpenConnections);
                    connectionConfigModelJsonObject.Add("max_idle_connections", postgreSqlConnectionConfigModel.MaximumIdleConnections);
                    connectionConfigModelJsonObject.Add("max_connection_lifetime", postgreSqlConnectionConfigModel.MaximumConnectionLifetime);
                    connectionConfigModelJsonObject.Add("auth_type", postgreSqlConnectionConfigModel.AuthType);
                    connectionConfigModelJsonObject.Add("service_account_json", postgreSqlConnectionConfigModel.ServiceAccountJson);
                    connectionConfigModelJsonObject.Add("username_template", postgreSqlConnectionConfigModel.UsernameTemplate);
                    connectionConfigModelJsonObject.Add("password_authentication", postgreSqlConnectionConfigModel.PasswordAuthentication);
                }
                
                connectionConfigModelJsonObject.WriteTo(writer);
            }
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