using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.Client.Core.Config;
using OrchardCore.Client.Core.Handlers;
using OrchardCore.Client.Core.Helpers;

namespace OrchardCore.Client.Core.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddOrchardCoreClient(this IServiceCollection services, IConfiguration configuration)
        {
            //Bind configuration settings
            services.Configure<CacheOptions>(options => configuration.GetSection(nameof(CacheOptions)).Bind(options));
            services.Configure<OrchardCoreOptions>(options => configuration.GetSection(nameof(OrchardCoreOptions)).Bind(options));
            services.Configure<RedisCacheOptions>(options => configuration.GetSection(nameof(RedisCacheOptions)).Bind(options));

            //Register caching handlers
            var cacheOptions = new CacheOptions();
            configuration.GetSection(nameof(OrchardCoreOptions)).Bind(cacheOptions);
            if (cacheOptions.UseCache)
            {
                if (cacheOptions.EnabledRedisCache)
                {
                    services.AddSingleton<ICacheConnection, RedisCacheConnection>();
                    services.AddSingleton<ICacheHelper, RedisCacheHelper>();
                }
                else
                {
                    services.AddSingleton<ICacheHelper, MemoryCacheHelper>();
                }
            }
            else
            {
                services.AddSingleton<ICacheHelper, SessionCacheHelper>();
            }

            //configure session and httpcontext accessor and httpclient factory
            services.AddHttpContextAccessor();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(cacheOptions.SessionTimeoutInMinutes);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            services.AddHttpClient();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            //configure GraphQL client
            var orchardCoreOptions = new OrchardCoreOptions();
            configuration.GetSection(nameof(OrchardCoreOptions)).Bind(orchardCoreOptions);
            services.AddScoped<IGraphQLClient>(s =>
            {
                var option = new GraphQLHttpClientOptions()
                {
                    EndPoint = new Uri(orchardCoreOptions.GraphQLSettings.ApiUrl),
                    HttpMessageHandler = new GraphQLRequestHandler(s.GetService<IHttpClientFactory>(), orchardCoreOptions.GraphQLSettings, s.GetService<IHttpContextAccessor>())
                };
                var client = new GraphQLHttpClient(option, new NewtonsoftJsonSerializer());
                return client;
            });
        }

        public static void ConfigureThreadForRedis(this IServiceCollection services, RedisCacheOptions redisCacheOptions, ILogger logger)
        {
            if (ThreadPool.SetMinThreads(redisCacheOptions.WorkerThreads, redisCacheOptions.IocpThreads))
            {
                logger.LogTrace(
                    "ConfigureMinimumThreads: Minimum configuration value set. IOCP = {0} and WORKER threads = {1}",
                    redisCacheOptions.IocpThreads,
                    redisCacheOptions.WorkerThreads);
            }
            else
            {
                logger.LogWarning("ConfigureMinimumThreads: The minimum number of threads was not changed");
            }

        }
    }
}
