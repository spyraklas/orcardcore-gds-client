using Newtonsoft.Json;
using OrchardCore.Client.Core.Handlers;

namespace OrcardCore.Client.Web.Entities
{
    [JsonConverter(typeof(JsonPathHandler))]
    public class CustomMetaTag
    {
        [JsonProperty(Constants.JsonProperties.Name)]
        public string Name { get; set; }

        [JsonProperty(Constants.JsonProperties.Content)]
        public string Content { get; set; }

        [JsonProperty(Constants.JsonProperties.HttpEquiv)]
        public string HttpEquiv { get; set; }

        [JsonProperty(Constants.JsonProperties.Property)]
        public string Property { get; set; }

        [JsonProperty(Constants.JsonProperties.Charset)]
        public string Charset { get; set; }
    }
}
