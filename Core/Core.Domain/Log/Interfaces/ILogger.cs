namespace Core.Domain.Log.Interfaces;

public interface ILogger
{
    Task Error(Exception ex);
    Task Error(Exception ex, string message);
    Task Info(string message);
    Task Warning(string message);
}