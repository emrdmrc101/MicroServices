using MassTransit;
using MassTransit.Transports;

namespace Identity.Infrastructure.MassTransit;

public class CustomMessageNameFormatter : IEntityNameFormatter
{
    public string FormatEntityName<T>()
    {
        return $"{typeof(T).Name}";
    }
}