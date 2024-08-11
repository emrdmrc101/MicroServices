using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Core.Identity;

public static class JwtConfiguration
{
    public static void AddJwt(this IServiceCollection serviceCollection, ConfigurationManager configurationManager)
    {
        string securityKey = configurationManager.GetSection("Jwt:securityKey").Value?.ToString();
        byte[] jWtSecurity = Encoding.ASCII.GetBytes(securityKey);
        SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(jWtSecurity);

        serviceCollection.AddAuthentication(cfg =>
        {
            cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = configurationManager.GetValue<bool>("Jwt:validateIssuerSigningKey"),
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuer = configurationManager.GetValue<bool>("Jwt:validateIssuer"),
                ValidIssuer = configurationManager.GetValue<string>("Jwt:validIssuer"),
                ValidateAudience = configurationManager.GetValue<bool>("Jwt:validateAudience"),
                ValidAudience = configurationManager.GetValue<string>("Jwt:validAudience"),
                ValidateLifetime = configurationManager.GetValue<bool>("Jwt:validateLifetime"),
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = configurationManager.GetValue<bool>("Jwt:requireExpirationTime")
            };
        });
    }
}