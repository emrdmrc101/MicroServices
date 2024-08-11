using Lesson.Application.Interfaces.Services;
using Lesson.Api.Models.Authentication.Request;
using Lesson.Api.Models.Authentication.Response;
using Lesson.Application.Services.Models.DTOs.Input;
using Lesson.Application.Services.Models.DTOs.Objects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Interfaces;
using LessonObject = Lesson.Api.Models.Lesson.Objects.LessonObject;

namespace Lesson.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class LessonController(
    ILessonService lessonService,
    IUserClaimsService contextService,
    IMapperService mapperService
    ) : BaseController
{
    [HttpPost("GetLessons")]
    public async Task<GetLessonResponse> GetLessons(GetLessonRequest request)
    {
        var lessonsServiceResult = await lessonService.GetLessons(new GetLessonsInputDto());

        var lessons = mapperService.Map<List<LessonDto>, List<LessonObject>>(lessonsServiceResult.Lessons);

        return new GetLessonResponse
        {
            Lessons = lessons
        };
    }

    [HttpGet("GetMyLessons")]
    public async Task<GetMyLessonsResponse> GetMyLessons()
    {
        var lessonsServiceResult = await lessonService.GetMyLessons(userId: contextService.UserContext.UserId);
        
        var lessons = mapperService.Map<List<LessonDto>, List<LessonObject>>(lessonsServiceResult.Lessons);

        return new GetMyLessonsResponse
        {
            Lessons = lessons
        };
    }
}