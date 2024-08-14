using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Repository.Infra.CacheStorage;

public class CacheService : ICacheService
{
    private readonly IDatabase _cache;
    public CacheService(IConnectionMultiplexer redis)
    {
        _cache = redis.GetDatabase();
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var objectString = await _cache.StringGetAsync(key);

        return string.IsNullOrWhiteSpace(objectString) ? default(T) : JsonSerializer.Deserialize<T>(objectString);
    }

    public async Task SetAsync<T>(string key, T data, TimeSpan? expiry = null)
    {
        var objectString = JsonSerializer.Serialize(data);
        await _cache.StringSetAsync(key, objectString);
    }
    
}