namespace EssayAnalyzer.Api.Services.Foundation.EssayAnalyzes
{
    public interface IEssayAnalysisService
    {
        public ValueTask<string> AnalyzeEssayAsync(string essay);
    }
}