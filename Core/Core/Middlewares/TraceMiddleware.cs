using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Core.Middlewares;

public class TraceMiddleware
{
    private RequestDelegate Next;
    private IConfiguration Configuration;
    private static ActivitySource ActivitySource;
    public static string ServiceName { get; set; }
    public TraceMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        Next = next;
        Configuration = configuration;
        
        ServiceName = configuration.GetValue<string>("ServiceName");
        ActivitySource = new ActivitySource(ServiceName);
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        using var activity = ActivitySource.StartActivity(ServiceName, ActivityKind.Server);
        if (activity != null)
        {
            activity.SetTag("http.method", context.Request.Method);
            activity.SetTag("http.url", context.Request.Path);
            activity.SetTag("user.id", context.User.Identity?.Name ?? "anonymous");
        }
            
        var traceId = Activity.Current?.TraceId.ToString() ?? "no-trace-id";
        context.Response.Headers.Add("Trace-Id", traceId);
            
        await Next(context);
    }
}