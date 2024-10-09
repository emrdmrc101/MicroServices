using System.Text.Json;
using Core.Domain.Cache.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Cache.Distributed.Redis;

public class RedisDistributedCache(IConfigurationManager configurationManager)
    : BaseRedisDistributedCache(configurationManager), IDistributedCache
{
    public Task Remove(string key)
    {
        throw new NotImplementedException();
    }

    public async Task<T> GetAsync<T>(string key)
    {
        string value = await Database.StringGetAsync(key);

        if (!value.IsNullOrEmpty())
            throw new InvalidCastException();

        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task<string?> GetAsync(string key)
    {
        var value = await Database.StringGetAsync(key);
        return value.HasValue ? value.ToString() : null;
    }

    public async Task SetAsync(string key, string value, TimeSpan? expiry)
    {
        await Database.StringSetAsync(key, value, expiry);
    }

    public Task<bool> Exist(string key)
    {
        throw new NotImplementedException();
    }
}