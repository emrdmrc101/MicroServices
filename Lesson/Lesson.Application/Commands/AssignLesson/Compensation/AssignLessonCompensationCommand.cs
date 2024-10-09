using MediatR;

namespace Lesson.Application.Commands.AssignLesson.Compensation;

public class AssignLessonCompensationCommand(Guid userId) : IRequest<Unit>
{
    public Guid UserId { get;} = userId;
}