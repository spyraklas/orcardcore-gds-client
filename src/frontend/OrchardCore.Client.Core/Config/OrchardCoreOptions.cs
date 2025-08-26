namespace OrchardCore.Client.Core.Config
{
    public class OrchardCoreOptions
    {
        public int CacheExpirySeconds { get; set; }

        public int SessionTimeoutInMinutes { get; set; }

        public GraphQLSettings GraphQLSettings { get; set; }

        public bool CacheEnabled { get; set; }
    }
}
