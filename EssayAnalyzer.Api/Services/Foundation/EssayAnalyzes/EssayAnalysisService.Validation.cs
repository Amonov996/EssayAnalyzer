using EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes.Exception;

namespace EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes
{
    public partial class EssayAnalysisService
    {
        private static void ValidateEssayAnalysisOnAdd(string essay)
        {
            ValidateEssayAnalysisIsNotNull(essay);
        }
        
        private static void ValidateEssayAnalysisIsNotNull(string essay)
        {
            if (essay is null)
            {
                throw new NullEssayAnalysisException();
            }
        }
    }
}
