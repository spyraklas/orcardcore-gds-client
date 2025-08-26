using Microsoft.Extensions.Caching.Memory;

namespace OrchardCore.Client.Core.Helpers
{
    public class MemoryCacheHelper : ICacheHelper
    {
        private IMemoryCache _memoryCache;

        public MemoryCacheHelper(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T GetValue<T>(string key) where T : class
        {
            return _memoryCache.Get<T>(key);
        }

        public T SetValue<T>(string key, T value, TimeSpan expires) where T : class
        {
            return _memoryCache.Set(key, value, expires);
        }

        public bool Exist(string key)
        {
            return _memoryCache.Get(key) != null;
        }

        public bool Remove(string key)
        {
            _memoryCache.Remove(key);
            return true;
        }
    }
}
