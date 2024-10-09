namespace Lesson.Application.Queries.GetMyLessons.Responses;

public class GetMyLessonsQueryResponse
{
    public List<Lesson> Lessons { get; set; } = new();
}