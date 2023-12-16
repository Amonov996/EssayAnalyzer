using Standard.AI.OpenAI.Clients.OpenAIs;
using Standard.AI.OpenAI.Models.Configurations;

namespace EssayAnalyzer.Api.Brokers.OpenAis;

public partial class OpenAiBroker : IOpenAiBroker
{
    private readonly IOpenAIClient openAiClient;
    private readonly IConfiguration configuration;

    public OpenAiBroker(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.openAiClient = ConfigureOpenAiClient();
    }

    private IOpenAIClient ConfigureOpenAiClient()
    {
        var apiKey = configuration.GetValue<string>(key: "OpenAiKey");
        var openAiConfiguration = new OpenAIConfigurations()
        {
            ApiKey = apiKey
        };
        
        return new OpenAIClient(openAiConfiguration);
    }
}