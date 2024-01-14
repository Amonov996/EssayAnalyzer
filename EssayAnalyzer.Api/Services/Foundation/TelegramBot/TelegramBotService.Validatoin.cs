
using EssayAnalyzer.Api.Models.Foundation.Essays;

namespace EssayAnalyzer.Api.Services.Foundation.TelegramBot;

public partial class TelegramBotService
{
    private string TelegramMessageValidate(string message)
    {
        var trimMessage = message.Trim();

        return trimMessage switch
        {
            "/start" => WelcomeMessage,
            "/help" => HelpMessage,
            _ => ValidateCountOfTelegramMessage(trimMessage)
        };
    }

    private string ValidateCountOfTelegramMessage(string message)
    {
        var countOfWords = message.Split(',', ' ','.', '!', '?','\n').Length;
        var countOfCharacters = message.Length;

        if (countOfWords > 450 || countOfCharacters > 3000)
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
}