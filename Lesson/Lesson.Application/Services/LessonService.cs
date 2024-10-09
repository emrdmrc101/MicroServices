using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lesson.Application.Interfaces.Services;
using Lesson.Application.Services.Models.DTOs;
using Lesson.Application.Services.Models.DTOs.Input;
using Lesson.Application.Services.Models.DTOs.Objects;
using Lesson.Application.Services.Models.DTOs.Result;
using Lesson.Application.Services.Models.Objects;
using Lesson.Domain.Interfaces.Repositories;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Shared.Interfaces;

namespace Lesson.Application.Services;

public class LessonService(
    ILessonRepository lessonRepository,
    IUserClaimsService contextService,
    IMapperService mapperService
    )
    : ILessonService
{
    public async Task<GetLessonsResultDto> GetLessons(GetLessonsInputDto getLessonsInput)
    {
        GetLessonsResultDto result = new();

        var lessonsDbResult = await lessonRepository.GetAllAsync();

        var dbResult = lessonsDbResult.ToList();

        var lessons = mapperService.Map<List<Domain.Entities.Lesson>, List<LessonDto>>(dbResult.ToList());

        result.Lessons = lessons;
        return result;
    }

    public Task<GetMyLessonsResultDTO> GetMyLessons(string? userId)
    {
        throw new NotImplementedException();
    }

    public async Task<GetMyLessonsResultDTO> GetMyLessons(Guid userId)
    {
        GetMyLessonsResultDTO result = new();
        var lessonsDbResult = await lessonRepository.GetMyLessons(userId: userId);

        var lessons = mapperService.Map<List<Domain.Entities.Lesson>, List<LessonDto>>(lessonsDbResult.ToList());

        result.Lessons = lessons;
        return result;
    }
}