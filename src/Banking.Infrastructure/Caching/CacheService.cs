using System.Text.Json;
using Banking.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Banking.Infrastructure.Caching;

public class CacheService : ICacheService
{   
    private readonly IDistributedCache _distributedCache;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        var json = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(json)) return null;

        return JsonSerializer.Deserialize<T>(json);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        var json = JsonSerializer.Serialize(value);
        
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        };

        await _distributedCache.SetStringAsync(key, json, options, cancellationToken);
    }
}
