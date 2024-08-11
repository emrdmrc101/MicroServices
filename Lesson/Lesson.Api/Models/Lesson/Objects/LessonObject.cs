namespace Lesson.Api.Models.Lesson.Objects;

public class LessonObject
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTimeOffset? EndDate { get; set; }
}