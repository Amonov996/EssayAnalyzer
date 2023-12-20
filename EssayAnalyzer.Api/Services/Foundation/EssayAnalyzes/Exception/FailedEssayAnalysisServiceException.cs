using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes.Exception
{
    public class FailedEssayAnalysisServiceException : Xeption
    {
        public FailedEssayAnalysisServiceException(Xeption innerException)
            : base(message: "Failed analyse essay service error occured, contact support.",
                 innerException)
        { }
    }
}
