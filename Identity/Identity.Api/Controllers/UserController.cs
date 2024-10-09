using Core.Tracing;
using Identity.Api.Models.Authentication.Request;
using Identity.Api.Models.Authentication.Response;
using Identity.Application.Interfaces.Services;
using Identity.Application.Services.Models.DTOs;
using Identity.Domain.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(
    IUserService _userService,
    IMapperService _mapperService,
    ActivityTracing _activityTracing) : BaseController
{
    [HttpPost("Login")]
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        return await _activityTracing.ExecuteWithTracingAsync<LoginResponse>(
            nameof(Login),
            async () =>
            {
                var loginServiceResult = await _userService.Login(new LoginInputDTO()
                {
                    UserName = request.UserName,
                    Password = request.Password
                });

                return new LoginResponse()
                {
                    Token = loginServiceResult.Token,
                    ExpiryDate = loginServiceResult.ExpiryDate,
                    ErrorMessage = loginServiceResult.ErrorMessage,
                    Succeeded = loginServiceResult.Succeeded
                };
            });
    }

    [HttpPost("Register")]
    public async Task<UserRegisterResponse> Register(UserRegisterRequest request)
    {
        return await _activityTracing.ExecuteWithTracingAsync<UserRegisterResponse>(
            nameof(Register),
            async () =>
            {
                var mappedModel = _mapperService.Map<UserRegisterRequest, UserRegisterInputDto>(request);

                var registerResult = await _userService.UserRegister(mappedModel);

                return new UserRegisterResponse()
                {
                    Success = !string.IsNullOrWhiteSpace(registerResult?.UserId)
                };
            });
    }
}