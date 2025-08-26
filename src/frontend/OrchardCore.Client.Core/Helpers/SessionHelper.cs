using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OrchardCore.Client.Core.Config;

namespace OrchardCore.Client.Core.Helpers
{
    public class SessionHelper : ISessionHelper
    {
        private readonly ICacheHelper _cacheHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SessionOptions _sessionOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionHelper"/> class.
        /// </summary>
        /// <param name="sessionOptions">Configuration session settings.</param>
        /// <param name="httpContextAccessor">Http context accessor instance.</param>
        /// <param name="cacheService">Redis cache service instance.</param>
        public SessionHelper(IOptions<SessionOptions> sessionOptions, IHttpContextAccessor httpContextAccessor, ICacheHelper cacheHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _cacheHelper = cacheHelper;
            _sessionOptions = sessionOptions.Value;
        }

        /// <summary>
        /// Removes session values from cache.
        /// </summary>
        /// <param name="key">Session key.</param>
        /// <returns>true or false if session state deleted successfully.</returns>
        public bool DeleteSessionState(string key)
        {
            string sessionId = GetSessionId();

            if (!string.IsNullOrEmpty(sessionId))
            {
                return _cacheHelper.Remove($"{key}/{sessionId}");
            }

            return true;
        }

        /// <summary>
        /// Get a session state from cache.
        /// </summary>
        /// <param name="key">Session key.</param>
        /// <typeparam name="TModel">Session model type.</typeparam>
        /// <returns>Session model value return null if key cant be found.</returns>
        public TModel GetSessionState<TModel>(string key)
            where TModel : class
        {
            string sessionId = GetSessionId();

            if (!_cacheHelper.Exist($"{key}/{sessionId}"))
            {
                return null;
            }

            var result = _cacheHelper.GetValue<TModel>($"{key}/{sessionId}");

            if (result != null)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Set a session state in cache.
        /// </summary>
        /// <typeparam name="TModel">Session model type.</typeparam>
        /// <param name="key">Session key.</param>
        /// <param name="sessionModel">Session model instace.</param>
        /// <returns>ession model value.</returns>
        public TModel SetSessionState<TModel>(string key, TModel sessionModel)
            where TModel : class
        {
            string sessionId = GetSessionId();
            var result = _cacheHelper.SetValue($"{key}/{sessionId}", sessionModel, TimeSpan.FromSeconds(_sessionOptions.ExpireTime));

            return result;
        }

        /// <summary>
        /// Try to geet the session value from a cookie. If cant get the value creates a cookie with a new guid value.
        /// </summary>
        /// <returns>returns a string of a guid session id.</returns>
        private string GetSessionId()
        {
            //Try to get id value from cookie
            var id = string.Empty;
            if ((_httpContextAccessor.HttpContext?.Request?.Cookies?.TryGetValue(_sessionOptions.CookieName, out id)).Value)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    return id;
                }
            }

            //Add cookie with new guid
            id = Guid.NewGuid().ToString();
            var options = new CookieOptions()
            {
                Secure = true,
                IsEssential = true,
                HttpOnly = true,
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append(_sessionOptions.CookieName, id, options);

            return id;
        }
    }
}
