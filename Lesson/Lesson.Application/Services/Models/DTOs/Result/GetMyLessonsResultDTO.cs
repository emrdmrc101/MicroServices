using Lesson.Application.Services.Models.DTOs.Objects;
using Lesson.Application.Services.Models.Objects;

namespace Lesson.Application.Services.Models.DTOs;

public class GetMyLessonsResultDTO
{
    public List<LessonDto> Lessons { get; set; } = new();
}