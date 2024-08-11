using Core.Domain.Cache.Interfaces;

namespace Core.Cache.Redis;

public class RedisCache  : ICache
{
    public Task Remove(string key)
    {
        throw new NotImplementedException();
    }

    public Task<T> Get<T>(string key)
    {
        throw new NotImplementedException();
    }

    public Task Set(string key, object value)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Exist(string key)
    {
        throw new NotImplementedException();
    }
}