using Newtonsoft.Json;
using OrchardCore.Client.Core.Handlers;

namespace OrcardCore.Client.Web.Entities
{
    [JsonConverter(typeof(JsonPathHandler))]
    public class Page
    {
        [JsonProperty(Constants.JsonProperties.Title)]
        public string Title { get; set; }

        [JsonProperty(Constants.JsonProperties.FileName)]
        public string FileName { get; set; }

        [JsonProperty(Constants.JsonProperties.Path)]
        public string Path { get; set; }

        [JsonProperty(Constants.JsonProperties.HtmlBody)]
        public string HtmlBody { get; set; }

        [JsonProperty(Constants.JsonProperties.JavaScript)]
        public string JavaScript { get; set; }

        [JsonProperty(Constants.JsonProperties.SeoMeta)]
        public SeoMeta SeoMeta { get; set; }
    }
}
