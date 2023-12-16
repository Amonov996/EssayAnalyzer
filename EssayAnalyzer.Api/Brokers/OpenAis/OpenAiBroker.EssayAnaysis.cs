using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayAnalyzer.Api.Brokers.OpenAis;

public partial class OpenAiBroker
{
    public async ValueTask<ChatCompletion> EssayAnalysisAsync(ChatCompletion chatCompletion)
    {
        return await this.openAiClient.ChatCompletions.SendChatCompletionAsync(chatCompletion);
    }
}