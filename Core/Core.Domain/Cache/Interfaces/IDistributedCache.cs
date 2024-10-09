namespace Core.Domain.Cache.Interfaces;

public interface IDistributedCache
{
    Task Remove(string key);
    Task<T> GetAsync<T>(string key);
    Task<string> GetAsync(string key);
    Task SetAsync(string key, string value, TimeSpan? expiry);
    Task<bool> Exist(string key);
}