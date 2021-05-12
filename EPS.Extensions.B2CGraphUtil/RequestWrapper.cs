using Newtonsoft.Json;

namespace EPS.Extensions.B2CGraphUtil
{
    public class GroupWrapper<T>
    {
        [JsonProperty("odata.metadata")]
        public string odataMetadata { get; set; }
        public T[] Items { get; set; }
    }

}
