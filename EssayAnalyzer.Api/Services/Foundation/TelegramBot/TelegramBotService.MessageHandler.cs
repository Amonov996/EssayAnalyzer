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
        
        const int typingIndicatorDelayInMilliseconds = 3000;
        var incomingTelegramMessage = update.Message;
        var chatId = incomingTelegramMessage!.Chat.Id;
        var messageId = incomingTelegramMessage.MessageId;
        string botResponseMessage;

        await BotChatActionAsync(botClient, chatId, ChatAction.Typing, cancellationToken);
        
        if (ValidateTypeOfMessage(update))
        {
            botResponseMessage = NotAllowedMessage.Replace("{USER}", update.Message.Chat.FirstName);
            var storedMessage = await SendMessageAsync(botClient, chatId, botResponseMessage, cancellationToken);
            
            // Deleting disallowed messages
            await Task.Delay(typingIndicatorDelayInMilliseconds, cancellationToken);
            await DeleteMessageAsync(botClient, chatId, messageId, cancellationToken);
            
            // Deleting stored messages
            await Task.Delay(typingIndicatorDelayInMilliseconds, cancellationToken);
            await DeleteMessageAsync(botClient, chatId, storedMessage, cancellationToken);
        }
        else
        {
            await BotChatActionAsync(botClient, chatId, ChatAction.Typing, cancellationToken);
            
            botResponseMessage = TelegramMessageValidate(incomingTelegramMessage.Text, update);
            await SendMessageAsync(botClient, chatId, botResponseMessage, cancellationToken);
        }
    }
}
