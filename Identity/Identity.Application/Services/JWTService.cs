using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.Application.Interfaces.Services;
using Identity.Application.Services.Models.Consts;
using Identity.Application.Services.Models.DTOs;
using Identity.Application.Services.Models.Objects;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Application.Services;

public class JWTService(IConfiguration configuration) : IJWTService
{
    public  Task<GenerateTokenResultDTO> GenerateToken(GenerateTokenInputDTO inputDTO)
    {
        var tokenResult = TokenGenerator(inputDTO);
        var result = new GenerateTokenResultDTO()
        {
            Token = tokenResult.Token,
            ExpiryDate = tokenResult.ExpiryDate
        };

        return Task.FromResult(result);
    }
    
    #region [Helper Methods]

    private TokenObject TokenGenerator(GenerateTokenInputDTO inputDTO)
    {
            
        string securityKey = configuration.GetSection("Jwt:securityKey").Value?.ToString();
        byte[] jWtSecurity = Encoding.ASCII.GetBytes(securityKey);
        SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(jWtSecurity);

        TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = configuration.GetValue<bool>("Jwt:validateIssuerSigningKey"),
            IssuerSigningKey = symmetricSecurityKey,
            ValidateIssuer = configuration.GetValue<bool>("Jwt:validateIssuer"),
            ValidIssuer = configuration.GetValue<string>("Jwt:validIssuer"),
            ValidateAudience = configuration.GetValue<bool>("Jwt:validateAudience"),
            ValidAudience = configuration.GetValue<string>("Jwt:validAudience"),
            ValidateLifetime = configuration.GetValue<bool>("Jwt:validateLifetime"),
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = configuration.GetValue<bool>("Jwt:requireExpirationTime")
        };

        DateTime nowDateTime = DateTime.Now;
        var expireDate = nowDateTime.Add(TimeSpan.FromHours(10));
        var jwt = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("Jwt:validIssuer"),
            audience: configuration.GetValue<string>("Jwt:validAudience"),
            claims: new List<Claim>
            {
                new Claim(CustomClaimTypes.FirstName, inputDTO.FirstName),
                new Claim(CustomClaimTypes.LastName, inputDTO.LastName),
                new Claim(CustomClaimTypes.UserId, inputDTO.Id),
                new Claim(CustomClaimTypes.Username, inputDTO.UserName),
                new Claim(CustomClaimTypes.Email, inputDTO.Email)
            },
            notBefore: nowDateTime,
            expires: expireDate,
            signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
        );

        return new TokenObject()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwt),
            ExpiryDate = expireDate
        };
    }

    #endregion
}