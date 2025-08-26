using Newtonsoft.Json;

namespace OrchardCore.Client.Core.Config
{
    public class OAuthToken
    {
        [JsonProperty(Constants.JsonProperties.AccessToken)]
        public string AccessToken { get; set; }

        [JsonProperty(Constants.JsonProperties.TokenType)]
        public string TokenType { get; set; }

        [JsonProperty(Constants.JsonProperties.ExpiresIn)]
        public int ExpiresIn { get; set; }

        public DateTime ExpiryDateTime { get; set; }
    }
}