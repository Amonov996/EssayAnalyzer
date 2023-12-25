using EssayAnalyzer.Api.Models.Foundation.Essays;
using EssayAnalyzer.Api.Models.Foundation.Results;

namespace EssayAnalyzer.Api.Models.Orchestration.Analyse;

public class EssayAnalyse
{
    public Essay? Essay { get; set; }

    public Result? Result { get; set; }
}