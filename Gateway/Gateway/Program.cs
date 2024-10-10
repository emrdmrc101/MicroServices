using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.Identity;
using Core.Middlewares;
using Core.Modules;
using Core.ServiceDiscovery;
using Core.Tracing;
using Gateway.Modules;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

#region [Register Modules]

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(
    b =>
    {
        b.RegisterModule<GatewayModule>();
        b.RegisterModule<BaseModule>();
        b.RegisterModule(new CoreModule(builder.Configuration));
    });

#endregion

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile($"ocelot.json", false, true);
builder.Services.AddOcelot();
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddOpenTelemetryAndJaeger(builder.Configuration);

var app = builder.Build();
app.RunZookeeper();
app.AddExceptionHandlingMiddleware();
app.AddTraceMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseOcelot().Wait();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();