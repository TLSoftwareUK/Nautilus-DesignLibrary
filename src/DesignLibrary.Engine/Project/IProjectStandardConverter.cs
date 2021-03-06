﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TLS.DesignLibrary.Engine.Project
{
    public class IProjectStandardConverter : JsonConverter<IProjectStandard>
    {

        private enum TypeDiscriminator
        {
            EurocodeStandardBritishNa = 0
        }

        public override bool CanConvert(Type type)
        {
            return typeof(IProjectStandard).IsAssignableFrom(type);
        }

        public override IProjectStandard Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            if (!reader.Read()
                || reader.TokenType != JsonTokenType.PropertyName
                || reader.GetString() != "TypeDiscriminator")
            {
                throw new JsonException();
            }

            if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException();
            }

            IProjectStandard baseClass;
            TypeDiscriminator typeDiscriminator = (TypeDiscriminator) reader.GetInt32();
            switch (typeDiscriminator)
            {
                case TypeDiscriminator.EurocodeStandardBritishNa:
                    if (!reader.Read() || reader.GetString() != "TypeValue")
                    {
                        throw new JsonException();
                    }

                    if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
                    {
                        throw new JsonException();
                    }

                    baseClass = (EurocodeStandardBritishNa) JsonSerializer.Deserialize(ref reader,
                        typeof(EurocodeStandardBritishNa));
                    break;
                default:
                    throw new NotSupportedException();
            }

            if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            return baseClass;
        }

        public override void Write(Utf8JsonWriter writer, IProjectStandard value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (value is EurocodeStandardBritishNa derivedA)
            {
                writer.WriteNumber("TypeDiscriminator", (int) TypeDiscriminator.EurocodeStandardBritishNa);
                writer.WritePropertyName("TypeValue");
                JsonSerializer.Serialize(writer, derivedA);
            }
            else
            {
                throw new NotSupportedException();
            }

            writer.WriteEndObject();
        }
    }
}
