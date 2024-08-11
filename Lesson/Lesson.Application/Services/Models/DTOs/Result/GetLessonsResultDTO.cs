using Lesson.Application.Services.Models.DTOs.Objects;

namespace Lesson.Application.Services.Models.DTOs.Result;

public class GetLessonsResultDto
{
    public List<LessonDto> Lessons { get; set; } = new();
}