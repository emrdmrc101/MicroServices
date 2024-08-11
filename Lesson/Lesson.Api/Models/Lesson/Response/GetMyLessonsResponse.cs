using Lesson.Api.Models.Common;
using Lesson.Api.Models.Lesson.Objects;

namespace Lesson.Api.Models.Authentication.Response;

public class GetMyLessonsResponse : BaseResponse
{
    public List<LessonObject> Lessons { get; set; } = new();
}