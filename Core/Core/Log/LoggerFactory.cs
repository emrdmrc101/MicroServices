using Core.Domain.Log.Enums;
using Core.Domain.Log.Interfaces;
using Core.Log.Serilog;
using Microsoft.Extensions.Configuration;

namespace Core.Log;

public static class LoggerFactory
{
    public static ILogger CreateLogger(IConfigurationManager configuration)
    {
        string loggerType = configuration.GetValue<string>("Logging:LoggerType");
        if (string.IsNullOrWhiteSpace(loggerType))
            throw new NullReferenceException("LoggerType not found");
        
        Enum.TryParse<LoggerType>(loggerType, out LoggerType outLoggerType);
        return outLoggerType switch
        {
            LoggerType.Serilog => new SerilogLogger(configuration),
            _ => throw new NotImplementedException($"LoggerType '{loggerType}' is not implemented."),
        };

        return default;
    }
}