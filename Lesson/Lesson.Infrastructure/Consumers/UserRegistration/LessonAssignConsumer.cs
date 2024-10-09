using Core.Tracing;
using Lesson.Application.Commands.AssignLesson;
using Lesson.Application.Commands.AssignLesson.Compensation;
using MassTransit;
using MediatR;
using Shared.Events.LessonService.LessonAssignment;

namespace Lesson.Infrastructure.Consumers.UserRegistration;

public class LessonAssignConsumer(
    IMediator _mediator,
    ActivityTracing _activityTracing)
    : IConsumer<LessonAssignmentRequestedEvent>, IConsumer<Fault<LessonAssignmentRequestedEvent>>
{
    #region [Consume]

    public async Task Consume(ConsumeContext<LessonAssignmentRequestedEvent> context)
    {
        await _activityTracing.ExecuteWithTracingAsync(
            nameof(LessonAssignConsumer),
            async () => { await _mediator.Send(new AssignLessonCommand(context.Message.UserId)); });
    }

    #endregion

    #region [Compensation]

    public async Task Consume(ConsumeContext<Fault<LessonAssignmentRequestedEvent>> context)
    {
        await _mediator.Send(new AssignLessonCompensationCommand(context.Message.Message.UserId));
    }

    #endregion
}