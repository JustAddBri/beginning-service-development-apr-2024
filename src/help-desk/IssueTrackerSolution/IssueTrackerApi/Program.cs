using FluentValidation;
using IssueTrackerApi;
using IssueTrackerApi.Controllers.Issues;
using IssueTrackerApi.Services;
using Marten;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var databaseConnectionString = builder.Configuration.GetConnectionString("data") ?? throw new Exception("No Connection String");
builder.Services.AddMarten(options =>
{
    options.Connection(databaseConnectionString);
}).UseLightweightSessions();

builder.Services.AddValidatorsFromAssemblyContaining<CreateIssueRequestModelValidator>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var apiUrl = builder.Configuration.GetValue<string>("api-url") ?? throw new Exception("Need the Support Api Url");
builder.Services.AddHttpClient<SupportHttpClient>(client =>
{
    client.BaseAddress = new Uri(apiUrl);
}).AddPolicyHandler(BasicSrePolicies.GetDefaultRetryPolicy())
    .AddPolicyHandler(BasicSrePolicies.GetDefaultCircuitBreaker());
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
