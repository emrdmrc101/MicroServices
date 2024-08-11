using Autofac;
using Identity.Application.Interfaces.Services;
using Identity.Application.Services;
using Identity.Domain.Interfaces.Repositories;
using Identity.Infrastructure.DependencyInjection;
using Identity.Infrastructure.Repositories;

namespace Identity.Application.Modules;

public class ApplicationModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerDependency();
        builder.RegisterType<JWTService>().As<IJWTService>().InstancePerDependency();
        base.Load(builder);
    }
}