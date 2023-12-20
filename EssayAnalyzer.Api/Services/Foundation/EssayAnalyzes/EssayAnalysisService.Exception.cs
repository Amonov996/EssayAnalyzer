using EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes.Exception;
using Xeptions;

namespace EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes;

public partial class EssayAnalysisService
{
    private delegate ValueTask<string> ReturnEssayAnalysisAsync();

    private async ValueTask<string> TryCatch(ReturnEssayAnalysisAsync returnEssayAnalysisAsync)
    {
        try
        {
            return await returnEssayAnalysisAsync();
        }
        catch (NullEssayAnalysisException nullEssayAnalysisException)
        {
            throw CreateAndLogValidationException(nullEssayAnalysisException);
        }
    }
    private EssayAnalysisServiceValiationException CreateAndLogValidationException(Xeption exceptionn)
    {
        var essayAnalysisServiceValiationException =
            new EssayAnalysisServiceValiationException(exceptionn);

        this.loggingBroker.LogError(essayAnalysisServiceValiationException);
        return essayAnalysisServiceValiationException;
    }
}