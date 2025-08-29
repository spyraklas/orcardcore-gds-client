namespace OrcardCore.Client.Web
{
    public class Constants
    {
        public const string RedisCacheConnectionStringNull = "Redis Cache Connection String is Null";
        public const string CrmPrefix = "/cms/";

        public class CacheKey
        {
            public const string Pages = "Pages";
        }

        public class JsonProperties
        {
            public const string Title = "displayText";
            public const string Path = "path";
            public const string HtmlBody = "render";
            public const string FileName = "alias.alias";
            public const string JavaScript = "javaScript";
            public const string SeoMeta = "seoMeta";
            public const string MetaKeywords = "metaKeywords";
            public const string MetaDescription = "metaDescription";
            public const string PageTitle = "pageTitle";
            public const string CustomMetaTags = "customMetaTags";
            public const string Name = "name";
            public const string Content = "content";
            public const string HttpEquiv = "httpEquiv";
            public const string Property = "property";
            public const string Charset = "charset";
        }

        public class Session
        {
            public const string ApiToken = "ApiToken";
        }

        public class HeaderNames
        {
            public const string Authorization = "Authorization";
            public const string Bearer = "Bearer";
        }

        public class FormData
        {
            public const string ClientId = "client_id";
            public const string ClientSecret = "client_secret";
            public const string GrantType = "grant_type";
            public const string ClientCredentials = "client_credentials";
        }
    }
}
