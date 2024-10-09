using Core.Tracing;
using Lesson.Domain.Entities;
using Lesson.Domain.Interfaces.Repositories;
using MassTransit;
using MediatR;
using Shared.Events.LessonService.LessonAssignment;

namespace Lesson.Application.Commands.AssignLesson;

public class AssignLessonCommandHandler(
    ILessonRepository _lessonRepository,
    IUserLessonRepository _userLessonRepository,
    ActivityTracing _activityTracing,
    IBus _bus) : IRequestHandler<AssignLessonCommand, bool>
{
    public async Task<bool> Handle(AssignLessonCommand request, CancellationToken cancellationToken)
    {
        return await _activityTracing.ExecuteWithTracingAsync<bool>(
            nameof(AssignLessonCommandHandler),
            async () =>
            {
                var lessons = await _lessonRepository.GetAllAsync() ?? new List<Domain.Entities.Lesson>();

                var userLessons = new List<Domain.Entities.UserLesson>();
                var random = new Random();

                for (var i = 0; i < 4; i++)
                {
                    var randomLessonIndex = random.Next(0, lessons.Count());
                    var assignedLesson = GetLesson(lessons, userLessons, randomLessonIndex);

                    userLessons.Add(new UserLesson()
                    {
                        Id = Guid.NewGuid(),
                        CreationDate = DateTimeOffset.UtcNow,
                        CreatorID = request.UserId,
                        LessonId = assignedLesson.Id,
                        UserId = request.UserId
                    });
                }

                await _userLessonRepository.AddRangeAsync(userLessons);

                await PublishAssignedEvent(userLessons, request.UserId);
                return true;
            });
    }

    #region [Publish Events]

    private async Task PublishAssignedEvent(List<Domain.Entities.UserLesson> userLessons, Guid userId)
    {
        var lessonAssignedEvent = new LessonAssignedEvent()
        {
            UserId = userId,
            LessonIds = userLessons.Select(s => s.LessonId)
        };

        await _bus.Publish(lessonAssignedEvent);
    }

    #endregion

    #region [Helper Methods]

    private Domain.Entities.Lesson GetLesson(IEnumerable<Domain.Entities.Lesson> lessons, List<UserLesson> userLessons,
        int randomLessonIndex)
    {
        return _activityTracing.ExecuteWithTracing<Domain.Entities.Lesson>(
            nameof(GetLesson),
            () =>
            {
                var randomLesson = lessons.ElementAt(randomLessonIndex);
                if (!userLessons.Exists(e => e.LessonId == randomLesson.Id)) return randomLesson;
                randomLessonIndex = new Random().Next(0, lessons.Count());
                GetLesson(lessons, userLessons, randomLessonIndex);

                return randomLesson;
            });
    }

    #endregion
}