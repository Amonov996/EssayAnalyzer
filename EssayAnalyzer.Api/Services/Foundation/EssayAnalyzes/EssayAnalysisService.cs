using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Brokers.OpenAis;
using Standard.AI.OpenAI.Models.Services.Foundations.ChatCompletions;

namespace EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes;

public partial class EssayAnalysisService : IEssayAnalysisService
{
    private readonly IOpenAiBroker openAiBroker;
    private readonly ILoggingBroker loggingBroker;

    public EssayAnalysisService(
        IOpenAiBroker openAiBroker,
        ILoggingBroker loggingBroker)
    {
        this.openAiBroker = openAiBroker;
        this.loggingBroker = loggingBroker;
    }

    public ValueTask<string> AnalyzeEssayAsync(string essay) =>
        TryCatch(async () =>
        {
            ValidateEssayAnalysisIsNotNull(essay);

            ChatCompletion request = CreateRequest(essay);
            ChatCompletion response = 
                await this.openAiBroker.EssayAnalysisAsync(request);

            return response.Response.Choices.FirstOrDefault().Message.Content;
        });

    private static ChatCompletion CreateRequest(string essay)
    {
        return new ChatCompletion
        {
            Request = new ChatCompletionRequest
            {
                Model = "gpt-4-1106-preview",
                MaxTokens = 1500,
                Messages = new ChatCompletionMessage[]
                {
                    new ChatCompletionMessage
                    {
                        Content = "You are IELTS Writing examiner. Give detailed IELTS feedback based on marking criteria of IELTS",
                        Role = "system",
                    },
                    new ChatCompletionMessage
                    {
                        Content = essay,
                        Role = "user",
                    }
                },
            }
        };
    }
}