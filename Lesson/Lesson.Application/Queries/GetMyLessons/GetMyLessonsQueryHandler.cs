using Core.Domain.ZooKeeper;
using Core.ServiceDiscovery;
using Core.Tracing;
using Lesson.Application.Interfaces.Services;
using Lesson.Application.Queries.GetMyLessons.Responses;
using Lesson.Domain.Interfaces.Repositories;
using MediatR;
using Shared.Interfaces;

namespace Lesson.Application.Queries.GetMyLessons;

public class GetMyLessonsQueryHandler(
    ILessonRepository _lessonRepository,
    IUserClaimsService _contextService,
    IMapperService _mapperService,
    ActivityTracing _activityTracing,
    ZookeeperHelper zookeeperHelper
) : IRequestHandler<GetMyLessonsQuery, GetMyLessonsQueryResponse>
{
    public async Task<GetMyLessonsQueryResponse> Handle(GetMyLessonsQuery request, CancellationToken cancellationToken)
    {
        return await _activityTracing.ExecuteWithTracingAsync<GetMyLessonsQueryResponse>(
            nameof(GetMyLessonsQueryHandler),
            async () =>
            {
                // Zookeeper test
                string identityServiceUrl = await zookeeperHelper.GetServiceUrl(AppServices.Identity);
                
                GetMyLessonsQueryResponse response = new();

                var lessonsDbResult = await _lessonRepository.GetMyLessons(userId: _contextService.UserContext.UserId);

                var lessons =
                    _mapperService.Map<List<Domain.Entities.Lesson>, List<Queries.GetMyLessons.Responses.Lesson>>(
                        lessonsDbResult.ToList());

                response.Lessons = lessons;

                return response;
            });
    }
}