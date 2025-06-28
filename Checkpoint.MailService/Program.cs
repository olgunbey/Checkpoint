using Checkpoint.MailService;
using Checkpoint.MailService.BackgroundJobs;
using Checkpoint.MailService.Consumers;
using Checkpoint.MailService.Data;
using Checkpoint.MailService.Interfaces;
using Checkpoint.MailService.MailServices;
using Hangfire;
using Hangfire.MemoryStorage;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddServices();
builder.Services.Configure<MailInformation>(builder.Configuration.GetSection("MailInformation"));
builder.Services.AddHangfire(y => y.UseMemoryStorage());
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddHangfireServer();
builder.Services.AddDbContext<MailDbContext>(y => y.UseNpgsql(builder.Configuration.GetConnectionString("checkpoint")));
builder.Services.AddMassTransit<IBus>(config =>
{
    config.AddConsumer<RegisterOutboxEventConsumer>();
    config.AddConsumer<UserTeamSelectedEventConsumer>();
    config.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration.GetSection("AmqpConf")["Host"], config =>
        {
            config.Username(builder.Configuration.GetSection("AmqpConf")["Username"]!);
            config.Password(builder.Configuration.GetSection("AmqpConf")["Password"]!);

        });

        configurator.ReceiveEndpoint(QueueConfigurations.RegisterOutboxQueue,
            conf => conf.ConfigureConsumer<RegisterOutboxEventConsumer>(context));

        configurator.ReceiveEndpoint(QueueConfigurations.Identity_Server_UserTeamSelected_Mail_Service, conf => conf.ConfigureConsumer<UserTeamSelectedEventConsumer>(context));
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<NotSentMailJob>("fail-sent-mail-job", y => y.ExecuteJob(CancellationToken.None), "*/15 * * * * *");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
