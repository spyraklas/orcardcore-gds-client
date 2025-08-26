using GraphQL.Client.Abstractions;
using Newtonsoft.Json;
using OrchardCore.Client.Core.Helpers;

namespace OrchardCore.Client.Core.Factory
{
    public abstract class BaseRepository
    {
        protected ICacheHelper _cacheHelper;
        protected IGraphQLClient _graphQLClient;

        public BaseRepository(ICacheHelper cacheHelper, IGraphQLClient graphQLClient)
        {
            _cacheHelper = cacheHelper;
            _graphQLClient = graphQLClient;
        }

        protected T ConvertDynamicToObject<T>(dynamic obj) where T : class
        {
            string json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
