#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace EPS.Extensions.DynamicTableEntityJsonSerializer
{
    public class DynamicTableEntityJsonConverter: JsonConverter
    {
        private readonly List<string>? excludedProperties;

        public DynamicTableEntityJsonConverter(List<string>? excludedProperties = null!)
        {
            this.excludedProperties = excludedProperties;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null) return;
            writer.WriteStartObject();
            WriteJsonProperties(writer, (DynamicTableEntity) value, excludedProperties);
            writer.WriteEndObject();

        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            var entity = new DynamicTableEntity();
            using var enumerator = JObject.Load(reader).Properties().ToList().GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                if (current == null) continue;
                if (string.Equals(current.Name, "PartitionKey", StringComparison.Ordinal))
                    entity.PartitionKey = current.Value.ToString();
                else if (string.Equals(current.Name, "RowKey", StringComparison.Ordinal))
                    entity.RowKey = current.Value.ToString();
                else if (string.Equals(current.Name, "Timestamp", StringComparison.Ordinal))
                    entity.Timestamp = (DateTimeOffset) current.Value;
                else if (string.Equals(current.Name, "ETag", StringComparison.Ordinal))
                    entity.ETag = current.Value.ToString();
                else
                {
                    var eprop = CreateEntityProperty(serializer, current);
                    entity.Properties.Add(current.Name, eprop);
                }
            }
            return entity;
        }

        public override bool CanConvert(Type objectType) => typeof(DynamicTableEntity).IsAssignableFrom(objectType);

        private static void WriteJsonProperties(JsonWriter writer, DynamicTableEntity entity,
            List<string>? excludedProperties = null!)
        {
            if (entity == null) return;
            writer.WritePropertyName("PartitionKey");
            writer.WriteValue(entity.PartitionKey);
            writer.WritePropertyName("RowKey");
            writer.WriteValue(entity.RowKey);
            writer.WritePropertyName("Timestamp");
            writer.WriteValue(entity.Timestamp);
            writer.WritePropertyName("ETag");
            writer.WriteValue(entity.ETag);

            using var enumerator =
                (excludedProperties == null
                    ? entity.Properties
                    : entity.Properties.Where(p => !excludedProperties.Contains(p.Key))).GetEnumerator();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current;
                WriteJsonProperty(writer, current);
            }
        }

        private static void WriteJsonProperty(JsonWriter writer,
            KeyValuePair<string, EntityProperty> property)
        {
            if (string.IsNullOrWhiteSpace(property.Key) || property.Value == null) return;
            switch ((int) property.Value.PropertyType)
            {
                case 0:
                    WriteJsonProperty(writer, property.Key, property.Value.StringValue);
                    break;
                case 1:
                    WriteJsonProperty(writer, property.Key, property.Value.BinaryValue);
                    break;
                case 2:
                    WriteJsonProperty(writer, property.Key, property.Value.BooleanValue!);
                    break;
                case 3:
                    WriteJsonProperty(writer, property.Key, property.Value.DateTimeOffsetValue!);
                    break;
                case 4:
                    WriteJsonProperty(writer, property.Key, property.Value.DoubleValue!);
                    break;
                case 5:
                    WriteJsonProperty(writer, property.Key, property.Value.GuidValue!);
                    break;
                case 6:
                    WriteJsonProperty(writer, property.Key, property.Value.Int32Value!);
                    break;
                case 7:
                    WriteJsonProperty(writer, property.Key, property.Value.Int64Value!);
                    break;
                default:
                    throw new NotSupportedException(
                        $"Unsupported EntityProperty.PropertyType:{property.Value.PropertyType} detected during serialization.");
            }
        }

        private static void WriteJsonProperty(JsonWriter writer, string key, object value)
        {
            writer.WritePropertyName(key);
            writer.WriteValue(value);
        }

        private static void WriteJsonProperty(JsonWriter writer, string key, object value, EdmType edmType)
        {
            writer.WritePropertyName(key);
            writer.WriteStartObject();
            writer.WritePropertyName(key);
            writer.WriteValue(value);
            writer.WritePropertyName("EdmType");
            writer.WriteValue(edmType.ToString());
            writer.WriteEndObject();
        }

        private static EntityProperty CreateEntityProperty(JsonSerializer serializer, JProperty property)
        {
            if (property != null) return null!;

            var list = JObject.Parse(property!.Value.ToString()).Properties().ToList();
            var edmType = (EdmType) Enum.Parse(typeof(EdmType), list[1].Value.ToString(), true);
            EntityProperty entityProperty;
            switch ((int)edmType)
            {
                case 0:
                    entityProperty =
                        EntityProperty.GeneratePropertyForString(list[0].Value.ToObject<string>(serializer));
                    break;
                case 1:
                    entityProperty =
                        EntityProperty.GeneratePropertyForByteArray(list[0].Value.ToObject<byte[]>(serializer));
                    break;
                case 2:
                    entityProperty = EntityProperty.GeneratePropertyForBool(list[0].Value.ToObject<bool>(serializer));
                    break;
                case 3:
                    entityProperty =
                        EntityProperty.GeneratePropertyForDateTimeOffset(list[0].Value
                            .ToObject<DateTimeOffset>(serializer));
                    break;
                case 4:
                    entityProperty =
                        EntityProperty.GeneratePropertyForDouble(list[0].Value.ToObject<double>(serializer));
                    break;
                case 5:
                    entityProperty = EntityProperty.GeneratePropertyForGuid(list[0].Value.ToObject<Guid>(serializer));
                    break;
                case 6:
                    entityProperty = EntityProperty.GeneratePropertyForInt(list[0].Value.ToObject<int>(serializer));
                    break;
                case 7:
                    entityProperty = EntityProperty.GeneratePropertyForLong(list[0].Value.ToObject<long>(serializer));
                    break;
                default:
                    throw new NotSupportedException($"Unsupported EntityProperty.PropertyType:{edmType} detected during deserialization");
            }

            return entityProperty;
        }
    }
}
