using Checkpoint.MailService;
using Checkpoint.MailService.Consumers;
using Checkpoint.MailService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddServices();
builder.Services.AddDbContext<MailDbContext>(y => y.UseNpgsql(builder.Configuration.GetConnectionString("checkpoint")));
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<RegisterOutboxEventConsumer>();
    config.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration.GetSection("AmqpConf")["Host"], config =>
        {
            config.Username(builder.Configuration.GetSection("AmqpConf")["Username"]!);
            config.Password(builder.Configuration.GetSection("AmqpConf")["Password"]!);

        });

        configurator.ReceiveEndpoint(QueueConfigurations.RegisterOutboxQueue,
            conf => conf.ConfigureConsumer<RegisterOutboxEventConsumer>(context));
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
