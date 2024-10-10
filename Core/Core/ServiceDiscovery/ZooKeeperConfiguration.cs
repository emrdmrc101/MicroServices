using System.Text;
using Core.Domain.ZooKeeper;
using Microsoft.Extensions.Configuration;
using org.apache.zookeeper;

namespace Core.ServiceDiscovery;

public class ZooKeeperConfiguration(
    ConfigurationManager configurationManager,
    ZooKeeperSettings zooKeeperSettings,
    ZooKeeper zooKeeper)
{
    public async Task Run()
    {
        string serviceUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS").Split(";").FirstOrDefault();
        var serviceData = Encoding.UTF8.GetBytes(serviceUrl);
        string parentPath = "/services";

        if (await zooKeeper.existsAsync(parentPath, false) is null)
            await zooKeeper.createAsync(parentPath, null, ZooDefs.Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT);
        
        if (await zooKeeper.existsAsync(zooKeeperSettings.Path, true) is null)
            await zooKeeper.createAsync(zooKeeperSettings.Path, serviceData, ZooDefs.Ids.OPEN_ACL_UNSAFE,
                CreateMode.EPHEMERAL);
        else
            await zooKeeper.setDataAsync(zooKeeperSettings.Path, serviceData, -1);
    }

    public class DefaultWatcher : Watcher
    {
        public override Task process(WatchedEvent @event)
        {
            Console.WriteLine(
                $"ZooKeeper Event: {@event.getState()}, Path: {@event.getPath()}, Type: {@event.GetType()}");
            return Task.CompletedTask;
        }
    }
}