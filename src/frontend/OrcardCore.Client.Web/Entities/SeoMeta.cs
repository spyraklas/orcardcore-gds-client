using Newtonsoft.Json;
using OrchardCore.Client.Core.Handlers;

namespace OrcardCore.Client.Web.Entities
{
    [JsonConverter(typeof(JsonPathHandler))]
    public class SeoMeta
    {
        [JsonProperty(Constants.JsonProperties.MetaKeywords)]
        public string MetaKeywords { get; set; }

        [JsonProperty(Constants.JsonProperties.MetaDescription)]
        public string MetaDescription { get; set; }

        [JsonProperty(Constants.JsonProperties.CustomMetaTags)]
        public List<CustomMetaTag> CustomMetaTags { get; set; }

    }
}
