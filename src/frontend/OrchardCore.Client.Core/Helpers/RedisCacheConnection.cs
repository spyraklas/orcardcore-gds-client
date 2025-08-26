using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchardCore.Client.Core.Config;
using StackExchange.Redis;

namespace OrchardCore.Client.Core.Helpers
{
    public class RedisCacheConnection : ICacheConnection
    {
        private readonly ILogger<RedisCacheConnection> _logger;
        private readonly Lazy<IConnectionMultiplexer> _connectionMultiplexer;
        private readonly RedisCacheOptions _redisCacheOptions;

        public RedisCacheConnection(IOptions<RedisCacheOptions> redisCacheOptions, ILogger<RedisCacheConnection> logger)
        {
            ArgumentNullException.ThrowIfNull(redisCacheOptions.Value, nameof(redisCacheOptions));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            _logger = logger;
            _redisCacheOptions = redisCacheOptions.Value;

            _connectionMultiplexer = new Lazy<IConnectionMultiplexer>(() =>
            {
                ArgumentNullException.ThrowIfNull(_redisCacheOptions.ConnectionString, Constants.RedisCacheConnectionStringNull);
                return ConnectionMultiplexer.Connect(_redisCacheOptions.ConnectionString);
            });
        }

        public IConnectionMultiplexer GetConnection()
        {
            try
            {
                return _connectionMultiplexer.Value;
            }
            catch (RedisException e)
            {
                _logger.LogError(e, "Error connecting to Redis Cache");
            }

            return null;
        }
    }
}
