using EssayAnalyzer.Api.Brokers.Storages;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddDbContext<StorageBroker>();

app.MapGet("/", () => "Hello World!");
// Test changes
app.Run();