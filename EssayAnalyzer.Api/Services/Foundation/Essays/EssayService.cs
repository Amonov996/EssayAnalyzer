using EssayAnalyzer.Api.Brokers.Loggings;
using EssayAnalyzer.Api.Models.Foundation.Essays;

namespace EssayAnalyzer.Api.Services.Foundation.Essays;

public partial class EssayService: IEssayService
{
    private readonly ILoggingBroker loggingBroker;
    private readonly IStorageBroker sotageBroker;
    
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

interface IStorageBroker { }