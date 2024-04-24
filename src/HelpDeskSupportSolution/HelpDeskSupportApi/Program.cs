using HelpDeskSupportApi.Services;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenTelemetry()
    .ConfigureResource(b => b.AddService("issues-api"))
    .WithTracing(b =>
    {
        b.AddAspNetCoreInstrumentation();
        b.AddHttpClientInstrumentation();
        b.AddZipkinExporter();
        b.AddHttpClientInstrumentation();
        b.AddConsoleExporter();
        b.SetSampler(new AlwaysOnSampler());
    })
    .WithMetrics(opts =>
    {
        opts.AddPrometheusExporter();
        opts.AddHttpClientInstrumentation();
        opts.AddRuntimeInstrumentation();

        opts.AddAspNetCoreInstrumentation();
    });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(sp => TimeProvider.System); // if anything needs a time provider, use the real system clock.
builder.Services.AddScoped<IProvideTheBusinessClock, HolidaysBusinessClock>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", async ([FromServices] IProvideTheBusinessClock clock) =>
{
    var ifWeAreOpen = await clock.AreWeCurrentOpenAsync();
    if (ifWeAreOpen)
    {
        return Results.Ok(new SupportResponseModel("Bob Smith", "555-1212", "bob@company.com"));
    }
    else
    {
        return Results.Ok(new SupportResponseModel("Support Pros", "(800) BAD-CODE", "help@support-pros.com"));
    }
});
app.MapPrometheusScrapingEndpoint();
app.Run();


public record SupportResponseModel(string Name, string Phone, string Email);

public partial class Program { }