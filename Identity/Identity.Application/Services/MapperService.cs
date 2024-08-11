using Identity.Application.Interfaces.Services;
using Identity.Domain.Interfaces.Common;
using Mapster;

namespace Identity.Application.Services;

public class MapperService : IMapperService
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return source.Adapt<TDestination>();
    }
}