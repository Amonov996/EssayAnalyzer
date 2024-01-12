using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Brokers.OpenAis;
using EssayAnalyzer.Api.Brokers.Storages;
using EssayAnalyzer.Api.Brokers.Telegram;
using EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes;
using EssayAnalyzer.Api.Services.Foundation.Essays;
using EssayAnalyzer.Api.Services.Foundation.Results;
using EssayAnalyzer.Api.Services.Foundation.TextInputFormatter;
using EssayAnalyzer.Api.Services.Foundation.Users;
using EssayAnalyzer.Api.Services.Orchestration.EssayAnalyser;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Add(new TextInputFormatterService());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// dbContext
builder.Services.AddDbContext<StorageBroker>();

//life cycle
//Brokers
builder.Services.AddTransient<IStorageBroker, StorageBroker>();
builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();
builder.Services.AddTransient<IOpenAiBroker, OpenAiBroker>();
builder.Services.AddTransient<ITelegramBroker, TelegramBroker>();

//Services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IEssayService, EssayService>();
builder.Services.AddTransient<IResultService, ResultService>();
builder.Services.AddTransient<IEssayAnalysisService, EssayAnalysisService>();
builder.Services.AddTransient<IEssayAnalyseFeedbackOrchestrationService, EssayAnalyserFeedbackOrchestrationService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();


app.Run();