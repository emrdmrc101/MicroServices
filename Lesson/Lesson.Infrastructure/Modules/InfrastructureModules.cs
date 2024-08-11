using Autofac;
using Core.DependencyInjection;
using Core.Log;
using Serilog;
using Serilog.Core;

namespace Lesson.Infrastructure.Modules;

public class InfrastructureModules : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        // builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
        
        base.Load(builder);
    }
}