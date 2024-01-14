namespace EssayAnalyzer.Api.Services.Foundation.TelegramBot;

public partial class TelegramBotService
{
    private const string WelcomeMessage = $"\nHey there, {{USER}}! \ud83d\ude0a I hope you're having an awesome day! \ud83c\udf1f " +
                                          $"Feel free to share your essay with me – I'd be delighted to help you out! \ud83d\udcdd\u2728 " +
                                          $"Just make sure it falls within the range of 100 to 450 words, and keep it under 3000 characters. " +
                                          $"Can't wait to see what you've got! \ud83d\ude80\ud83d\udcda";
    
    private const string HelpMessage = $"\nHey! \ud83d\ude0a If writing isn't your thing, no worries! \ud83c\udf1f " +
                                       $"Just send me a text with your essay, making sure it's between 100 to 450" +
                                       $" words and stays under 3000 characters. \ud83d\udcdd Need help counting? " +
                                       $"Check out {{source}} for a quick character count! \ud83d\ude80 Can't wait to " +
                                       $"assist you! \ud83d\udcda\u2728";
   
    
    private const string ShortEssayMessage = "\ud83d\ude0a It looks like the essay you sent is a bit " +
                                             "too short for a thorough check. \ud83d\udcdd";
    
    private const string LongEssayMessage = "Hey! \ud83d\ude0a It seems like your essay is a bit longer than expected. " +
                                            "\ud83d\udcdd\u2728 Please make sure it's within the range of 100 to 450 " +
                                            "words to ensure a more effective check. ";

    private const string NotAllowedMessage = $"Hey {{USER}} you sent me wrong type of message! " +
                                             "I can ryple only in Text messages!";
}