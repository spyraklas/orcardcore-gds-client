namespace OrchardCore.Client.Core.Config
{
    public class CacheOptions
    {
        public bool UseCache { get; set; }

        public bool EnabledRedisCache { get; set; }

        public int CacheExpirySeconds { get; set; }

        public int SessionTimeoutInMinutes { get; set; }
    }
}
