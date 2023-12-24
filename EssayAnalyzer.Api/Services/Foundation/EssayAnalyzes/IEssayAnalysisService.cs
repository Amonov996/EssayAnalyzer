using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Results;

namespace EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes;

public interface IEssayAnalysisService
{
    public ValueTask<Result> AnalyzeEssayAsync(Essay essay);
}