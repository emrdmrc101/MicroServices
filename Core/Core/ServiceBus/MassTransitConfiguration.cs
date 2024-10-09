using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Core.ServiceBus;

public static class MassTransitConfiguration
{
    public static void AddMassTransit(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMassTransit(x =>
        {
            x.AddConsumers(AppDomain.CurrentDomain.GetAssemblies());
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", a =>
                {
                    a.Username("guest");
                    a.Password("guest");
                });

                // cfg.MessageTopology.SetEntityNameFormatter(new CustomMessageNameFormatter());
                cfg.ConfigureEndpoints(context);
            });
        });
    }
}