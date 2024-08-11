using Lesson.Application.Interfaces.Services;
using Lesson.Application.Services.Models.Consts;
using Lesson.Application.Services.Models.Objects;
using Shared.Interfaces;
using Shared.Objects.Identity;

namespace Lesson.Api.Middleware;

public class UserClaimsMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUserClaimsService userClaimsService)
    {
        var user = context.User;
        if (user.Identity is { IsAuthenticated: true })
        {
            var userContext = new IdentityContext()
            {
                UserId = Guid.Parse(user.FindFirst(ClaimTypes.UserId)?.Value),
                Username = user.FindFirst(ClaimTypes.Username)?.Value,
                FirstName = user.FindFirst(ClaimTypes.FirstName)?.Value,
                LastName = user.FindFirst(ClaimTypes.LastName)?.Value,
                Email = user.FindFirst(ClaimTypes.Email)?.Value
            };

            userClaimsService.SetUserClaims(userContext);
        }

        await next(context);
    }
}