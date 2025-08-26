namespace OrchardCore.Client.Core.Config
{
    public class RedisCacheOptions
    {
        public string CacheKeyPrefix { get; set; }

        public string ConnectionString { get; set; }

        public bool RedisCacheEnabled { get; set; }

        public int WorkerThreads { get; set; }

        public int IocpThreads { get; set; }
    }
}
