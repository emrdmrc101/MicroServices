using Autofac;
using Core.DependencyInjection;
using Core.Log;
using Lesson.Application.Interfaces.Services;
using Lesson.Application.Services;
using Lesson.Domain.Interfaces.Repositories;
using Lesson.Infrastructure.Repositories;
using Serilog;

namespace Lesson.Application.Modules;

public class ApplicationModule : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<LessonRepository>().As<ILessonRepository>().InstancePerDependency();
        builder.RegisterType<UserLessonRepository>().As<IUserLessonRepository>().InstancePerDependency();
        // builder.RegisterType<SerilogOptions>().As<ILogger>().InstancePerDependency();
        base.Load(builder);
    }
}