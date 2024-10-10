using System.Diagnostics;
using Autofac;
using Core.Cache.Distributed;
using Core.Domain.Cache.Interfaces;
using Core.Domain.ZooKeeper;
using Core.Log;
using Core.ServiceDiscovery;
using Core.Tracing;
using MediatR;
using Microsoft.Extensions.Configuration;
using org.apache.zookeeper;

namespace Core.Modules;

public class CoreModule(ConfigurationManager configurationManager) : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        string serviceName = configurationManager.GetValue<string>("ServiceName");

        builder.Register<Core.Domain.Log.Interfaces.ILogger>(r => LoggerFactory.CreateLogger(configurationManager))
            .As<Core.Domain.Log.Interfaces.ILogger>().SingleInstance();
        builder.Register<IDistributedCache>(r => DistributedCacheFactory.CreateCache(configurationManager))
            .As<IDistributedCache>().InstancePerDependency();
        builder.Register(r => new ActivityTracing(new ActivitySource(serviceName))).SingleInstance();

        #region [MediatR]

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        builder.RegisterAssemblyTypes(assemblies)
            .AsClosedTypesOf(typeof(IRequestHandler<,>))
            .AsImplementedInterfaces();

        builder.RegisterType<Mediator>()
            .As<IMediator>()
            .InstancePerLifetimeScope();

        #endregion

        // ZooKeeperSettings
        var zooKeeperSettings = configurationManager.GetSection("ZooKeeper").Get<ZooKeeperSettings>();
        var zooKeeper = new ZooKeeper(zooKeeperSettings.Url, 100000, new ZooKeeperConfiguration.DefaultWatcher());
        builder.Register(r => zooKeeper).SingleInstance();
        builder.Register(r => zooKeeperSettings).SingleInstance();
        builder.Register(r => new ZookeeperHelper(zooKeeperSettings, zooKeeper)).SingleInstance();
        builder.Register(r => new ZooKeeperConfiguration(configurationManager,zooKeeperSettings,zooKeeper));

        base.Load(builder);
    }
}