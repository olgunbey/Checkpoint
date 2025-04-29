using Checkpoint.IdentityServer;
using Checkpoint.IdentityServer.BackgroundJobs;
using Checkpoint.IdentityServer.Data;
using Checkpoint.IdentityServer.Policies;
using Hangfire;
using Hangfire.MemoryStorage;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddServices();

builder.Services.AddAuthorization(configure =>
configure.AddPolicy("CorporateAddRole", configureBuilder =>
configureBuilder.AddRequirements(new CorporateAddRoleRequirement())));


builder.Services.AddDbContext<IdentityDbContext>(y => y.UseNpgsql(builder.Configuration.GetConnectionString("checkpoint")));
builder.Services.AddMassTransit<IBus>(configure =>
{
    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(builder.Configuration.GetSection("AmqpConf")["Host"], config =>
        {
            config.Username(builder.Configuration.GetSection("AmqpConf")["Username"]!);
            config.Password(builder.Configuration.GetSection("AmqpConf")["Password"]!);

        });
    });
});
builder.Services.AddHangfire(y => y.UseMemoryStorage());
builder.Services.AddHangfireServer();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapScalarApiReference();


app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<RegisterOutboxJob>("registerOutbox", y => y.ExecuteJob(CancellationToken.None), "*/15 * * * * *");


app.MapOpenApi();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
