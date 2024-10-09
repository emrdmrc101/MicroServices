namespace Lesson.Application.Queries.GetLessons.Response;

public class Lesson
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTimeOffset? EndDate { get; set; }
}