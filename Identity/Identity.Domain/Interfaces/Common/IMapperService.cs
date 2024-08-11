namespace Identity.Domain.Interfaces.Common;

public interface IMapperService
{
    TDestination Map<TSource, TDestination>(TSource source);
}