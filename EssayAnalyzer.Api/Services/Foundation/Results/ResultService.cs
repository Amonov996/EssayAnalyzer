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

    public IQueryable<Result> RetrieveAllResults() =>
        TryCatch(() => this.storageBroker.SelectAllResults());

    public ValueTask<Result> RetrieveResultByIdAsync(Guid id) =>
        TryCatch(async () =>
        {
            ValidateResultId(id);
            Result result = await this.storageBroker
                .SelectResultByIdAsync(id);

            ValidateResultIsExists(result, id);
            return result;
        });

    public ValueTask<Result> RemoveResultByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}