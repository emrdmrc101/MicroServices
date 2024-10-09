using System.Net;
using Core.Domain.Cache.DTOs;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Core.Cache.Distributed.Redis;

public abstract class BaseRedisDistributedCache
{
    protected IDatabase Database;

    private readonly IConfigurationManager _configurationManager;

    protected BaseRedisDistributedCache(IConfigurationManager configurationManager)
    {
        _configurationManager = configurationManager;

        var sentinelConnection = ConnectionMultiplexer.SentinelConnectAsync(SentinelConfiguration, Console.Out).Result;

        var masterConnection =
            sentinelConnection.GetSentinelMasterConnection(MasterConfiguration(sentinelConnection.GetServers()),
                Console.Out);

        Database = masterConnection.GetDatabase();
    }

    private string ServiceName
    {
        get
        {
            var serviceName = _configurationManager.GetValue<string>("Cache:Distributed:RedisSentinel:ServiceName");

            if (string.IsNullOrWhiteSpace(serviceName))
                throw new ArgumentNullException("Redis sentinel serviceName could not be found");

            return serviceName;
        }
    }

    private ConfigurationOptions SentinelConfiguration
    {
        get
        {
            var configuration = new ConfigurationOptions()
            {
                CommandMap = CommandMap.Sentinel,
                EndPoints = new EndPointCollection(),
                ServiceName = ServiceName,
                AbortOnConnectFail = false,
            };

            var sentinelAddress = _configurationManager.GetSection("Cache:Distributed:RedisSentinel:Address")
                .Get<List<RedisConfigurationDTO>>();

            foreach (var dnsEndPoint in sentinelAddress.Select(s => new DnsEndPoint(s.Host, s.Port)))
            {
                configuration.EndPoints.Add(dnsEndPoint);
            }

            return configuration;
        }
    }


    private ConfigurationOptions MasterConfiguration(IServer[] servers)
    {
        var configuration = new ConfigurationOptions()
        {
            AllowAdmin = true,
            ServiceName = ServiceName,
            CommandMap = CommandMap.Default,
            AbortOnConnectFail = false,
            EndPoints = { GetMasterEndpoint(servers) }
        };


        return configuration;
    }

    private System.Net.EndPoint GetMasterEndpoint(IServer[] servers)
    {
        System.Net.EndPoint? masterEndpoint = null;

        foreach (var server in servers)
        {
            if (!server.IsConnected)
                continue;

            masterEndpoint = server.SentinelGetMasterAddressByName(ServiceName);
            break;
        }

        if (masterEndpoint is null)
            throw new ArgumentNullException("Redis Sentinel Master endpoint could not be found.");

        return masterEndpoint;
    }
}