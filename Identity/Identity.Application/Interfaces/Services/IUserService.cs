using Identity.Application.Services.Models.DTOs;

namespace Identity.Application.Interfaces.Services;

public interface IUserService
{
    public Task<LoginResultDTO> Login(LoginInputDTO loginInput);
    Task<UserRegisterResultDto?> UserRegister(UserRegisterInputDto registerInput);
}