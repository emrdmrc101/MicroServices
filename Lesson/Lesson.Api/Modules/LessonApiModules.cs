using Autofac;
using Core.DependencyInjection;
using Lesson.Application.Interfaces.Services;
using Lesson.Application.Services;
using Shared.Interfaces;

namespace Lesson.Api.Modules;

public class LessonApiModules : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<LessonService>().As<ILessonService>().SingleInstance();
        builder.RegisterType<UserClaimsService>().As<IUserClaimsService>().SingleInstance();
        builder.RegisterType<MapperService>().As<IMapperService>().SingleInstance();
        
        base.Load(builder);
    }
}