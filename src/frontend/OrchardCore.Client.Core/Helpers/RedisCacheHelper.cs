using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchardCore.Client.Core.Helpers
{
    public class RedisCacheHelper : ICacheHelper
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly ILogger<RedisCacheHelper> _logger;

        public RedisCacheHelper(ICacheConnection connection, ILogger<RedisCacheHelper> logger)
        {
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));

            _logger = logger;
            _connection = connection.GetConnection();
        }

        public bool Exist(string key)
        {
            try
            {
                IDatabase cache = _connection.GetDatabase();
                return cache.KeyExists(key);
            }
            catch (RedisException e)
            {
                _logger.LogError(e, "Error checking if Cache Has Key");
            }

            return false;
        }

        public T GetValue<T>(string key) where T : class
        {
            try
            {
                IDatabase cache = _connection?.GetDatabase() ?? throw new ArgumentNullException(nameof(_connection), "Redis cache connection is null");
                if (cache.KeyExists(key))
                {
                    RedisValueWithExpiry val = cache.StringGetWithExpiry(key);

                    if (!val.Value.IsNull)
                    {
                        return JsonConvert.DeserializeObject<T>(val.Value);
                    }
                }
            }
            catch (RedisException e)
            {
                _logger.LogError(e, "Error getting entity from Cache");
            }

            return default;
        }

        public bool Remove(string key)
        {
            try
            {
                IDatabase cache = _connection.GetDatabase();
                return cache.KeyDelete(key);
            }
            catch (RedisException e)
            {
                _logger.LogError(e, "Error removing key from Cache");
            }

            return false;
        }

        public T SetValue<T>(string key, T value, TimeSpan expires) where T : class
        {
            try
            {
                IDatabase cache = _connection.GetDatabase();
                cache.StringSet(key, JsonConvert.SerializeObject(value), expires);
                return value;
            }
            catch (RedisException e)
            {
                _logger.LogError(e, "Error Saving into Cache");
            }

            return null;
        }
    }
}
