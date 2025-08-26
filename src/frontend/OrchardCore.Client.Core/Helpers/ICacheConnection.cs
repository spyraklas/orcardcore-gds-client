using StackExchange.Redis;

namespace OrchardCore.Client.Core.Helpers
{
    public interface ICacheConnection
    {
        public IConnectionMultiplexer GetConnection();
    }
}
