using System;
using System.Text.Json;
using System.Text.Json.Nodes;
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
                // VERY IMPORTANT: Do not use this, as it'll go into a infinite loop
                // This is because attribute-based-converters go into a loop.
                // Use manual construction.
                // JsonSerializer.Serialize(writer, value);

                var auditBackendJsonObject = new JsonObject();

                auditBackendJsonObject.Add("description", value.Description);
                auditBackendJsonObject.Add("type", value.Type.Value);
                
                auditBackendJsonObject.Add("local", value.Local);


                if (value.Type == AuditBackendType.File)
                {
                    var fileAuditBackend = value as FileAuditBackend;

                    var fileAuditBackendOptionsJsonObject = new JsonObject();

                    fileAuditBackendOptionsJsonObject.Add("elide_list_responses", fileAuditBackend.Options.ElideListResponses);

                    // enterprise only

                    if (!string.IsNullOrEmpty(fileAuditBackend.Options.Fallback))
                    {
                        fileAuditBackendOptionsJsonObject.Add("fallback", fileAuditBackend.Options.Fallback);
                    }

                    if (!string.IsNullOrEmpty(fileAuditBackend.Options.Filter))
                    {
                        fileAuditBackendOptionsJsonObject.Add("filter", fileAuditBackend.Options.Filter);
                    }

                    fileAuditBackendOptionsJsonObject.Add("format", fileAuditBackend.Options.Format);
                    fileAuditBackendOptionsJsonObject.Add("hmac_accessor", fileAuditBackend.Options.HmacAccessor);
                    fileAuditBackendOptionsJsonObject.Add("log_raw", fileAuditBackend.Options.LogSensitiveDataInRawFormat);
                    fileAuditBackendOptionsJsonObject.Add("prefix", fileAuditBackend.Options.Prefix);

                    fileAuditBackendOptionsJsonObject.Add("file_path", fileAuditBackend.Options.FilePath);

                    auditBackendJsonObject.Add("options", fileAuditBackendOptionsJsonObject);
                }

                if (value.Type == AuditBackendType.Syslog)
                {
                    var sysLogAuditBackend = value as SyslogAuditBackend;

                    var sysLogAuditBackendOptionsJsonObject = new JsonObject();

                    sysLogAuditBackendOptionsJsonObject.Add("elide_list_responses", sysLogAuditBackend.Options.ElideListResponses);

                    // enterprise only

                    if (!string.IsNullOrEmpty(sysLogAuditBackend.Options.Fallback))
                    {
                        sysLogAuditBackendOptionsJsonObject.Add("fallback", sysLogAuditBackend.Options.Fallback);
                    }

                    if (!string.IsNullOrEmpty(sysLogAuditBackend.Options.Filter))
                    {
                        sysLogAuditBackendOptionsJsonObject.Add("filter", sysLogAuditBackend.Options.Filter);
                    }

                    sysLogAuditBackendOptionsJsonObject.Add("format", sysLogAuditBackend.Options.Format);
                    sysLogAuditBackendOptionsJsonObject.Add("hmac_accessor", sysLogAuditBackend.Options.HmacAccessor);
                    sysLogAuditBackendOptionsJsonObject.Add("log_raw", sysLogAuditBackend.Options.LogSensitiveDataInRawFormat);
                    sysLogAuditBackendOptionsJsonObject.Add("prefix", sysLogAuditBackend.Options.Prefix);

                    sysLogAuditBackendOptionsJsonObject.Add("facility", sysLogAuditBackend.Options.Facility);
                    sysLogAuditBackendOptionsJsonObject.Add("tag", sysLogAuditBackend.Options.Tag);

                    auditBackendJsonObject.Add("options", sysLogAuditBackendOptionsJsonObject);
                }

                if (value is CustomAuditBackend)
                {
                    var customAuditBackend = value as CustomAuditBackend;

                    var customAuditBackendOptionsJsonObject = new JsonObject();

                    if (customAuditBackend.Options != null)
                    {
                        foreach (var key in customAuditBackend.Options.Keys)
                        {
                            customAuditBackendOptionsJsonObject.Add(key, customAuditBackend.Options[key]);
                        }
                    }

                    auditBackendJsonObject.Add("options", customAuditBackendOptionsJsonObject);
                }

                auditBackendJsonObject.WriteTo(writer);
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
                var jsonString = jsonDocument.RootElement.GetRawText();

                if (auditBackendType == AuditBackendType.File)
                {
#if NET8_0_OR_GREATER
                    return (FileAuditBackend)JsonSerializer.Deserialize(jsonString, options.GetTypeInfo(typeof(FileAuditBackend)));
#else
                    return JsonSerializer.Deserialize<FileAuditBackend>(jsonString, options);
#endif
                }

                if (auditBackendType == AuditBackendType.Syslog)
                {
#if NET8_0_OR_GREATER
                    return (SyslogAuditBackend)JsonSerializer.Deserialize(jsonString, options.GetTypeInfo(typeof(SyslogAuditBackend)));
#else
                    return JsonSerializer.Deserialize<SyslogAuditBackend>(jsonString, options);
#endif
                }

#if NET8_0_OR_GREATER
                return (CustomAuditBackend)JsonSerializer.Deserialize(jsonString, options.GetTypeInfo(typeof(CustomAuditBackend)));
#else
                return JsonSerializer.Deserialize<CustomAuditBackend>(jsonString, options);
#endif
            }
        }
    }
}