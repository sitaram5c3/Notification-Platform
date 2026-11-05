using Microsoft.Extensions.Caching.Memory;
using NotificationPlatform.Application.Abstractions;

namespace NotificationPlatform.Infrastructure.Caching;

public class InMemoryCacheService(IMemoryCache cache) : ICacheService
{
    public bool TryGetValue<T>(string key, out T? value)
    {
        if (cache.TryGetValue(key, out var result) && result is T casted)
        {
            value = casted;
            return true;
        }

        value = default;
        return false;
    }

    public void Set<T>(string key, T value, TimeSpan ttl)
    {
        cache.Set(key, value, ttl);
    }

    public void Remove(string key)
    {
        cache.Remove(key);
    }
}
