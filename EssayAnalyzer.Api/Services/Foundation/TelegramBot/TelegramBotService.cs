using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Brokers.Telegram;
using EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace EssayAnalyzer.Api.Services.Foundation.TelegramBot;

public partial class TelegramBotService : ITelegramBotService
{
    private readonly ITelegramBroker telegramBroker;
    private readonly ILoggingBroker loggingBroker;
    private readonly IEssayAnalysisService essayAnalysisService;
    
    public TelegramBotService(ITelegramBroker telegramBroker, 
        ILoggingBroker loggingBroker, 
        IEssayAnalysisService essayAnalysisService)
    {
        this.telegramBroker = telegramBroker;
        this.loggingBroker = loggingBroker;
        this.essayAnalysisService = essayAnalysisService;
    }


    public Task StartBotAsync()
    {
        var botClient = this.telegramBroker.TelegramBotClient;
        var cancellationToken = this.telegramBroker.CancellationTokenSource.Token;
        
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cancellationToken
        );

        return Task.CompletedTask;
    }
}