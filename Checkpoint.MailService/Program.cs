using Checkpoint.MailService;
using Checkpoint.MailService.BackgroundJobs;
using Checkpoint.MailService.Consumers;
using Checkpoint.MailService.Data;
using Checkpoint.MailService.MailServices;
using Hangfire;
using Hangfire.MemoryStorage;
using MailKit.Net.Smtp;
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
builder.Services.AddSingleton<MailService>();
builder.Services.AddSingleton<SmtpClient>(y =>
{
    var smtpClient = new SmtpClient();
    smtpClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

    smtpClient.Authenticate(builder.Configuration.GetSection("MailInformation").Get<MailInformation>()!.Username, builder.Configuration.GetSection("MailInformation").Get<MailInformation>()!.Password);
    return smtpClient;
});
builder.Services.AddHangfireServer();
builder.Services.AddDbContext<MailDbContext>(y => y.UseNpgsql(builder.Configuration.GetConnectionString("checkpoint")));
builder.Services.AddMassTransit<IBus>(config =>
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

app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<NotSentMailJob>("fail-sent-mail-job", y => y.ExecuteJob(CancellationToken.None), "*/15 * * * * *");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
