using System.Diagnostics;

namespace Core.Tracing;

public class ActivityTracing(ActivitySource activitySource)
{
    public async Task<T> ExecuteWithTracingAsync<T>(string operationName, Func<Task<T>> action)
    {
        using var activity = activitySource.StartActivity(operationName, ActivityKind.Server);
        activity?.SetTag("traceId", Activity.Current?.TraceId.ToString());

        return await action();
    }
    
    public T ExecuteWithTracing<T>(string operationName, Func<T> action)
    {
        using var activity = activitySource.StartActivity(operationName, ActivityKind.Server);
        activity?.SetTag("traceId", Activity.Current?.TraceId.ToString());

        return action();
    }
    
    public async Task ExecuteWithTracingAsync(string operationName,  Func<Task> action)
    {
        using var activity = activitySource.StartActivity(operationName, ActivityKind.Server);
        activity?.SetTag("traceId", Activity.Current?.TraceId.ToString());
        
        await action();
    }
}