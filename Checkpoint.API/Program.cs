using Carter;
using Checkpoint.API.BackgroundJobs;
using Checkpoint.API.DependencyInjections;
using Checkpoint.API.Features.Project.Command;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Scalar.AspNetCore;
using Shared.Middlewares;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddSingleton<IAuthorizationHandler, AddProject.AuthorizationTransaction.Handler>();
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




var app = builder.Build();


app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<Request>("request-job", req => req.ExecuteJob(CancellationToken.None), "*/15 * * * * *");
RecurringJob.AddOrUpdate<Analysis>("analysis-job", req => req.ExecuteJob(CancellationToken.None), "*/30 * * * * *");

app.MapScalarApiReference();

app.MapOpenApi();
app.UseCors();

app.UseHttpsRedirection();
app.UseHangfireDashboard();
app.UseAuthentication();
app.UseMiddleware<AdminCheck>();
app.UseAuthorization();

app.MapCarter();
app.Run();
