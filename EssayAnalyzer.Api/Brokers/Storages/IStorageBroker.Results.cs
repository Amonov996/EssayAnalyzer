using EssayAnalyzer.Api.Models.Foundation.Results;

namespace EssayAnalyzer.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Result> InsertResultAsync(Result result);
        ValueTask<Result> SelectResultByIdAsync(Guid id);
        IQueryable<Result> SelectAllResults();
    }
}
