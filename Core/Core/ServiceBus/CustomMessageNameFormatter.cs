using MassTransit;

namespace Core.ServiceBus;

public class CustomMessageNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>()
    {
        return $"{typeof(T).Name}";
    }
}