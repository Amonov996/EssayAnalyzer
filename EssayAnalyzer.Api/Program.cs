using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Brokers.OpenAis;
using EssayAnalyzer.Api.Brokers.Storages;
using EssayAnalyzer.Api.Services.Foundation.Essays;
using EssayAnalyzer.Api.Services.Foundation.Results;
using EssayAnalyzer.Api.Services.Foundation.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dbContext
builder.Services.AddDbContext<StorageBroker>();
//life cycle
builder.Services.AddTransient<IStorageBroker, StorageBroker>();
builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();
builder.Services.AddTransient<IOpenAiBroker, OpenAiBroker>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IEssayService, EssayService>();
builder.Services.AddTransient<IResultService, ResultService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();


app.Run();