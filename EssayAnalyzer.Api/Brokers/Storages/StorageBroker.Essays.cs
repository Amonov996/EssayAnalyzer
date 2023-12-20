using EssayAnalyzer.Api.Models.Foundation.Essays;
using Microsoft.EntityFrameworkCore;

namespace EssayAnalyzer.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Essay> Essays { get; set; }

        public IQueryable<Essay> SelectAllEssays() => 
            SelectAll<Essay>();
        
        public async ValueTask<Essay> InsertEssayAsync(Essay essay) =>
            await InsertAsync(essay);

        public async ValueTask<Essay> SelectEssayByIdAsync(Guid id) =>
            await SelectAsync<Essay>(id);
        
        public async ValueTask<Essay> DeleteEssayAsync(Essay essay) => 
            await DeleteAsync<Essay>(essay);
    }
}