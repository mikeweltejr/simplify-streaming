using Microsoft.OpenApi.Models;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using DynamoDB.DAL.App.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});
builder.Services.AddHealthChecks();

var region = builder.Configuration["DynamoDBConfig:Region"];
var serviceURL = builder.Configuration["DynamoDBConfig:ServiceURL"];
var config = new AmazonDynamoDBConfig();
AmazonDynamoDBClient client;

if (region != null)
{
    config.RegionEndpoint = RegionEndpoint.GetBySystemName(region);
    client = new AmazonDynamoDBClient(config);
}
else
{
    config.ServiceURL = serviceURL;
    var credentials = new BasicAWSCredentials(accessKey: "Test", secretKey: "Test");
    client = new AmazonDynamoDBClient(credentials, config);
}

var db = new DynamoDBContext(client);
builder.Services.AddSingleton<IDynamoDBContext>(db);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UsePathBase(new PathString("/api"));
app.MapHealthChecks("/health");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

if(!app.Environment.IsEnvironment("Testing"))
{
    var seedData = new SeedData(db);
    await seedData.SeedAllTitles();
    await seedData.SeedStreamingTitles();
}

await app.RunAsync();

public partial class Program { }
