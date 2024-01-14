using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace EssayAnalyzer.Api.Services.Foundation.TelegramBot;

public partial class TelegramBotService
{
    private static async Task<int> SendMessageAsync(ITelegramBotClient botClient, long chatId, string messageText, 
        CancellationToken cancellationToken)
    {
         var storedMessageId = await botClient.SendTextMessageAsync(
            chatId: chatId, 
            text: messageText,
            cancellationToken: cancellationToken);
         
         return storedMessageId.MessageId;
    }
    
    private static async Task DeleteMessageAsync(ITelegramBotClient botClient, long chatId, int messageId,
        CancellationToken cancellationToken)
    {
        await botClient.DeleteMessageAsync(
            chatId:chatId, 
            messageId: messageId, 
            cancellationToken: cancellationToken);
    }
    
    private static async Task BotChatActionAsync(ITelegramBotClient botClient, long chatId, ChatAction action,
        CancellationToken cancellationToken)
    {
        await botClient.SendChatActionAsync(
            chatId: chatId, 
            chatAction: action, 
            cancellationToken: cancellationToken);
    }
}
