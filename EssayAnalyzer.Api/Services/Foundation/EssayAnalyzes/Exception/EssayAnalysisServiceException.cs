using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes.Exception;

public class EssayAnalysisServiceException : Xeption
{
    public EssayAnalysisServiceException(Xeption innerException)
        : base(message: "OpenAi Service error occured, contact support.", innerException)
    { }
}