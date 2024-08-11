using Identity.Application.Services.Models.DTOs;

namespace Identity.Application.Interfaces.Services;

public interface IJWTService
{
    Task<GenerateTokenResultDTO> GenerateToken(GenerateTokenInputDTO inputDTO);
}