using EssayAnalyzer.Api.Models.Foundation.Essays;

namespace EssayAnalyzer.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Essay> InsertEssayAsync(Essay essay);
        IQueryable<Essay> SelectAllEssays();
        ValueTask<Essay> SelectEssayByIdAsync(Guid id);
    }
}
