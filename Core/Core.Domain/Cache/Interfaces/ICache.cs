namespace Core.Domain.Cache.Interfaces;

public interface ICache
{
    Task Remove(string key);
    Task<T> Get<T>(string key);
    Task Set(string key, object value);
    Task<bool> Exist(string key);
}