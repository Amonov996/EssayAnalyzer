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

    public ValueTask<Essay> AddEssayAsync(Essay essay)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IQueryable<Essay>> RetrieveEssaysAsync()
    {
         throw new NotImplementedException();
    }

    public ValueTask<Essay> RetrieveEssayByIdAsync(Guid id)
    {
         throw new NotImplementedException();
    }

    public ValueTask<Essay> RemoveEssayByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}