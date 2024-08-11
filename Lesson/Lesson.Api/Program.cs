using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.DependencyInjection.Modules;
using Core.Identity;
using Lesson.Api.Middleware;
using Lesson.Api.Modules;
using Lesson.Application.Modules;
using Lesson.Infrastructure.Consumers.UserRegistration;
using Lesson.Infrastructure.Data;
using Lesson.Infrastructure.Modules;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using LoggerFactory = Core.Log.LoggerFactory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

#region [MassTransit]

builder.Services.AddMassTransit(x =>
{   
    x.AddConsumer<LessonAssignConsumer>();
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

builder.Services.AddMassTransitHostedService();

#endregion

#region [Register Modules]
string elasticSearchUrl = builder.Configuration.GetValue<string>("ElasticConfiguration:Uri");
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(
    b =>
    {
        
        b.RegisterModule<LessonApiModules>();
        b.RegisterModule<ApplicationModule>();
        b.RegisterModule<InfrastructureModules>();
        b.RegisterType<LessonDbContext>().AsSelf();
        b.RegisterModule<CoreModule>();
        b.Register<Core.Domain.Log.Interfaces.ILogger>(r => LoggerFactory.CreateLogger(builder.Configuration)).As<Core.Domain.Log.Interfaces.ILogger>().SingleInstance();
    });

#endregion

#region [Entityframework]

builder.Services.AddDbContext<LessonDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetValue<string>("Database:connectionString"));
});

#endregion

// Set Identity Configurations
builder.Services.AddJwt(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<UserClaimsMiddleware>();

// app.UseMiddleware<UserContextMiddleware>();
var dbContext = app.Services.GetService<LessonDbContext>();


//if (dbContext != null) await dbContext.Database.MigrateAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

// app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();