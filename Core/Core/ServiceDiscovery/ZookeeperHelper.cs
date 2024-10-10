using System.Text;
using Core.Domain.ZooKeeper;
using org.apache.zookeeper;

namespace Core.ServiceDiscovery;

public class ZookeeperHelper(ZooKeeperSettings zooKeeperSettings, ZooKeeper zooKeeper)
{
    public async Task<string> GetServiceUrl(string serviceName)
    {
        if (string.IsNullOrWhiteSpace(serviceName) || !zooKeeperSettings.Services.ContainsKey(serviceName))
            throw new Exception($"Not found service. {serviceName}");

        var serviceData = await zooKeeper.getDataAsync(zooKeeperSettings.Services[serviceName], false);
        
        if (serviceData?.Data is null)
            throw new Exception($"Not found service data. {serviceName}");
        
        string serviceUrl = Encoding.UTF8.GetString(serviceData.Data);

        return serviceUrl;
    }
}