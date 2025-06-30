using Checkpoint.IdentityServer.BackgroundJobs;
using Checkpoint.IdentityServer.DependencyInjections;
using Hangfire;
using Scalar.AspNetCore;
using Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddAuthorizationService();

var app = builder.Build();

app.MapScalarApiReference();
app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<RegisterOutboxJob>("registerOutbox", y => y.ExecuteJob(CancellationToken.None), "*/15 * * * * *");
app.MapOpenApi();
app.UseCors();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<AdminCheck>();
app.UseAuthorization();

app.MapControllers();

app.Run();
