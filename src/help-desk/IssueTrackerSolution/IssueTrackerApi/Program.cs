using FluentValidation;
using IssueTrackerApi.Controllers.Issues;
using Marten;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var databaseConnectionString = builder.Configuration.GetConnectionString("data") ?? throw new Exception("No Connection String");
builder.Services.AddMarten(options =>
{
    options.Connection(databaseConnectionString);
}).UseLightweightSessions();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<CreateIssueRequestModelValidator>();

// stuff above this line - configuration of api and all the stuff inside of it
var app = builder.Build();
//stuff after this line - how it takes requests and makes them into responses

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
