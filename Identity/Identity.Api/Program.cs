using Autofac;
using Autofac.Extensions.DependencyInjection;
using Identity.Api.Modules;
using Identity.Application.Modules;
using Identity.Infrastructure.Data;
using LoggerFactory = Core.Log.LoggerFactory;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region [MassTransit]

builder.Services.AddMassTransit(x =>
{
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

#endregion


#region [Register Modules]

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(
    b =>
    {
        b.RegisterModule<IdentityApiModules>();
        b.RegisterModule<ApplicationModule>();
        b.RegisterType<IdentityDbContext>().AsSelf();
        b.Register<Core.Domain.Log.Interfaces.ILogger>(r => LoggerFactory.CreateLogger(builder.Configuration)).As<Core.Domain.Log.Interfaces.ILogger>().SingleInstance();
    });

#endregion

builder.Services.AddDbContext<IdentityDbContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetValue<string>("Database:connectionString"));
});

var app = builder.Build();

var dbContext = app.Services.GetService<IdentityDbContext>();

Console.WriteLine("Migrations...");
if (dbContext != null) await dbContext.Database.MigrateAsync();

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