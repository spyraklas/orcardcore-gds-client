using Microsoft.AspNetCore.Http;
using OrchardCore.Client.Core.Extensions;

namespace OrchardCore.Client.Core.Helpers
{
    public class SessionCacheHelper : ICacheHelper
    {
        private IHttpContextAccessor _httpContentAccessor;
        private ISession _session;

        public SessionCacheHelper(IHttpContextAccessor httpContentAccessor)
        {
            _httpContentAccessor = httpContentAccessor;
            _session = _httpContentAccessor.HttpContext?.Session;

            if (_session == null)
                throw new Exception("Session is not configured. Failed using SessionCacheHelper");
        }

        public T GetValue<T>(string key) where T : class
        {
            return _session.GetObjectFromJson<T>(key);
        }

        public T SetValue<T>(string key, T value, TimeSpan expires) where T : class
        {
            _session.SetObjectAsJson(key, value);
            return value;
        }

        public bool Exist(string key)
        {
            byte[] value = null;
            return _session.TryGetValue(key, out value);
        }

        public bool Remove(string key)
        {
            _session.Remove(key);
            return true;
        }
    }
}
