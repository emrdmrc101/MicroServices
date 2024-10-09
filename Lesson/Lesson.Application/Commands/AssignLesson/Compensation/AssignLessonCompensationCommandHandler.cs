using Core.Tracing;
using Lesson.Domain.Interfaces.Repositories;
using MediatR;

namespace Lesson.Application.Commands.AssignLesson.Compensation;

public class AssignLessonCompensationCommandHandler(
    IUserLessonRepository _userLessonRepository,
    ActivityTracing _activityTracing)
    : IRequestHandler<AssignLessonCompensationCommand, Unit>
{
    public async Task<Unit> Handle(AssignLessonCompensationCommand request, CancellationToken cancellationToken)
    {
        return await _activityTracing.ExecuteWithTracingAsync<Unit>(
            nameof(AssignLessonCompensationCommandHandler),
            async () =>
            {
                await _userLessonRepository.RemoveAsync(x => x.UserId == request.UserId);

                return Unit.Value;
            });
    }
}