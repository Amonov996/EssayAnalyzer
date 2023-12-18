using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Brokers.Storages;
using EssayAnalyzer.Api.Models.Foundation.Results;

namespace EssayAnalyzer.Api.Services.Foundation.Results;

public partial class ResultService: IResultService
{
    private readonly IStorageBroker storageBroker;
    public readonly ILoggingBroker loggingBroker;

    public ResultService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
    {
        this.storageBroker = storageBroker;
        this.loggingBroker = loggingBroker;
    }

    public ValueTask<Result> AddResultAsync(Result result) =>
        TryCatch(async () =>
        {
            ValidateResultOnAdd(result);

            return await this.storageBroker.InsertResultAsync(result);
        });

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