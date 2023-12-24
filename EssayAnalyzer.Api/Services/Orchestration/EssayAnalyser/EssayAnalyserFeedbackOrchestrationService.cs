using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Results;
using EssayAnalyzer.Api.Models.Orchestration.Analyse;
using EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes;
using EssayAnalyzer.Api.Services.Foundation.Essays;
using EssayAnalyzer.Api.Services.Foundation.Results;

namespace EssayAnalyzer.Api.Services.Orchestration.EssayAnalyser;

public class EssayAnalyserFeedbackOrchestrationService : IEssayAnalyseFeedbackOrchestrationService
{
    private readonly IEssayAnalysisService essayAnalysisService;
    private readonly IEssayService essayService;
    private readonly IResultService resultService;

    public EssayAnalyserFeedbackOrchestrationService(IEssayAnalysisService essayAnalysisService,
        IEssayService essayService, IResultService resultService)
    {
        this.essayAnalysisService = essayAnalysisService;
        this.essayService = essayService;
        this.resultService = resultService;
    }


    public async ValueTask<EssayAnalyse> AnalyseEssayFeedback(EssayAnalyse essayAnalyse)
    {
        essayAnalyse.Result = await this.essayAnalysisService
            .AnalyzeEssayAsync(essayAnalyse.Essay);

        await essayService.AddEssayAsync(essayAnalyse.Essay);
        await resultService.AddResultAsync(essayAnalyse.Result);
        
        return essayAnalyse;
    }
}