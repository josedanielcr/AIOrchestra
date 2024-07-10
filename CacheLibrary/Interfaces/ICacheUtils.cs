namespace CacheLibrary.Interfaces
{
    public interface ICacheUtils
    {
        void Set<T>(string key, T value, TimeSpan? expiry = null);
        Task<T> Get<T>(string key);
        void Remove(string key);
    }
}