using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayAnalyzer.Api.Brokers.OpenAis;

public partial interface IOpenAiBroker
{
    ValueTask<ChatCompletion> EssayAnalysisAsync(ChatCompletion chatCompletion);
}