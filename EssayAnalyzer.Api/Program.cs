using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Brokers.OpenAis;
using EssayAnalyzer.Api.Brokers.Storages;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IStorageBroker, StorageBroker>();
builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();
builder.Services.AddTransient<IOpenAiBroker, OpenAiBroker>();

var app = builder.Build();

builder.Services.AddDbContext<StorageBroker>();

app.MapGet("/", () => "Hello World!");
// Test changes
app.Run();