using Core.Cache.Distributed.Redis;
using Core.Domain.Cache.Enums;
using Core.Domain.Cache.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Core.Cache.Distributed;

public static class DistributedCacheFactory
{
    public static IDistributedCache CreateCache(IConfigurationManager configuration)
    {
        string cacheType = configuration.GetValue<string>("Cache:Distributed:Type");
        if (string.IsNullOrWhiteSpace(cacheType))
            throw new NullReferenceException("Distributed cache type not found");
       
        Enum.TryParse<DistributedCacheType>(cacheType, out DistributedCacheType outLoggerType);
        return outLoggerType switch
        {
            DistributedCacheType.Redis => new RedisDistributedCache(configuration),
            _ => throw new NotImplementedException($"Distributed cache type '{cacheType}' is not implemented."),
        };
    }
}