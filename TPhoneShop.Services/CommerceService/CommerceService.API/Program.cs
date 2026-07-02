using BuildingBlocks.Infrastructure.Authentication;
using BuildingBlocks.Infrastructure.Middlewares;
using BuildingBlocks.Infrastructure.Validation;
using CommerceService.Application;
using CommerceService.Infrastructure;
using CommerceService.Persistence;
using CommerceService.ReadModel;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddCurrentUser();
builder.Services.AddFluentValidationBuildingBlocks();
builder.Services.AddJwtAuthentication(configuration, builder.Environment);

builder.Services.AddApplication(configuration);
builder.Services.AddInfrastructure(configuration);
builder.Services.AddPersistence(configuration);
builder.Services.AddReadModel(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
