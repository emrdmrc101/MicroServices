using MassTransit.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Npgsql;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Core.Tracing;

public static class OpenTelemetryJaegerConfiguration
{
    public static void AddOpenTelemetryAndJaeger(this IServiceCollection serviceCollection,
        IConfigurationManager configurationManager)
    {
        string serviceName = configurationManager.GetValue<string>("ServiceName");
        string jaegerUrl = configurationManager.GetValue<string>("Jaeger:Url");

        CreateElasticSearchIndex(configurationManager);

        serviceCollection.AddOpenTelemetry().WithTracing(tracerProviderBuilder =>
        {
            tracerProviderBuilder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddSource("EDemApp.*", DiagnosticHeaders.DefaultListenerName)
                .AddNpgsql()
                .AddEntityFrameworkCoreInstrumentation()
                .AddJaegerExporter(options => { options.Endpoint = new Uri(jaegerUrl); });
        });
    }

    private static void CreateElasticSearchIndex(IConfigurationManager configurationManager)
    {
        string elasticSearchUrl = configurationManager.GetValue<string>("ElasticConfiguration:Uri");
        var settings = new ConnectionSettings(new Uri(elasticSearchUrl));
        var elasticClient = new ElasticClient(settings);
        var createIndexResponse = elasticClient.Indices.Create("jaeger-logs", c => c
            .Map(m => m
                .AutoMap()
            )
        );

        if (createIndexResponse.IsValid)
            Console.WriteLine("'jaeger-logs' Index created successfully.");
    }
}