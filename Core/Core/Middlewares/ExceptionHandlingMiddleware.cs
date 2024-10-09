using System.Net;
using System.Text.Json;
using Core.Domain.Log.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.Error(exception, exception.Message);
        
        var response = context.Response;
        response.ContentType = "application/json";
        
        var responseModel = new
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Message = "An unexpected error occurred.",
            Detailed = exception.Message
        };
        
        response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = JsonSerializer.Serialize(responseModel);
        return response.WriteAsync(result);
    }
}