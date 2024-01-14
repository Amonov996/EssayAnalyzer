using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EssayAnalyzer.Api.Services.Foundation.TelegramBot;

public partial class TelegramBotService
{
    private async Task HandleUpdateAsync(ITelegramBotClient telegramBotClient, Update update,
        CancellationToken cancellationToken)
    {
        var botClient = this.telegramBroker.TelegramBotClient;
        var message = update.Message;
        
        await botClient.SendChatActionAsync(
            chatId: message.Chat.Id,
            chatAction: ChatAction.Typing,
            cancellationToken: cancellationToken);
        
        var messageToBot = TelegramMessageValidate(message.Text);
        
        await botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: messageToBot,
            cancellationToken: cancellationToken);
    }
}
