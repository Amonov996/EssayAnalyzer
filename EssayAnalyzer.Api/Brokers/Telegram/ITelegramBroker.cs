using Telegram.Bot;

namespace EssayAnalyzer.Api.Brokers.Telegram;

public interface ITelegramBroker
{
    internal ITelegramBotClient TelegramBotClient { get; }
    internal CancellationTokenSource CancellationTokenSource { get;}
}