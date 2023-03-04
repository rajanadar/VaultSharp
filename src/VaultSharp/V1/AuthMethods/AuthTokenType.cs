using System;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods
{
    [JsonConverter(typeof(AuthTokenTypeEnumConverter))]
    public enum AuthTokenType
    {
        [EnumMember(Value = "service")]  // CRITICAL: doesn't work for System.Text.Json
        Service,

        [EnumMember(Value = "batch")]
        Batch,

        [EnumMember(Value = "default")]
        Default,

        [EnumMember(Value = "default-service")]
        DefaultService,

        [EnumMember(Value = "default-batch")]
        DefaultBatch,
    }

    internal sealed class AuthTokenTypeEnumConverter : JsonConverter<AuthTokenType>
    {
        public override AuthTokenType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();

            switch(stringValue)
            {
                case "service":
                    return AuthTokenType.Service;

                case "batch":
                    return AuthTokenType.Batch;

                case "default":
                    return AuthTokenType.Default;

                case "default-service":
                    return AuthTokenType.DefaultService;

                case "default-batch":
                    return AuthTokenType.DefaultBatch;
            }

            throw new VaultApiException("Unknown AuthTokenType of " + stringValue);
        }

        public override void Write(Utf8JsonWriter writer, AuthTokenType value, JsonSerializerOptions options)
        {
            string stringValue = null;

            switch(value)
            {
                case AuthTokenType.Service:
                    stringValue = "service";
                    break;

                case AuthTokenType.Batch:
                    stringValue = "batch";
                    break;

                case AuthTokenType.Default:
                    stringValue = "default";
                    break;

                case AuthTokenType.DefaultService:
                    stringValue = "default-service";
                    break;

                case AuthTokenType.DefaultBatch:
                    stringValue = "default-batch";
                    break;
            }

            writer.WriteStringValue(stringValue);
        }
    }
}