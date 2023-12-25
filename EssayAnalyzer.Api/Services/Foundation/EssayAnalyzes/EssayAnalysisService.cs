using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Brokers.OpenAis;
using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Results;
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

    public async ValueTask<Result> AnalyzeEssayAsync(Essay essay)
    {
        ChatCompletion request = CreateRequest(essay);
        
        ChatCompletion response = await this.openAiBroker
            .EssayAnalysisAsync(request);

        string? feedback = response.Response.Choices
            .FirstOrDefault()
            ?.Message.Content;

        Result result = new Result()
        {
            Id = Guid.NewGuid(),
            EssayId = essay.Id,
            Essay = essay,
            Feedback = feedback
        };
        
        return result;
    }


    private static ChatCompletion CreateRequest(Essay essay)
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
                        Content = "You are IELTS Writing examiner. Give detailed IELTS feedback based on " +
                                  "marking criteria of IELTS and give potential band score for each criteria" +
                                  "and give the overall band score in new line",
                        Role = "system",
                    },
                    new ChatCompletionMessage
                    {
                        Content = essay.Content,
                        Role = "user",
                    }
                },
            }
        };
    }

}