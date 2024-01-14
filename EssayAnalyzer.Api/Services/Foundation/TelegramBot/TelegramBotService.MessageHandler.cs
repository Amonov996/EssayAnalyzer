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
        var chatId = incomingTelegramMessage.Chat.Id;
        var messageId = incomingTelegramMessage.MessageId;
        string botResponseMessage;
        
        await botClient.SendChatActionAsync(
            chatId: chatId, 
            chatAction: ChatAction.ChooseSticker, 
            cancellationToken: cancellationToken);
        
        if (ValidateTypeOfMessage(update))
        {
            botResponseMessage = NotAllowedMessage.Replace("{USER}", update.Message.Chat.Username);
            
           var storedMessage = await botClient.SendTextMessageAsync(
               chatId: chatId, 
               text: botResponseMessage, 
                cancellationToken: cancellationToken);
            
            await Task.Delay(typingIndicatorDelayInMilliseconds, cancellationToken);
            
            await botClient.DeleteMessageAsync(
                chatId:chatId, 
                messageId: messageId, 
                cancellationToken: cancellationToken);
            
            await Task.Delay(typingIndicatorDelayInMilliseconds, cancellationToken);
            
            await botClient.DeleteMessageAsync(
                chatId:chatId, 
                messageId: storedMessage.MessageId, 
                cancellationToken: cancellationToken);
        }
        else
        {
            await botClient.SendChatActionAsync(
                chatId: chatId, 
                chatAction: ChatAction.Typing, 
                cancellationToken: cancellationToken);
            
            botResponseMessage = TelegramMessageValidate(incomingTelegramMessage.Text, update);
            await botClient.SendTextMessageAsync(
                chatId: chatId, 
                text: botResponseMessage, 
                cancellationToken: cancellationToken);
        }
    }
}
