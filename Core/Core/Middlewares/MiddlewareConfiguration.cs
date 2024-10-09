using Microsoft.AspNetCore.Builder;

namespace Core.Middlewares;

public static class MiddlewareConfiguration
{
    public static  void AddExceptionHandlingMiddleware(this WebApplication webApplication)
    {
        webApplication.UseMiddleware<ExceptionHandlingMiddleware>();
    }
    
    public static  void AddTraceMiddleware(this WebApplication webApplication)
    {
        webApplication.UseMiddleware<TraceMiddleware>();
    }
    
    public static  void AddUserClaimsMiddleware(this WebApplication webApplication)
    {
        webApplication.UseMiddleware<UserClaimsMiddleware>();
    }
}