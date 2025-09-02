using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Turna96.Application;
using Turna96.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg.WriteTo.Console());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});
builder.Services.AddOpenTelemetry().WithTracing(t =>
{
    t.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Turna96.Api"));
    t.AddAspNetCoreInstrumentation();
    t.AddHttpClientInstrumentation();
    t.AddOtlpExporter();
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty);

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var api = app.MapGroup("/api/v1");
api.MapGet("/health", () => Results.Ok(new { status = "ok" }));
api.MapPost("/messages/send", () => Results.Ok());
api.MapGet("/messages/list", () => Results.Ok());
api.MapPost("/rooms/join", () => Results.Ok());

app.Run();
