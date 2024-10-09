using Autofac;
using MediatR;
using Module = Autofac.Module;

namespace Core.Modules;

public class BaseModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
    }
}