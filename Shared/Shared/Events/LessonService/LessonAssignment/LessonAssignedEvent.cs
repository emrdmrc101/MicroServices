namespace Shared.Events.LessonService.LessonAssignment;

public class LessonAssignedEvent
{
    public Guid UserId { get; set; }
    public IEnumerable<Guid> LessonIds { get; set; }
}