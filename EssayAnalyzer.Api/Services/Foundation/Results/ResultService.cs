using EssayAnalyzer.Api.Brokers.Storages;
using EssayAnalyzer.Api.Models.Foundation.Results;

namespace EssayAnalyzer.Api.Services.Foundation.Results;

public partial class ResultService: IResultService
{
    private readonly IStorageBroker storageBroker;

    public ResultService(IStorageBroker storageBroker)
    {
        this.storageBroker = storageBroker;
    }

    public ValueTask<Result> AddResultAsync(Result result)
    {
        throw new NotImplementedException();
    }

    public ValueTask<ICollection<Result>> RetrieveAllResults()
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result> RetrieveResultByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result> RemoveResultByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}