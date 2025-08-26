namespace OrchardCore.Client.Core.Helpers
{
    public interface ISessionHelper
    {
        /// <summary>
        /// Removes session values from cache.
        /// </summary>
        /// <param name="key">Session key.</param>
        /// <returns>true or false if session state deleted successfully.</returns>
        public bool DeleteSessionState(string key);

        /// <summary>
        /// Get a session state from cache.
        /// </summary>
        /// <param name="key">Session key.</param>
        /// <typeparam name="TModel">Session model type.</typeparam>
        /// <returns>Session model value.</returns>
        public TModel GetSessionState<TModel>(string key)
            where TModel : class;

        /// <summary>
        /// Set a session state in cache.
        /// </summary>
        /// <typeparam name="TModel">Session model type.</typeparam>
        /// <param name="key">Session key.</param>
        /// <param name="sessionModel">Session model instace.</param>
        /// <returns>Session model value.</returns>
        public TModel SetSessionState<TModel>(string key, TModel sessionModel)
            where TModel : class;
    }
}
