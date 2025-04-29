using Carter;
using Checkpoint.API.Data;
using FluentValidation;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));

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
