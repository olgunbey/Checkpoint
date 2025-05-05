using Carter;
using Checkpoint.API.BackgroundJobs;
using Checkpoint.API.Data;
using FluentValidation;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Shared.Hash;
using Shared.Middlewares;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddHangfire(y => y.UseMemoryStorage());
builder.Services.AddHangfireServer();
builder.Services.AddHttpClient();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = "https://localhost:7253",
            ValidAudience = "https://localhost:5000",
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Hashing.Hash("checkpointsecret"))
        };
    });
builder.Services.AddOpenApi();

builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.GetDataServices(builder.Configuration);

builder.Services.AddCarter();

var app = builder.Build();


app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<Request>("request-job", req => req.ExecuteJob(CancellationToken.None), "*/15 * * * * *");
//RecurringJob.AddOrUpdate<Analysis>("analysis-job", req => req.ExecuteJob(CancellationToken.None), "*/30 * * * * *");

app.MapScalarApiReference();

app.MapOpenApi();

app.UseHttpsRedirection();
app.UseHangfireDashboard();
app.UseAuthentication();
app.UseMiddleware<AdminCheck>();
app.UseAuthorization();

app.MapCarter();
app.Run();
