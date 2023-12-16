using EssayAnalyzer.Api.Models.Foundation.Results;

namespace EssayAnalyzer.Api.Services.Foundation.Results;

public interface IResultService
{
    ValueTask<Result> AddResultAsync(Result result);
    ValueTask<ICollection<Result>> RetrieveAllResults();
    ValueTask<Result> RetrieveResultByIdAsync(Guid id);
    ValueTask<Result> RemoveResultByIdAsync(Guid id);
}