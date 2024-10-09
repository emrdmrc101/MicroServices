namespace Lesson.Application.Queries.GetMyLessons.Responses;

public class Lesson
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTimeOffset? EndDate { get; set; }
}