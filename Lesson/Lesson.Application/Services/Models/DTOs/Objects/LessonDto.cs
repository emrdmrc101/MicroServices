namespace Lesson.Application.Services.Models.DTOs.Objects;

public class LessonDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTimeOffset? EndDate { get; set; }
}