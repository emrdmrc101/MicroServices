using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaOrchestration.Application.StateMachines.UserRegistration;
using SagaOrchestration.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<SagaOrchestrationDbContext>();
builder.Services.AddMassTransit(cfg =>
{
    cfg.AddSagaStateMachine<UserRegistrationStateMachine, SagaOrchestration.Domain.State.UserRegistrationSagaState>()
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Optimistic;
            r.AddDbContext<DbContext, SagaOrchestrationDbContext>((provider, builderOptions) =>
            {
                builderOptions.UseNpgsql(builder.Configuration.GetValue<string>("Database:connectionString"),
                    m =>
                    {
                        m.MigrationsHistoryTable($"__{nameof(SagaOrchestrationDbContext)}");
                    });
            });

            r.UsePostgres();
        });

    cfg.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", a =>
        {
            a.Username("guest");
            a.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();