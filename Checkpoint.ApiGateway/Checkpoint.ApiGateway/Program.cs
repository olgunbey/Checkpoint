using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelotSettings.json");
builder.Services.AddOcelot(builder.Configuration);
var app = builder.Build();

await app.UseOcelot();

app.Run();
