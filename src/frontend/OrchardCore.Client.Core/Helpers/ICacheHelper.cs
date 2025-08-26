namespace OrchardCore.Client.Core.Helpers
{
    public interface ICacheHelper
    {
        T SetValue<T>(string key, T value, TimeSpan expires) where T : class;

        T GetValue<T>(string key) where T : class;

        bool Exist(string key);

        bool Remove(string key);
    }
}
