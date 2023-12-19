using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Brokers.Storages;
using EssayAnalyzer.Api.Models.Foundation.Essays;

namespace EssayAnalyzer.Api.Services.Foundation.Essays;

public partial class EssayService: IEssayService
{
    private readonly ILoggingBroker loggingBroker;
    private readonly IStorageBroker storageBroker;

    public EssayService(ILoggingBroker loggingBroker, IStorageBroker storageBroker)
    {
        this.loggingBroker = loggingBroker;
        this.storageBroker = storageBroker;
    }

    public ValueTask<Essay> AddEssayAsync(Essay essay) =>
                TryCatch(async () =>
                {
                    ValidateEssay(essay);
                    return await this.storageBroker.InsertEssayAsync(essay);
                });
               
    public IQueryable<Essay> RetrieveAllEssays() => 
        TryCatch(() => this.storageBroker.SelectAllEssays());

    public ValueTask<Essay> RetrieveEssayByIdAsync(Guid id) =>
        TryCatch(async () =>
        {
            ValidateEssayId(id);
            Essay maybeEssay = await this.storageBroker.SelectEssayByIdAsync(id);

            ValidateStorageEssayExists(maybeEssay, id);

            return maybeEssay;

        });

    public ValueTask<Essay> RemoveEssayByIdAsync(Guid id) => 
        throw new NotImplementedException();
}