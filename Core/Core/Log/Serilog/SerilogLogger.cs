using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Core.Log.Serilog;

public class SerilogLogger : Core.Domain.Log.Interfaces.ILogger
{
    private ILogger _logger;
    public SerilogLogger(IConfigurationManager configurationManager)
    {
        var elasticSearchUrl = configurationManager.GetValue<string>("ElasticConfiguration:Uri");
        if (string.IsNullOrWhiteSpace(elasticSearchUrl))
            throw new NullReferenceException("Elasticsearch url not found");

        var loggerConfiguration = new LoggerConfiguration();
        var logger = loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
        {
            AutoRegisterTemplate = true,
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
            DetectElasticsearchVersion = false,
        }).CreateLogger();

        _logger = logger;
    }

    public Task Error(Exception ex)
    {
        _logger.Error(ex,"");
        return Task.CompletedTask;
    }

    public Task Error(Exception ex, string message)
    {
        _logger.Error(ex,message);
        return Task.CompletedTask;
    }

    public Task Info(string message)
    {
        _logger.Information(message);
       return Task.CompletedTask;
    }

    public Task Warning(string message)
    {
        _logger.Warning(message);
        return Task.CompletedTask;
    }
}