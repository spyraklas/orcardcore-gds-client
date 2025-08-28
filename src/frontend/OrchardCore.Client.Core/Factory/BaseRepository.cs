using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OrchardCore.Client.Core.Config;
using OrchardCore.Client.Core.Helpers;

namespace OrchardCore.Client.Core.Factory
{
    public abstract class BaseRepository
    {
        protected ICacheHelper _cacheHelper;
        protected IGraphQLClient _graphQLClient;
        protected CacheOptions _cacheOptions;

        public BaseRepository(IOptions<CacheOptions> cacheOptions, ICacheHelper cacheHelper, IGraphQLClient graphQLClient)
        {
            _cacheHelper = cacheHelper;
            _graphQLClient = graphQLClient;
            _cacheOptions = cacheOptions.Value;
        }

        protected async Task<dynamic> QueryAsync(string query, string cacheKey = null)
        {
            dynamic result = null;

            if (_cacheOptions.UseCache)
                result = _cacheHelper.GetValue<dynamic>(cacheKey);

            if (result == null)
            {
                var response = await _graphQLClient.SendQueryAsync<dynamic>(query);
                result = response?.Data;

                if (_cacheOptions.UseCache)
                    _cacheHelper.SetValue(cacheKey, result, TimeSpan.FromSeconds(_cacheOptions.CacheExpirySeconds));
            }

            return result;
        }
        protected T ConvertDynamicToObject<T>(dynamic obj) where T : class
        {
            string json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
