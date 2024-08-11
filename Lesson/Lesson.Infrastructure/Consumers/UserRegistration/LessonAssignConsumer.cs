using Core.Log.Helpers;
using Lesson.Domain.Entities;
using Lesson.Domain.Interfaces.Repositories;
using MassTransit;
using Serilog;
using Serilog.Events;
using Shared.Events.LessonService.LessonAssignment;
using Shared.Interfaces;
using ILogger = Core.Domain.Log.Interfaces.ILogger;

namespace Lesson.Infrastructure.Consumers.UserRegistration;

public class LessonAssignConsumer(
    ILessonRepository lessonRepository,
    IUserLessonRepository userLessonRepository,
    IUserClaimsService userClaimsService,
    ILogger logger)
    : IConsumer<LessonAssignmentRequestedEvent>, IConsumer<Fault<LessonAssignmentRequestedEvent>>
{
    #region [Consume]

    public async Task Consume(ConsumeContext<LessonAssignmentRequestedEvent> context)
    {
        var lessons = await lessonRepository.GetAllAsync() ?? new List<Domain.Entities.Lesson>();
        
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
                CreatorID = context.Message.UserId,
                LessonId = assignedLesson.Id,
                UserId = context.Message.UserId
            });
        }

        await userLessonRepository.AddRangeAsync(userLessons);

        await PublishAssignedEvent(userLessons, context);
    }

    #endregion

    #region [Compensation]

    public async Task Consume(ConsumeContext<Fault<LessonAssignmentRequestedEvent>> context)
    {
        var exception = context.Message.Exceptions.ToException();
        await logger.Error(exception,"LessonAssignment Failed.");
        await userLessonRepository.RemoveAsync(x => x.UserId == context.Message.Message.UserId);
    }

    #endregion

    #region [Publish Events]

    private async Task PublishAssignedEvent(List<Domain.Entities.UserLesson> userLessons,
        ConsumeContext<LessonAssignmentRequestedEvent> context)
    {
        var lessonAssignedEvent = new LessonAssignedEvent()
        {
            UserId = context.Message.UserId,
            LessonIds = userLessons.Select(s => s.LessonId)
        };

        await context.Publish(lessonAssignedEvent);
    }

    #endregion

    #region [Helper Methods]

    private Domain.Entities.Lesson GetLesson(IEnumerable<Domain.Entities.Lesson> lessons, List<UserLesson> userLessons,
        int randomLessonIndex)
    {
        var assignedLesson = lessons.ElementAt(randomLessonIndex);
        var randomLesson = lessons.ElementAt(randomLessonIndex);
        if (userLessons.Exists(e => e.LessonId == randomLesson.Id))
        {
            randomLessonIndex = new Random().Next(0, lessons.Count());
            GetLesson(lessons, userLessons, randomLessonIndex);
        }

        return randomLesson;
    }

    #endregion
}