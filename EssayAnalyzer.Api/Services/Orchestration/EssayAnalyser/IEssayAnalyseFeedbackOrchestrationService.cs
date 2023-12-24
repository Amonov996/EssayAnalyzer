using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Models.Orchestration.Analyse;

namespace EssayAnalyzer.Api.Services.Orchestration.EssayAnalyser;

public interface IEssayAnalyseFeedbackOrchestrationService
{
    ValueTask<EssayAnalyse> AnalyseEssayFeedback(EssayAnalyse essayAnalyse);
}