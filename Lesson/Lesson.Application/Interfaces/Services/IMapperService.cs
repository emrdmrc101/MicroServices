namespace Lesson.Application.Interfaces.Services;

public interface IMapperService
{
    public TDestination Map<TSource, TDestination>(TSource source);
}