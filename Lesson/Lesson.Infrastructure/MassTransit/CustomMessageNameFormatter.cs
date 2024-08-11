using MassTransit;

namespace Lesson.Infrastructure.MassTransit;

public class CustomMessageNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>()
    {
        return $"{typeof(T).Name}";
    }
}