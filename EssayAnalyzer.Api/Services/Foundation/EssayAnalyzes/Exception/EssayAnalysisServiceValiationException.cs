using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes.Exception
{
    public class EssayAnalysisServiceValiationException : Xeption
    {
        public EssayAnalysisServiceValiationException(Xeption innerException)
           : base("Chat completion validation error occurred, fix errors and try again.", innerException)
        { }
    }
}
