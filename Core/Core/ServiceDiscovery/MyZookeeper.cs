using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Core.ServiceDiscovery;

public static class MyZookeeper
{
    public static void RunZookeeper(this WebApplication webApplication)
    {
        webApplication.Lifetime.ApplicationStarted.Register(async () =>
        {
            var zooKeeperConfiguration = webApplication.Services.GetRequiredService<ZooKeeperConfiguration>();

            await zooKeeperConfiguration.Run();
        });
    }
}