using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes.Exception;

public class EssayAnalysisServiceValidationException : Xeption
{
    public EssayAnalysisServiceValidationException(Xeption innerException)
       : base("Chat completion validation error occurred, fix errors and try again.", innerException)
    { }
}
