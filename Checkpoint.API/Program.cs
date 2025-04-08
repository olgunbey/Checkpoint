using Carter;
using Checkpoint.API.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.GetDataServices(builder.Configuration);

builder.Services.AddCarter();

var app = builder.Build();



app.MapScalarApiReference();

app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();
app.Run();
