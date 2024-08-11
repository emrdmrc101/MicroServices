using Identity.Application.Interfaces.Services;
using Identity.Application.Services.Models.DTOs;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces.Common;
using Identity.Domain.Interfaces.Repositories;
using MassTransit;
using Shared.Events.IdentityService.UserRegistration;

namespace Identity.Application.Services;

public class UserService(IUserRepository userRepository, IJWTService JWTService, IMapperService mapperService, IBus bus)
    : IUserService
{
    public async Task<LoginResultDTO> Login(LoginInputDTO loginInput)
    {
        LoginResultDTO result = new();

        var findUser = await userRepository
            .FirstOrDefaultAsync(f => f.UserName == loginInput.UserName && f.Password == loginInput.Password);

        if (findUser is null)
        {
            result.ErrorMessage = "Username or password is wrong";
            return result;
        }

        var generateTokenInputModel = mapperService.Map<User, GenerateTokenInputDTO>(findUser);

        var tokenGeneratorResult = await JWTService.GenerateToken(generateTokenInputModel);

        result.Token = tokenGeneratorResult.Token;
        result.ExpiryDate = tokenGeneratorResult.ExpiryDate;
        result.Succeeded = true;
        return result;
    }

    public async Task<UserRegisterResultDto?> UserRegister(UserRegisterInputDto registerInput)
    {
        var checkUserForEmail = await userRepository
            .FirstOrDefaultAsync(x => x.Email == registerInput.Email || x.UserName == registerInput.UserName);
        if (checkUserForEmail is not null)
            return default;

        var user = mapperService.Map<UserRegisterInputDto, User>(registerInput);

        await userRepository.AddAsync(user);

        await PublishRegisteredEvent(user);

        return new UserRegisterResultDto() { UserId = user.Id.ToString() };
    }
    
    private async Task PublishRegisteredEvent(User user)
    {
        var registeredEvent = new UserRegisteredEvent
        {
            UserId = user.Id
        };
        await bus.Publish(registeredEvent);
    }
}