using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Options;
using OrcardCore.Client.Web.Entities;
using OrchardCore.Client.Core.Config;
using OrchardCore.Client.Core.Factory;
using OrchardCore.Client.Core.Helpers;

namespace OrcardCore.Client.Web.Repositories
{
    public class PageRepository : BaseRepository, IPageRepository
    {
        public PageRepository(IOptions<CacheOptions> cacheOptions, ICacheHelper cacheHelper, IGraphQLClient graphQLClient) 
            : base(cacheOptions, cacheHelper, graphQLClient)
        {
        }

        public async Task<List<Page>> GetPages()
        {
            string query = @$"
                        query pages 
                        {{
                          page(status: PUBLISHED) {{
                            displayText
                            alias {{
                              alias
                            }}
                            path
                            render
                            javaScript
                            seoMeta {{
                              metaKeywords
                              metaDescription
                              customMetaTags {{
                                charset
                                content
                                name
                                httpEquiv
                                property        
                              }}
                            }}
                          }}
                        }}";

            var result = await QueryAsync(query, Constants.CacheKey.Pages);
            return ConvertDynamicToObject<List<Page>>(result.page);
        }
    }
}
