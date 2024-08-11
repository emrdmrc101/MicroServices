using Lesson.Application.Interfaces.Services;
using Mapster;

namespace Lesson.Application.Mapper;

public class MapperService : IMapperService
{
    public TDestination Map<TSource, TDestination>(TSource source)
    {
        return source.Adapt<TDestination>();
    }
}