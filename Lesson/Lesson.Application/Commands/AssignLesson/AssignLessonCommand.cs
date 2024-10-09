using MediatR;

namespace Lesson.Application.Commands.AssignLesson;

public class AssignLessonCommand(Guid userId) : IRequest<bool>
{ 
    public Guid UserId { get; } = userId;
}