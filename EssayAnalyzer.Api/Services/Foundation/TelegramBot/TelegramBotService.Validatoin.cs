using EssayAnalyzer.Api.Models.Foundation.Essays;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace EssayAnalyzer.Api.Services.Foundation.TelegramBot;

public partial class TelegramBotService
{
    private string TelegramMessageValidate(string message, Update update)
    {
        var trimMessage = message.Trim();

        return trimMessage switch
        {
            "/start" => WelcomeMessage.Replace("{USER}", update.Message.Chat.FirstName),
            "/help" => HelpMessage,
            _ => ValidateCountOfTelegramMessage(trimMessage)
        };
    }

    private string ValidateCountOfTelegramMessage(string message)
    {
        var countOfWords = message.Split(' ').Length;
        var countOfCharacters = message.Length;

        if (countOfWords > 500 || countOfCharacters > 3500)
            return LongEssayMessage;
        
        if (countOfWords < 100 || countOfCharacters < 300)
            return ShortEssayMessage;

        var result = this.essayAnalysisService.AnalyzeEssayAsync(new Essay
        {
            Id = Guid.NewGuid(),
            Content = message
        });
        
        return result.Result.Feedback;
    }

    private bool ValidateTypeOfMessage(Update message) => 
        message.Message.Type != MessageType.Text;

}