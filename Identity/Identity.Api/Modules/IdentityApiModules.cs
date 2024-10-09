using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Identity.Application.Interfaces.Services;
using Identity.Application.Services;
using Identity.Domain.Interfaces.Common;
using Identity.Infrastructure.DependencyInjection;
using Shared.Interfaces;

namespace Identity.Api.Modules;

public class IdentityApiModules : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
        builder.RegisterType<MapperService>().As<IMapperService>().SingleInstance();
        
        base.Load(builder);
    }
}