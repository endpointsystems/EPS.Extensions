using Newtonsoft.Json;

namespace EPS.Extensions.GraphObjects
{
    public class GroupWrapper<T>
    {
        [JsonProperty("odata.metadata")]
        public string odataMetadata { get; set; }
        public T[] Items { get; set; }
    }

}
