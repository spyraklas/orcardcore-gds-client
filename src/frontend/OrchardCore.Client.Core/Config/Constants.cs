using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchardCore.Client.Core.Config
{
    public class Constants
    {
        public const string RedisCacheConnectionStringNull = "Redis Cache Connection String is Null";

        public class JsonProperties
        {
            public const string AccessToken = "access_token";
            public const string TokenType = "token_type";
            public const string ExpiresIn = "expires_in";
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
