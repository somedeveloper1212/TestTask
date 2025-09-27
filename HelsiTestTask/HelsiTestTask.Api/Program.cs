using System.Text.Json.Serialization;
using HelsiTestTask.Api.Common.ExceptionHandlers;
using HelsiTestTask.Api.Common.Responses;
using HelsiTestTask.Application.Configurations;
using HelsiTestTask.Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMongoClient(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
// builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context => 
            new BadRequestObjectResult(ApiResponse.From(context.ModelState));
    })
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Helsi API", Version = "v1" });

    c.UseOneOfForPolymorphism();
    c.EnableAnnotations();
});

builder.Services.AddProblemDetails();

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseRouting();
app.MapControllers();

await app.RunAsync();