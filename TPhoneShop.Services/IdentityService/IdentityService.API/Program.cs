using BuildingBlocks.Infrastructure.Authentication;
using BuildingBlocks.Infrastructure.Middlewares;
using BuildingBlocks.Infrastructure.Validation;
using IdentityService.Application;
using IdentityService.Infrastructure;
using IdentityService.Persistence;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddCurrentUser();

builder.Services.AddJwtAuthentication(configuration, builder.Environment);
builder.Services.AddFluentValidationBuildingBlocks();
builder.Services.AddApplication(configuration);
builder.Services.AddPersistence(configuration);
builder.Services.AddInfrastructure(configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
