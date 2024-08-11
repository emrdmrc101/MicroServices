using Lesson.Application.Services.Models.DTOs;
using Lesson.Application.Services.Models.DTOs.Input;
using Lesson.Application.Services.Models.DTOs.Result;

namespace Lesson.Application.Interfaces.Services;

public interface ILessonService
{
    public Task<GetLessonsResultDto> GetLessons(GetLessonsInputDto getLessonsInput);
    public Task<GetMyLessonsResultDTO> GetMyLessons(Guid userId);
}