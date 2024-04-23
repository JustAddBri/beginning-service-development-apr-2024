using HelpDeskSupportApi.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(sp => TimeProvider.System);
builder.Services.AddScoped<IProvideTheBusinessClock, StandardBusinessClock>();

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
app.Run();

public record SupportResponseModel(string Name, string Phone, string Email);

public partial class Program { }
