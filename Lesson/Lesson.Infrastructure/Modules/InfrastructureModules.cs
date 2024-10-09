using Autofac;
using Core.Modules;
using Lesson.Domain.Interfaces.Repositories;
using Lesson.Infrastructure.Repositories;

namespace Lesson.Infrastructure.Modules;

public class InfrastructureModules : BaseModule
{
    protected override void Load(ContainerBuilder builder)
    {   
        builder.RegisterType<LessonRepository>().As<ILessonRepository>().InstancePerDependency();
        builder.RegisterType<UserLessonRepository>().As<IUserLessonRepository>().InstancePerDependency();
        base.Load(builder);
    }
}