using Checkpoint.IdentityServer.SagaOrchestration.SagaContext;
using Checkpoint.IdentityServer.SagaOrchestration.StateInstances;
using Checkpoint.IdentityServer.SagaOrchestration.StateMachines;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<IdentityServerStateDbContext>(conf => conf.UseNpgsql(builder.Configuration.GetConnectionString("saga")));

builder.Services.AddMassTransit<IBus>(conf =>
{
    conf.AddSagaStateMachine<IdentityServerStateMachine, IdentityServerStateInstance>()
        .EntityFrameworkRepository(opt =>
        {
            opt.ConcurrencyMode = ConcurrencyMode.Pessimistic;
            opt.AddDbContext<DbContext, IdentityServerStateDbContext>();
            opt.UsePostgres();
        });
    conf.UsingRabbitMq((context, configure) =>
    {
        configure.Host(builder.Configuration.GetSection("AmqpConf")["Host"], config =>
        {
            config.Username(builder.Configuration.GetSection("AmqpConf")["Username"]);
            config.Password(builder.Configuration.GetSection("AmqpConf")["Password"]);

        });
        configure.ReceiveEndpoint(QueueConfigurations.StateMachine, e =>
        {
            e.ConfigureSaga<IdentityServerStateInstance>(context);
        });
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
