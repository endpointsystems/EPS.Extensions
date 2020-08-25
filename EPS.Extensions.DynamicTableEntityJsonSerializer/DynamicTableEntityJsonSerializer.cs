using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace EPS.Extensions.DynamicTableEntityJsonSerializer
{
    public class DynamicTableEntityJsonSerializer
    {
        private readonly DynamicTableEntityJsonConverter jsonConverter;

        public DynamicTableEntityJsonSerializer(List<string> excludedProperties = null) =>
            jsonConverter = new DynamicTableEntityJsonConverter(excludedProperties!);

        public string Serialize(DynamicTableEntity entity)
        {
            return entity != null ? JsonConvert.SerializeObject(entity, jsonConverter) : null;
        }

        public DynamicTableEntity Deserialize(string serializedEntity)
        {
            return serializedEntity != null
                ? JsonConvert.DeserializeObject<DynamicTableEntity>(serializedEntity, (JsonConverter) jsonConverter)
                : null;
        }
    }
}
