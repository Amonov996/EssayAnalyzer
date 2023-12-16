using EssayAnalyzer.Api.Models.Foundation.Results;
using Microsoft.EntityFrameworkCore;

namespace EssayAnalyzer.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Result> Results { get; set; }

        public async ValueTask<Result> InsertResultAsync(Result result) =>
            await InsertAsync(result);
    }
}
