using Carter;
using Checkpoint.API.BackgroundJobs;
using Checkpoint.API.Data;
using Checkpoint.API.Interfaces;
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
builder.Services.GetDataServices(builder.Configuration);

//using var scope = builder.Services.BuildServiceProvider().CreateScope();
//var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
//dbContext.Action.Add(new Checkpoint.API.Entities.Action()
//{
//    ActionPath = "getAllUser",
//    ControllerId = 1,
//    RequestType = Checkpoint.API.Enums.RequestType.Get,
//    Header = new List<Checkpoint.API.RequestPayloads.Header>()
//    {
//        new Checkpoint.API.RequestPayloads.Header()
//        {
//            Key="userId",
//            Value=1
//        }
//    }
//});
//await dbContext.SaveChangesAsync(CancellationToken.None);

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
