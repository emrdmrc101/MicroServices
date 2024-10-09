using Core.Tracing;
using Lesson.Application.Interfaces.Services;
using Lesson.Application.Queries.GetLessons.Response;
using Lesson.Domain.Interfaces.Repositories;
using MediatR;

namespace Lesson.Application.Queries.GetLessons;

public class GetLessonsQueryHandler(
    ILessonRepository _lessonRepository,
    IMapperService _mapperService,
    ActivityTracing _activityTracing
) : IRequestHandler<GetLessonsQuery, GetLessonsQueryResponse>
{
    public async Task<GetLessonsQueryResponse> Handle(GetLessonsQuery request, CancellationToken cancellationToken)
    {
        return await _activityTracing.ExecuteWithTracingAsync<GetLessonsQueryResponse>(
            nameof(GetLessonsQueryHandler),
            async () =>
            {
                GetLessonsQueryResponse response = new();

                var lessonsDbResult = await _lessonRepository.GetAllAsync();
                var dbResult = lessonsDbResult.ToList();
                var lessons = _mapperService.Map<List<Domain.Entities.Lesson>, List<Response.Lesson>>(dbResult.ToList());

                response.Lessons = lessons;
                return response;
            });
    }
}