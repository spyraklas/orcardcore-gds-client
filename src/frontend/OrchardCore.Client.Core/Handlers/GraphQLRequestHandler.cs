using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OrchardCore.Client.Core.Config;
using OrchardCore.Client.Core.Extensions;
using System.Net;

namespace OrchardCore.Client.Core.Handlers
{
    public class GraphQLRequestHandler : HttpClientHandler
    {
        private IHttpClientFactory _httpClientFactory;
        private IHttpContextAccessor _contextAccessor;
        private GraphQLSettings _settings;

        public GraphQLRequestHandler(IHttpClientFactory httpClientFactory, GraphQLSettings settings, IHttpContextAccessor contextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _contextAccessor = contextAccessor;
            _settings = settings;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string token = await GetTokenAsync();
            request.Headers.Add(Constants.HeaderNames.Authorization, $"{Constants.HeaderNames.Bearer} {token}");
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private async Task<string> GetTokenAsync()
        {
            //Try get token from session
            if (_contextAccessor.HttpContext != null && _contextAccessor.HttpContext.Session != null)
            {
                var sessionToken = _contextAccessor.HttpContext.Session.GetObjectFromJson<OAuthToken>(Constants.Session.ApiToken);
                if (sessionToken != null && sessionToken.ExpiryDateTime > DateTime.UtcNow)
                {
                    return sessionToken.AccessToken;
                }
            }

            var tokenResponse = await GenerateApiToken<OAuthToken>();
            // populate expiry date time and reduce to 120 seconds less for time skew tolerance
            tokenResponse.ExpiryDateTime = DateTime.UtcNow.AddSeconds(Convert.ToInt32(tokenResponse.ExpiresIn) - 120);
            //Cache OAuth Token in session
            _contextAccessor.HttpContext.Session.SetObjectAsJson(Constants.Session.ApiToken, tokenResponse);

            return tokenResponse.AccessToken;
        }

        private async Task<TResponse> GenerateApiToken<TResponse>()
            where TResponse : class
        {
            var client = _httpClientFactory.CreateClient();
            var formData = new Dictionary<string, string>()
            {
                { Constants.FormData.ClientId, _settings.ClientId },
                { Constants.FormData.ClientSecret, _settings.ClientSecret },
                { Constants.FormData.GrantType, Constants.FormData.ClientCredentials }
            };
            var requestBody = new HttpRequestMessage(HttpMethod.Post, _settings.TokenEndPointUrl)
            {
                Content = new FormUrlEncodedContent(formData)
            };

            using (HttpResponseMessage response = await client.SendAsync(requestBody))
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    //remove secret key for logging values
                    formData.Remove(Constants.FormData.ClientSecret);

                    string errorDetails = $"StatusCode: {response.StatusCode},\n ResponsePhrase: {response.ReasonPhrase}\n TokenEndPointUrl: {_settings.TokenEndPointUrl}\n ResponseBody: {responseBody}\n FormData:{string.Join(Environment.NewLine, formData)}";
                    throw new WebException($"Unable to retrieve token. Response details are: {errorDetails}");
                }

                return JsonConvert.DeserializeObject<TResponse>(responseBody);
            }
        }
    }
}
