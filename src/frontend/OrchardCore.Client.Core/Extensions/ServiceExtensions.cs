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
        public static void AddOrchardCoreClient(this IServiceCollection services, IConfiguration configuration, bool enableRedis = false)
        {
            services.Configure<OrchardCoreOptions>(options => configuration.GetSection(nameof(OrchardCoreOptions)).Bind(options));
            services.Configure<RedisCacheOptions>(options => configuration.GetSection(nameof(RedisCacheOptions)).Bind(options));

            //Register caching handlers
            if (enableRedis)
            {
                services.AddSingleton<ICacheConnection, RedisCacheConnection>();
                services.AddSingleton<ICacheHelper, RedisCacheHelper>();
            }
            else
            {
                services.AddSingleton<ICacheHelper, MemoryCacheHelper>();
            }

            var orchardCoreOptions = new OrchardCoreOptions();
            configuration.GetSection(nameof(OrchardCoreOptions)).Bind(orchardCoreOptions);

            services.AddHttpContextAccessor();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(orchardCoreOptions.SessionTimeoutInMinutes);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            services.AddHttpClient();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

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
